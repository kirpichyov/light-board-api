﻿using System.Security.Claims;
using System.Text.Encodings.Web;
using LightBoard.Application.Abstractions.Options;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Shared.Api;
using LightBoard.Shared.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LightBoard.Api.Middleware.SessionKey;

public class SessionAuthHandler : AuthenticationHandler<SessionAuthSchemeOptions>
{
    private readonly IUserSessionsCache _userSessions;
    private readonly AuthOptions _authOptions;

    public SessionAuthHandler(IOptionsMonitor<SessionAuthSchemeOptions> options,
        IOptions<AuthOptions> identityOptions,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserSessionsCache userSessions)
        : base(options, logger, encoder, clock)
    {
        _userSessions = userSessions;
        _authOptions = identityOptions.Value;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return AuthenticateResult.NoResult();
        }

        if (!Request.Headers.ContainsKey(ApiHeaders.SessionKey))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        string sessionKey = Request.Headers[ApiHeaders.SessionKey].ToString();

        if (string.IsNullOrEmpty(sessionKey) || sessionKey.Length != _authOptions.SessionKeyLength)
        {
            return AuthenticateResult.Fail("Session key is invalid.");
        }

        UserSession? session = await _userSessions.FetchAsync(sessionKey);
        if (session is null)
        {
            return AuthenticateResult.Fail("Session key is invalid or revoked.");
        }

        if (session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            return AuthenticateResult.Fail("Session key is expired.");
        }

        var claims = new Claim[]
        {
            new(CustomClaimTypes.UserId, session.UserId.ToString())
        };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, nameof(SessionAuthHandler));
        AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), ApiSchemas.SessionKeyScheme);

        return AuthenticateResult.Success(ticket);
    }
}