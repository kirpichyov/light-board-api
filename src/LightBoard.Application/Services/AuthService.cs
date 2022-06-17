using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Models.Users;
using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Shared.Api;
using LightBoard.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LightBoard.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationMapper _mapper;
    private readonly IHashingProvider _hashingProvider;
    private readonly IKeysGenerator _keysGenerator;
    private readonly IUserInfoService _userInfoService;
    private readonly AuthOptions _authOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserSessionsRepository _userSessionsRepository;

    public AuthService(
        IUnitOfWork unitOfWork,
        IApplicationMapper mapper,
        IHashingProvider hashingProvider,
        IKeysGenerator keysGenerator,
        IOptions<AuthOptions> authOptions,
        IHttpContextAccessor httpContextAccessor,
        IUserSessionsRepository userSessionsRepositoryRepository,
        IUserInfoService userInfoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hashingProvider = hashingProvider;
        _keysGenerator = keysGenerator;
        _httpContextAccessor = httpContextAccessor;
        _userSessionsRepository = userSessionsRepositoryRepository;
        _userInfoService = userInfoService;
        _authOptions = authOptions.Value;
    }
    public async Task<(UserInfoResponse CreatedUserInfo, string SessionKey)> CreateUser(RegisterRequest request)
    {
        bool emailInUse = await _unitOfWork.Users.IsExists(request.Email);

        if (emailInUse)
        {
            throw new ValidationFailedException("Email is already in use");
        }
        
        User user = _mapper.ToUser(request, _hashingProvider);
        
        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync();

        var session = await PersistSession(user);
        var createdUser = _mapper.ToUserInfoResponse(user);

        return (createdUser, session.Key);
    }

    public async Task<string> CreateUserSession(SignInRequest request)
    {
        User? user = await _unitOfWork.Users.Get(request.Email);

        if (user is null || !_hashingProvider.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationFailedException("Credentials are invalid");
        }

        var session = await PersistSession(user);
        return session.Key;
    }

    public async Task DeleteSession()
    {
        bool isHeaderPresent = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(ApiHeaders.SessionKey, out StringValues headerValue);

        if (!isHeaderPresent || string.IsNullOrEmpty(headerValue))
        {
            throw new ValidationFailedException($"Session key in '{ApiHeaders.SessionKey}' header is required for logout operation");
        }

        await _userSessionsRepository.RemoveAsync(headerValue);
    }

    private async Task<UserSession> PersistSession(User user)
    {
        var key = _keysGenerator.Generate(_authOptions.SessionKeyLength);
        var expiresAt = DateTime.UtcNow.AddDays(_authOptions.SessionDaysLifetime);
        var session = new UserSession(user, key, DateTime.UtcNow, expiresAt);

        await _userSessionsRepository.AddAsync(session);

        return session;
    }
}