using System;
using System.Collections.Generic;
using Sprache;

namespace PmtaUtilities.Configuration.Parser
{
    public static class PmtaParser
    {
        public static Parser<char> NewLine = Parse.String(Environment.NewLine).Return('\n');
        public static Parser<char> WhiteSpace = Parse.Char(' ').Or(Parse.Char('\t'));
        public static Parser<string> EmptyLine()
        {
            return from comment in Comment().Many().Tokenize()
                   from ws in WhiteSpace.Many()
                   from end in NewLine.Or(End<char>())
                   select "";
        }


        public static Parser<char> EscapedDelimiter()
        {
            return from escape in Parse.Char('\\')
                   from f in Parse.Char('/')
                   select f;
        }

        public static Parser<char> RegularExpressionCharacters = Parse.AnyChar
                                                                    .Except(NewLine)
                                                                    .Except(Parse.Char('/'));

        public static Parser<T> End<T>()
        {
            return Parse.End(Parse.Return(default(T)));
        }

        public static Parser<string> RegularExpression()
        {
            return from startDelimit in Parse.Char('/')
                   from expression in (EscapedDelimiter().Or(RegularExpressionCharacters)).AtLeastOnce().Text()
                   from endDelimit in Parse.Char('/')
                   select "/" + expression.Replace("/", @"\/") + "/";
        }

        public static Parser<string> PatternHeader()
        {
            return from header in Parse.String("header").Tokenize()
                   from type in Command
                   select type;
        }

        public static Parser<string> Comment()
        {
            return from delimiter in Parse.Char('#').Tokenize()
                   from comment in Parse.AnyChar.Except(NewLine).Many().Text()
                   select comment;

        }

        public static Parser<char> CommandCharacters = Parse.AnyChar
            .Except(NewLine)
            .Except(WhiteSpace)
            .Except(Parse.Char('<'))
            .Except(Parse.Char('>'))
            .Except(Parse.Char('#'))
            .Except(Parse.Char('='));

        public static Parser<string> Command = from thecommand in CommandCharacters.AtLeastOnce().Text().Tokenize()
                                               select thecommand;

        public static Parser<T> Tokenize<T>(this Parser<T> parser)
        {
            if (parser == null) throw new ArgumentNullException("parser");

            return from ws in WhiteSpace.Many()
                   from item in parser
                   from ws2 in WhiteSpace.Many()
                   select item;
        }

        public static Parser<Object> Property()
        {
            return from keyvalue in CommandCharacters.AtLeastOnce().Text().Tokenize()
                   from propertyValue in CommandCharacters.AtLeastOnce().Text().Tokenize()
                   from comment in Comment().Many()
                   from endline in NewLine.Or(Parse.End(Parse.Return(' ')))
                   select new Property { Key = keyvalue, Value = propertyValue };
        }

        public static Parser<Object> Directive(Parser<char> endline)
        {
            return from commandName in Command
                   from arg1 in Command
                   from arg2 in Command
                   from comment in Comment().Many()
                   from end in endline
                   select new Directive { Command = commandName, Argument1 = arg1, Argument2 = arg2 };
        }

        public static Parser<object> PatternDirective(Parser<char> endline)
        {
            return from commandName in PatternHeader().Or(Command)
                   from arg1 in RegularExpression().Tokenize()
                   from arg2 in RegularExpression().Or(KeyValuePair())
                   from comment in Comment().Many()
                   from end in endline
                   select new Directive { Command = commandName, Argument1 = arg1, Argument2 = arg2 };        
        }

        public static Parser<string> KeyValuePair()
        {
            return from key in Command
                   from divider in Parse.Char('=')
                   from value in Command
                   select value;
        }

        private static Parser<T> Tag<T>(Parser<T> content,Parser<T> endline)
        {
            return from lt in Parse.Char('<').Tokenize()
                   from t in content
                   from gt in Parse.Char('>').Tokenize()
                   from comment in Comment().Many()
                   from end in endline
                   select t;
        }

        public static Parser<Tuple<string, string>> BeginEntry()
        {
            return Tag(from entryName in Command
                       from argument in Command
                       select new Tuple<string, string>(entryName, argument),
                       NewLine.Return(new Tuple<string,string>("","")));
        }

        public static Parser<string> EndTag(string name)
        {
            return Tag(from slash in Parse.Char('/').Tokenize()
                       from id in Command
                       where id == name
                       select id,
                       NewLine.Return("").Or(Parse.End(Parse.Return("")))).Named("closing tag for " + name);
        }

        public static Parser<object> Entry()
        {
             return from entryTag in BeginEntry()
                    from stuff in (Property().Or(PatternDirective(NewLine)).Or(Directive(NewLine)).Or(Entry()).Or(EmptyLine())).Many()
                    from endTag in EndTag(entryTag.Item1)
                    select new Entry
                               {
                                   Name = entryTag.Item1,
                                   Argument = entryTag.Item2,
                                   Directives = Sort(stuff).Item1,
                                   Entries = Sort(stuff).Item2,
                                   Properties = Sort(stuff).Item3
                               };    
        }

        public static Parser<Configuration> Configuration()
        {
            return from stuff in (EmptyLine()
                       .Or(PatternDirective(NewLine.Or(End<char>())))
                       .Or(Directive(NewLine.Or(End<char>())))
                       .Or(Property())
                       .Or(Entry())).Many()
                   from eof in End<char>()
                   select new Configuration
                              {
                                  Directives = Sort(stuff).Item1,
                                  Entries = Sort(stuff).Item2,
                                  Properties = Sort(stuff).Item3
                              };
                                    

        }

        public static Tuple<IList<Directive>, IList<Entry>, IList<Property>> Sort(IEnumerable<Object> objects)
        {
            IList<Directive> directives = new List<Directive>();
            IList<Entry> entries = new List<Entry>();
            IList<Property> properties = new List<Property>();

            foreach (var x in objects)
            {
                if (x.GetType() == typeof(Directive))
                {
                    directives.Add((Directive)x);
                }

                if (x.GetType() == typeof(Property))
                {
                    properties.Add((Property)x);
                }

                if (x.GetType() == typeof(Entry))
                {
                    entries.Add((Entry)x);
                }


            }
            return Tuple.Create(directives, entries, properties);
        }


    }


}
