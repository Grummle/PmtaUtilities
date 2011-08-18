using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PmtaUtilities.XML.Filler
{
    public class JobFiller
    {
        public IList<Job> Extract(XDocument doc)
        {
            return doc.Descendants("job")
                .Select(x => new Job
                                 {
                                     KB = double.Parse((string) x.Element("kb")),
                                     Name = (string) x.Element("id"),
                                     Recipients = int.Parse((string) x.Element("rcp"))
                                 }).ToList();
        }
    }
}
