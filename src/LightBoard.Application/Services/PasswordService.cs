using System.ComponentModel.DataAnnotations;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.Options;

namespace LightBoard.Application.Services;

public class PasswordService : IPasswordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfoService;
    private readonly IEmailService _emailService;
    private readonly IKeysGenerator _keysGenerator;
    private readonly IHashingProvider _hashingProvider;
    private readonly AuthOptions _authOptions;

    public PasswordService(IUnitOfWork unitOfWork, IUserInfoService userInfoService, IHashingProvider hashingProvider, 
        IKeysGenerator keysGenerator, IEmailService emailService, IOptions<AuthOptions> authOptions)
    {
        _unitOfWork = unitOfWork;
        _userInfoService = userInfoService;
        _hashingProvider = hashingProvider;
        _keysGenerator = keysGenerator;
        _emailService = emailService;
        _authOptions = authOptions.Value;
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

    public async Task RequestPasswordReset(ResetPasswordEmailRequest request)
    {
        if (!await _unitOfWork.Users.IsExists(request.Email))
        {
            return;
        }
        
        var resetCodeEmail = await _unitOfWork.ResetCodeEmails.GetByEmail(request.Email);
        if (resetCodeEmail is null || resetCodeEmail.ExpirationDate < DateTime.UtcNow)
        {
            resetCodeEmail =  new ResetPasswordCode(
                _keysGenerator.Generate(8), 
                request.Email,
                DateTime.UtcNow.AddMinutes(_authOptions.PasswordMinutesLifetime));
        
            _unitOfWork.ResetCodeEmails.Add(resetCodeEmail);
            await _unitOfWork.SaveChangesAsync();
        }

        var mailArgs = new SendMailArgs(request.Email, "Verification code", "It's your verification code:" + resetCodeEmail.ResetCode);
        _emailService.Send(mailArgs);
    }

    public async Task ResetPassword(ResetPasswordRequest request)
    {
        var resetCodeEmail = await _unitOfWork.ResetCodeEmails.GetByResetCode(request.ResetCode);
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
        _unitOfWork.ResetCodeEmails.Delete(resetCodeEmail);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
}