namespace PmtaUtilities.Configuration
{
    public class Property
    {
        public virtual string Key {get;set;}
        public virtual string Value { get; set; }

        public override string ToString()
        {
            return Key + " " + Value;
        }
    }
}
