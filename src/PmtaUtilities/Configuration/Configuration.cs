using System;
using System.Collections.Generic;
using PmtaUtilities.Extensions;

namespace PmtaUtilities.Configuration
{
    public class Configuration
    {
        public string Text { get; private set; }
        public IList<Directive> Directives { get; set; }
        public IList<Entry> Entries { get; set; }
        public IList<Property> Properties { get; set; }

        public override string ToString()
        {
            string config = "";
            Properties.ForEach(x => config +=x.ToString() + Environment.NewLine);
            Directives.ForEach(x => config +=x.ToString() + Environment.NewLine);
            Entries.ForEach(x => config += x.ToString());

            return config;
        }
    }
}
