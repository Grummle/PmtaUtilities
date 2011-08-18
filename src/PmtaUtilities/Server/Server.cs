using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using PmtaUtilities.XML;
using PmtaUtilities.Configuration;

namespace PmtaUtilities.Server
{
    public class Server
    {
        public Uri baseUrl { get; set; }
        public PmtaUtilities.Configuration.Configuration Configuration { get; set; }
        public Status Status { get; set; }
        public IList<Job> Jobs { get; set; }

        public IList<VirtualMta> VirtualMtas
        {
            get
            {
                return (from mta in Configuration.Entries
                           where mta.Name.ToLower() == "virtual-mta"
                           select new VirtualMta
                                      {
                                          Name = mta.Argument,
                                          Sources = (from source in mta.Directives
                                                     where source.Command.ToLower() == "smtp-source-host"
                                                     select new VirtualMta.SmtpSourceHost
                                                                {
                                                                    Name = source.Argument2,
                                                                    IpAddress = IPAddress.Parse(source.Argument1)
                                                                }).ToList()
                                      }).ToList();
                    
            }
        }
        public IList<VirtualMtaPool> VirtualMtaPools
        {
            get
            {
                return (from mtapool in Configuration.Entries
                        where mtapool.Name.ToLower() == "virtual-mta-pool"
                        select new VirtualMtaPool
                                   {
                                       Name = mtapool.Argument,
                                       VirtualMtas = (from mta in mtapool.Properties
                                                     where mta.Key.ToLower() == "virtual-mta"
                                                     select VirtualMtas.First(x=>x.Name.ToLower() == mta.Value.ToLower()))
                                                     .ToList()
                                   }).ToList();
            }
        }

        public IList<PatternList> PatternLists { get; set; }

    }

    public class PatternList
    {
        
    }
}
