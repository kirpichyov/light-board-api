using LightBoard.Application.Abstractions.Services;
using LightBoard.DataAccess.Abstractions;

namespace LightBoard.Application.Services
{
    public class CardAttachmentService : ICardAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfo;

        public CardAttachmentService(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfo = userInfoService;
        }

        public async Task DeleteCardAttachment(Guid attachmentId)
        {
            var attachment = await _unitOfWork.Attachments.GetForUser(attachmentId, _userInfo.UserId);

            _unitOfWork.Attachments.Delete(attachment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
