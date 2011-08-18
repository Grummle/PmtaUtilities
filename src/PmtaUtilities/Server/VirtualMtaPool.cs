using System.Collections.Generic;

namespace PmtaUtilities.Server
{
    public class VirtualMtaPool
    {
        public string Name {get; set;}
        public IList<VirtualMta> VirtualMtas {get; set;}

        public VirtualMtaPool()
        {
            VirtualMtas = new List<VirtualMta>();
        }
    }
}