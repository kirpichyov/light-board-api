using Bogus;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Tests.Common;

public class ModelFaker
{
    private readonly Faker _faker = new();
    
    public User GivenUser()
    {
        return new User(_faker.Person.Email, _faker.Person.FirstName, _faker.Person.LastName, _faker.Internet.Password());
    }
}