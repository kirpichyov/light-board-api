﻿using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class UsersRepository : RepositoryBase<User, Guid>, IUsersRepository
{
    public UsersRepository(PostgreSqlContext context)
        : base(context)
    {
    }

    public async Task<bool> IsExists(string email)
    {
        return await Context.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> Get(string email)
    {
        return await Context.Users.SingleOrDefaultAsync(user => user.Email == email);
    }
}