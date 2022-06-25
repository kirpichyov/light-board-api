using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.Application.Models.CardComments
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
