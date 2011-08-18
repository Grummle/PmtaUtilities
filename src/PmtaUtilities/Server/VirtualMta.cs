using System.Collections.Generic;
using System.Net;

namespace PmtaUtilities.Server
{
    public class VirtualMta
    {
        public string Name { get; set; }
        public IList<SmtpSourceHost> Sources { get; set; }

        public VirtualMta()
        {
            Sources = new List<SmtpSourceHost>();
        }

        public class SmtpSourceHost
        {
            public string Name { get; set; }
            public IPAddress IpAddress { get; set; }
        }
    }
}