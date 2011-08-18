namespace PmtaUtilities.Configuration
{
    public class Directive
    {
        public string Command { get; set; }
        public string Argument1 { get; set; }
        public string Argument2 { get; set; }

        public override string ToString()
        {
            return Command + " " + Argument1 + " "+Argument2;
        }
    }
}
