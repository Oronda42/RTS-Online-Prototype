using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    /// <summary>
    /// Class wich identify a tag. Values can be between 0..65535
    /// </summary>
    public static class ServerCommunicationTags
    {

        public const ushort SERVER_REGISTRATION_REQUEST = 65000;

    }
}
