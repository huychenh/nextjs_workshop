using System;
using System.Runtime.Serialization;

namespace N8T.Infrastructure.Exceptions
{
    public class CareStoreException : Exception
    {
        public CareStoreException()
        {
        }

        public CareStoreException(string message) : base(message)
        {
        }

        public CareStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CareStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
