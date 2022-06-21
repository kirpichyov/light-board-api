using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBoard.Shared.Exceptions
{
    public class ExternalApiException : ExceptionBase
    {
        public string ApiResponse { get; }

        public ExternalApiException(string message, string apiResponse)
            : base(message, ExceptionIdentifiers.Generic)
        {
            ApiResponse = apiResponse;
        }
    }
}
