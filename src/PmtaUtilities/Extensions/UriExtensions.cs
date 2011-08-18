using System;
using System.Linq;

namespace PmtaUtilities.Extensions
{
    public static class UriExtensions
    {
        public static Uri Combine(this Uri uri1, string uri2)
        {
            string uri;
            uri = uri1.ToString().TrimEnd('/');
            uri = uri.TrimEnd('\\');
            uri2 = uri2.TrimStart('/');
            uri2 = uri2.TrimStart('\\');
            return new Uri(string.Format("{0}/{1}", uri, uri2));
        }

        public static Uri Combine(this Uri uri1, string[] additions)
        {


            foreach (var addition in additions)
            {
                uri1 = uri1.Combine(addition);
            }
            return uri1;
        }

        public static Uri Combine(this Uri uri1, string ad1, string ad2)
        {
            return uri1.Combine(new string[] { ad1, ad2 });
        }

        public static string FileName(this Uri uri)
        {
            if (uri.ToString().Last() == '/')
                return null;

            return uri.Segments[uri.Segments.Length - 1];
        }
    }
}
