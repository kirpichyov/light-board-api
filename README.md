# Light Board API
LightBoard API is a backend for Trello analogue application developed with ASP.NET Core 6.
[Project documentation.](https://github.com/kirpichyov/light-board-api/wiki)


## Local environment without Docker
- [Download PostgreSQL](https://www.postgresql.org/download/)
- Set `UseInMemoryInsteadRedis: true` in the **appsettings.json** to use PC RAM instead of Redis.   

## Local environment with Docker

### Prerequisites
- Docker installed ([see documentation](https://docs.microsoft.com/en-us/windows/wsl/install))
- Docker desktop installed (optionally)
- Images is not already deployed

### PostgreSql

```
docker run -p 5432:5432 --name postgresql_db -e POSTGRES_PASSWORD=postgres -d postgres
```

### Redis


```
docker run --name redis_local -d -p 6379:6379 redis
```

---

## EF Migrations

### Prerequisites
- dotnet ef is installed ([How to install](https://docs.microsoft.com/en-us/ef/core/cli/dotnet))
- Commands is executed from **LightBoard.Api project** (you can use `cd <your_path_to_api_project>` command)

1. To apply migrations to database use `dotnet ef database update --project "path_to_migrations_project"`
2. To generate a migration use `dotnet ef migrations add "YourMigrationName" --project "path_to_migrations_project"`
3. To remove the last migration use `dotnet ef migrations remove --project "path_to_migrations_project"`


Examples:
- `cd LightBoard.Api`
- `dotnet ef database update --project "G:\Develop\RiderProjects\LightBoard\src\LightBoard.DataAccess.Migrations\LightBoard.DataAccess.Migrations.csproj"`
- `dotnet ef migrations add "AddAvatarUrlColumnToUserTable" --project "G:\Develop\RiderProjects\LightBoard\src\LightBoard.DataAccess.Migrations\LightBoard.DataAccess.Migrations.csproj"`
---

## Tools

- [DBeaver](https://dbeaver.io/)
- [Redis GUI](https://github.com/ekvedaras/redis-gui/releases/tag/v2.0.0)

---
