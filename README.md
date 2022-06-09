# Light Board API
LightBoard API is a backend for Trello analogue application developed with ASP.NET Core 6.
[Project documentation.](https://github.com/kirpichyov/light-board-api/wiki)
[Figma](https://www.figma.com/file/pBkCAmVgUR5Gh34071pUHI/LightBoard?node-id=0%3A1)

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

## Tools

- [DBeaver](https://dbeaver.io/)
- [Redis GUI](https://github.com/ekvedaras/redis-gui/releases/tag/v2.0.0)

---
