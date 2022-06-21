﻿using System.ComponentModel.DataAnnotations;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.Options;

namespace LightBoard.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly ICodeService _codeService;
        private readonly IUserInfoService _userInfoService;
        private readonly IEmailService _emailService;
        private readonly IHashingProvider _hashingProvider;
        private readonly AuthOptions _authOptions;
        private readonly EmailTemplatesOptions _emailTemplatesOptions;

        public ProfileService(
            IUnitOfWork unitOfWork, 
            IApplicationMapper applicationMapper, 
            IBlobService blobService,
            IUserInfoService userInfoService, 
            IOptions<AuthOptions> authOptions, 
            IOptions<EmailTemplatesOptions> emailTemplateOptions, 
            ICodeService codeService, 
            IHashingProvider hashingProvider, 
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = applicationMapper;
            _blobService = blobService;
            _userInfoService = userInfoService;
            _authOptions = authOptions.Value;
            _emailTemplatesOptions = emailTemplateOptions.Value;
            _codeService = codeService;
            _hashingProvider = hashingProvider;
            _emailService = emailService;
        }
        
        public async Task RequestEmailConfirmation()
        {
            var user = await _unitOfWork.Users.GetById(_userInfoService.UserId);
            var confirmEmailCode = await _unitOfWork.GeneratedCodes.GetLastByEmail<ConfirmEmailCode>(user.Email);

            if (user.IsEmailConfirmed)
            {
                throw new ValidationException("Email is already confirmed.");
            }

            if (confirmEmailCode is null || confirmEmailCode.ExpirationDate < DateTime.UtcNow)
            {
                confirmEmailCode = _codeService.GenerateCode<ConfirmEmailCode>(
                    symbolCount: 8, 
                    user.Email,
                    _authOptions.CodeMinutesLifetime);
                
                _unitOfWork.GeneratedCodes.Add(confirmEmailCode);
                
                await _unitOfWork.SaveChangesAsync();
            }

            if (user.Email != confirmEmailCode.Email)
            {
                throw new ValidationException("Code is invalid");
            }

            var mailTemplate = await _blobService.GetBlobStringContentAsync(
                BlobContainer.MailTemplates, _emailTemplatesOptions.EmailConfirmationTemplateFilename);
            var mailArgs = new SendMailArgs(user.Email, "Email confirmation code", mailTemplate);
            await _emailService.SendConfirmEmail(mailArgs, mailTemplate, confirmEmailCode.ResetCode, user.Name);
        }

        public async Task ConfirmEmail(string confirmEmailCode)
        {
            var user = await _unitOfWork.Users.GetById(_userInfoService.UserId);
            var confirmEmailCodeEntity = await _unitOfWork.GeneratedCodes.GetLastByEmail<ConfirmEmailCode>(user.Email);
            
            if (confirmEmailCodeEntity is null || 
                confirmEmailCodeEntity.ResetCode != confirmEmailCode || 
                confirmEmailCodeEntity.Email != user.Email)
            {
                throw new ValidationException("Code is invalid");
            }

            user.IsEmailConfirmed = true;
            _unitOfWork.Users.Update(user);
            
            _unitOfWork.GeneratedCodes.Delete(confirmEmailCodeEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RequestPasswordReset(ResetPasswordEmailRequest request)
        {
            if (!await _unitOfWork.Users.IsExists(request.Email))
            {
                return;
            }
        
            var resetCodeEmail = await _unitOfWork.GeneratedCodes.GetLastByEmail<ResetPasswordCode>(request.Email);
            
            if (resetCodeEmail is null || resetCodeEmail.ExpirationDate < DateTime.UtcNow)
            {
                resetCodeEmail =  _codeService.GenerateCode<ResetPasswordCode>(
                    symbolCount: 8, 
                    request.Email,
                    _authOptions.CodeMinutesLifetime);
        
                _unitOfWork.GeneratedCodes.Add(resetCodeEmail);
                
                await _unitOfWork.SaveChangesAsync();
            }

            var mailTemplate = await _blobService.GetBlobStringContentAsync(
                BlobContainer.MailTemplates, _emailTemplatesOptions.PasswordResetTemplateFilename);
            var mailArgs = new SendMailArgs(request.Email, "Email confirmation code", mailTemplate);
            await _emailService.SendResetCode(mailArgs, mailTemplate, resetCodeEmail.ResetCode);
        }

        public async Task UpdatePassword(UpdatePasswordRequest request)
        {
            var user = await _unitOfWork.Users.GetById(_userInfoService.UserId);
            
            if (!_hashingProvider.Verify(request.OldPassword, user.PasswordHash))
            {
                throw new ValidationException("Current password is invalid.");
            }

            user.PasswordHash = _hashingProvider.GetHash(request.Password);
            
            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var resetCodeEmail = await _unitOfWork.GeneratedCodes.GetByResetCode<ResetPasswordCode>(request.ResetCode);
            if (resetCodeEmail is null || resetCodeEmail.ResetCode != request.ResetCode)
            {
                throw new ValidationException("Code is invalid");
            }

            var user = await _unitOfWork.Users.Get(resetCodeEmail.Email);
        
            if (user is null)
            {
                throw new InvalidOperationException("User is required for this operation.");
            }

            user.PasswordHash = _hashingProvider.GetHash(request.Password);
            
            _unitOfWork.GeneratedCodes.Delete(resetCodeEmail);
            _unitOfWork.Users.Update(user);
            
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<UpdateAvatarResponse> UpdateAvatar(UpdateAvatarRequest request)
        {
            var args = new UploadFormFileArgs()
            {
                 Container = BlobContainer.UserAvatars,
                 Purpose = BlobPurpose.Inline,
                 FormFile = request.File
            };
            var result = await _blobService.UploadFormFile(args);
            
            var userId = _userInfoService.UserId;
            var user = await _unitOfWork.Users.GetById(userId);

            if (user.AvatarBlobName != null)
            {
                await _blobService.DeleteFile(BlobContainer.UserAvatars, user.AvatarBlobName);
            }
            
            user.AvatarUrl = result.Uri;
            user.AvatarBlobName = result.BlobName;
            
            await _unitOfWork.SaveChangesAsync();
            
            return new UpdateAvatarResponse() 
            {
                UserAvatarUrl = user.AvatarUrl
            };
        }

        public async Task<UserProfileResponse> GetProfile()
        {
            var userId = _userInfoService.UserId;
            var user = await _unitOfWork.Users.GetById(userId);
             
            return _mapper.ToUserProfileResponse(user);
        }
    }
}