﻿using LightBoard.DataAccess.Abstractions.Arguments;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IBoardsRepository : IRelationalRepositoryBase<Board, Guid>
{
    Task<Board> GetForUser(Guid boardId, Guid userId);
    Task<IReadOnlyCollection<Board>> GetAllByUserId(Guid userId);
    Task<bool> HasAccessToBoard(Guid boardId, Guid userId);
    Task<IReadOnlyCollection<Card>> SearchForUser(Guid boardId, SearchCardsArgs searchCardsArgs); 
}