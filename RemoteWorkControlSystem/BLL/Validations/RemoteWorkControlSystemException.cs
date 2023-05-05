using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validations
{
    [Serializable]
    public class RemoteWorkControlSystemException: Exception
    {

        public RemoteWorkControlSystemException()
        { }

        public RemoteWorkControlSystemException(string message)
            : base(message)
        { }

        public RemoteWorkControlSystemException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected RemoteWorkControlSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
