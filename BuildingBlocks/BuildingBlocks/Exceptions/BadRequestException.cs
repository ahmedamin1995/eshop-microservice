using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException()
            : base("The request was invalid or cannot be served.")
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
            Detail = message;
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
            Detail = innerException.InnerException?.Message ?? innerException.Message;
        }

        public string? Detail { get; private set; }
    }
}
