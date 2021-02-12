using System.Collections.Generic;

namespace Carpool.Contracts.Response
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }

    public class ErrorModel
    {
        public string FiledName { get; set; }
        public string Message { get; set; }
    }
}