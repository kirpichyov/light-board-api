using Bogus;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Mapping;
using LightBoard.Application.Models.Auth;
using LightBoard.Application.Services;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Shared.Api;
using LightBoard.Shared.Exceptions;
using LightBoard.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LightBoard.UnitTests;

public class AuthServiceTests
{
    private readonly Faker _faker = new();
    private readonly ModelFaker _modelFaker = new();
    
    private readonly UnitOfWorkFakeWrapper _unitOfWorkFakeWrapper;
    private readonly Fake<IHashingProvider> _hashingProviderFake;
    private readonly Fake<IKeysGenerator> _keysGeneratorFake;
    private readonly IOptions<AuthOptions> _authOptions;
    private readonly Fake<IHttpContextAccessor> _httpAccessorAccessorFake;
    private readonly Fake<IUserSessionsRepository> _userSessionsRepositoryFake;

    public AuthServiceTests()
    {
        _hashingProviderFake = new Fake<IHashingProvider>();
        _keysGeneratorFake = new Fake<IKeysGenerator>();
        _httpAccessorAccessorFake = new Fake<IHttpContextAccessor>();
        _userSessionsRepositoryFake = new Fake<IUserSessionsRepository>();
        _unitOfWorkFakeWrapper = new UnitOfWorkFakeWrapper();
        
        _authOptions = Options.Create(new AuthOptions()
        {
            SessionKeyLength = 64
        });
    }

    [Fact]
    public async Task CreateUser_EmailIsFree_ShouldBeEquivalentToExpected()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = _faker.Person.Email,
            Name = _faker.Person.FirstName,
            Password = _faker.Internet.Password(),
        };

        string key = _faker.Random.Hash();

        _unitOfWorkFakeWrapper.Users
            .CallsTo(repository => repository.IsExists(request.Email))
            .Returns(false);

        _keysGeneratorFake
            .CallsTo(generator => generator.Generate(_authOptions.Value.SessionKeyLength))
            .Returns(key);

        _hashingProviderFake
            .CallsTo(provider => provider.GetHash(request.Password))
            .Returns(_faker.Random.Hash());
        
        var sut = BuildSut();

        // Act
        var result = await sut.CreateUser(request);

        // Assert
        using (new AssertionScope())
        {
            result.SessionKey.Should().NotBeNullOrEmpty();
            result.SessionKey.Should().Be(key);

            result.CreatedUserInfo.Should().NotBeNull();
            result.CreatedUserInfo.Email.Should().Be(request.Email);
            result.CreatedUserInfo.Name.Should().Be(request.Name);

            _unitOfWorkFakeWrapper.Users
                .CallsTo(repository => repository.Add(A<User>._))
                .MustHaveHappenedOnceExactly();

            _unitOfWorkFakeWrapper.Fake
                .CallsTo(unitOfWork => unitOfWork.SaveChangesAsync())
                .MustHaveHappenedOnceExactly();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.AddAsync(A<UserSession>._))
                .MustHaveHappenedOnceExactly();
        }
    }

    [Fact]
    public async Task CreateUser_EmailIsAlreadyExist_ShouldThrowValidationFailedException()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = _faker.Person.Email,
            Name = _faker.Person.FirstName,
            Password = _faker.Internet.Password(),
        };

        _unitOfWorkFakeWrapper.Users
            .CallsTo(repository => repository.IsExists(request.Email))
            .Returns(true);

        var sut = BuildSut();

        // Act
        var func = async () => await sut.CreateUser(request);

        // Assert
        using (new AssertionScope())
        {
            await func.Should().ThrowExactlyAsync<ValidationFailedException>();
            
            _unitOfWorkFakeWrapper.Users
                .CallsTo(repository => repository.Add(A<User>._))
                .MustNotHaveHappened();

            _unitOfWorkFakeWrapper.Fake
                .CallsTo(unitOfWork => unitOfWork.SaveChangesAsync())
                .MustNotHaveHappened();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.AddAsync(A<UserSession>._))
                .MustNotHaveHappened();
        }
    }

    [Fact]
    public async Task CreateUserSession_CredentialsIsValid_ShouldBeEquivalentToExpected()
    {
        // Arrange
        var user = _modelFaker.GivenUser();
        
        var request = new SignInRequest
        {
            Email = user.Email,
            Password = _faker.Internet.Password()
        };

        var key = _faker.Random.Hash();

        _unitOfWorkFakeWrapper.Users
            .CallsTo(repository => repository.Get(user.Email))
            .Returns(user);

        _hashingProviderFake
            .CallsTo(provider => provider.Verify(request.Password, user.PasswordHash))
            .Returns(true);

        _keysGeneratorFake
            .CallsTo(generator => generator.Generate(_authOptions.Value.SessionKeyLength))
            .Returns(key);

        var sut = BuildSut();

        // Act
        var result = await sut.CreateUserSession(request);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(key);
            
            _userSessionsRepositoryFake
                .CallsTo(repository => repository.AddAsync(A<UserSession>._))
                .MustHaveHappenedOnceExactly();
        }
    }
    
    [Fact]
    public async Task CreateUserSession_PasswordIsInvalid_ShouldBeEquivalentToExpected()
    {
        // Arrange
        var user = _modelFaker.GivenUser();
        
        var request = new SignInRequest
        {
            Email = user.Email,
            Password = _faker.Internet.Password()
        };

        _unitOfWorkFakeWrapper.Users
            .CallsTo(repository => repository.Get(user.Email))
            .Returns(user);

        _hashingProviderFake
            .CallsTo(provider => provider.Verify(request.Password, user.PasswordHash))
            .Returns(false);

        var sut = BuildSut();

        // Act
        var func = async () => await sut.CreateUserSession(request);

        // Assert
        using (new AssertionScope())
        {
            await func.Should().ThrowExactlyAsync<ValidationFailedException>();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.AddAsync(A<UserSession>._))
                .MustNotHaveHappened();
        }
    }
    
        
    [Fact]
    public async Task CreateUserSession_EmailIsInvalid_ShouldBeEquivalentToExpected()
    {
        // Arrange
        User user = null;
        
        var request = new SignInRequest
        {
            Email = _faker.Person.Email,
            Password = _faker.Internet.Password()
        };

        _unitOfWorkFakeWrapper.Users
            .CallsTo(repository => repository.Get(request.Email))
            .Returns(user);

        var sut = BuildSut();

        // Act
        var func = async () => await sut.CreateUserSession(request);

        // Assert
        using (new AssertionScope())
        {
            await func.Should().ThrowExactlyAsync<ValidationFailedException>();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.AddAsync(A<UserSession>._))
                .MustNotHaveHappened();
        }
    }

    [Fact]
    public async Task DeleteSession_SessionKeyIsPresent_ShouldInvokeServices()
    {
        // Arrange
        var sessionKey = _faker.Random.Hash();
        
        var httpContext = new DefaultHttpContext()
        {
            Request =
            {
                Headers =
                {
                    new KeyValuePair<string, StringValues>(ApiHeaders.SessionKey, sessionKey)
                }
            }
        };

        _httpAccessorAccessorFake
            .CallsTo(accessor => accessor.HttpContext)
            .Returns(httpContext);

        var sut = BuildSut();

        // Act
        await sut.DeleteSession();

        // Assert
        using (new AssertionScope())
        {
            _httpAccessorAccessorFake
                .CallsTo(accessor => accessor.HttpContext)
                .MustHaveHappenedOnceExactly();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.RemoveAsync(sessionKey))
                .MustHaveHappenedOnceExactly();
        }
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task DeleteSession_SessionKeyIsEmpty_ShouldInvokeServices(string sessionKey)
    {
        // Arrange
        var httpContext = new DefaultHttpContext()
        {
            Request =
            {
                Headers =
                {
                    new KeyValuePair<string, StringValues>(ApiHeaders.SessionKey, sessionKey)
                }
            }
        };

        _httpAccessorAccessorFake
            .CallsTo(accessor => accessor.HttpContext)
            .Returns(httpContext);

        var sut = BuildSut();

        // Act
        var func = async () => await sut.DeleteSession();

        // Assert
        using (new AssertionScope())
        {
            await func.Should().ThrowExactlyAsync<ValidationFailedException>();
            
            _httpAccessorAccessorFake
                .CallsTo(accessor => accessor.HttpContext)
                .MustHaveHappenedOnceExactly();

            _userSessionsRepositoryFake
                .CallsTo(repository => repository.RemoveAsync(sessionKey))
                .MustNotHaveHappened();
        }
    }
    
    AuthService BuildSut() => new(
        _unitOfWorkFakeWrapper.FakedObject,
        new ApplicationMapper(),
        _hashingProviderFake.FakedObject,
        _keysGeneratorFake.FakedObject,
        _authOptions,
        _httpAccessorAccessorFake.FakedObject,
        _userSessionsRepositoryFake.FakedObject
    );
}