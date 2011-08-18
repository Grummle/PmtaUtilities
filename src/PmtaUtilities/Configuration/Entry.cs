using System;
using System.Collections.Generic;
using PmtaUtilities.Extensions;

namespace PmtaUtilities.Configuration
{
    public class Entry
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public IList<Directive> Directives { get; set; }
        public IList<Entry> Entries { get; set; }
        public IList<Property> Properties { get; set; }


        public override string ToString()
        {
            string entry = "";
            entry += "<" + Name + " " + Argument + ">" + Environment.NewLine;
            Properties.ForEach(x => entry +=  x.ToString() + Environment.NewLine);
            Directives.ForEach(x => entry +=  x.ToString() + Environment.NewLine);
            Entries.ForEach(x => entry += x.ToString());
            entry += "</" + Name + ">" + Environment.NewLine;

            return entry;
        }
    }
}
