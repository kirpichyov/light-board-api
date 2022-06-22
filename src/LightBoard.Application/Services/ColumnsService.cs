using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Models.Cards;
using LightBoard.Application.Models.Columns;
using LightBoard.DataAccess.Abstractions;
using LightBoard.Domain.Entities.Cards;

namespace LightBoard.Application.Services;

public class ColumnsService : IColumnsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInfoService _userInfo;
    private readonly IApplicationMapper _mapper;

    public ColumnsService(
        IUnitOfWork unitOfWork, 
        IUserInfoService userInfo, 
        IApplicationMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userInfo = userInfo;
        _mapper = mapper;
    }
    
    public async Task<ColumnResponse> UpdateColumn(Guid id, UpdateColumnNameRequest request)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);

        column.Name = request.Name;
        
        _unitOfWork.Columns.Update(column);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToColumnResponse(column);
    }

    public async Task DeleteColumn(Guid id)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);
        
        _unitOfWork.Columns.Delete(column);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ColumnResponse> GetColumn(Guid id)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);

        return _mapper.ToColumnResponse(column);
    }

    public async Task<ColumnResponse> UpdateOrder(Guid id, UpdateColumnOrderRequest request)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);
        
        var columnToSwap = column.Board.Columns.SingleOrDefault(boardColumn => boardColumn.Order == request.Order);

        if (columnToSwap is null)
        {
            column.Order = column.Board.Columns.Max(boardColumn => boardColumn.Order);

            foreach (var item in column.Board.Columns.Where(boardColumn => boardColumn.Id != column.Id))
            {
                item.Order--;
            }
        }
        else
        {
            var tempOrder = column.Order;
        
            column.Order = request.Order;

            columnToSwap.Order = tempOrder;
        }

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToColumnResponse(column);
    }

    public async Task<CardResponse> CreateCard(Guid id, CreateCardRequest request)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);
        
        var card = new Card(column.Id, request.Title, request.Description, column.Cards.Count + 1);
        card.Priority = _mapper.ToPriority(request.Priority);
        
        _unitOfWork.Cards.Add(card);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.ToCardResponse(card);
    }

    public async Task<IReadOnlyCollection<CardResponse>> GetColumnCards(Guid id)
    {
        var column = await _unitOfWork.Columns.GetForUser(id, _userInfo.UserId);

        return _mapper.MapCollection(column.Cards, _mapper.ToCardResponse);
    }
}