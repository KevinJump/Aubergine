using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aubergine.UserContent
{
    /// <summary>
    ///  simple html cleaning helpers, (lifted from our.umbraco.org source) 
    /// </summary>
    public static class StringHelperExtensions
    {
        private static Regex _tags = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static Regex _whitelist = new Regex(@"
            ^</?(a|b(lockquote)?|code|em|h(1|2|3)|i|li|ol|p(re)?|s(ub|up|trong|trike)?|table|tr|th|td|ul)>$
            |^<(b|h)r\s?/?>$
            |^<a[^>]+>$
            |^<img[^>]+/?>$",
            RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace |
            RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        public static string SanitizeHtml(this string html)
        {
            var tagname = "";
            Match tag;
            var tags = _tags.Matches(html);

            // iterate through all HTML tags in the input
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLower();

                if (!_whitelist.IsMatch(tagname))
                {
                    // not on our whitelist? I SAY GOOD DAY TO YOU, SIR. GOOD DAY!
                    html = html.Remove(tag.Index, tag.Length);
                }
                else if (tagname.StartsWith("<a"))
                {
                    // detailed <a> tag checking
                    if (!IsMatch(tagname,
                        @"<a\s
                  href=""(\#\d+|(https?|ftp)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+)""
                  (\stitle=""[^""]+"")?\s?>"))
                    {
                        html = html.Remove(tag.Index, tag.Length);
                    }
                }
                else if (tagname.StartsWith("<img"))
                {
                    // detailed <img> tag checking
                    if (!IsMatch(tagname,
                        @"<img\s
              src=""https?://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+""
              (\swidth=""\d{1,3}"")?
              (\sheight=""\d{1,3}"")?
              (\salt=""[^""]*"")?
              (\stitle=""[^""]*"")?
              \s?/?>"))
                    {
                        html = html.Remove(tag.Index, tag.Length);
                    }
                }

            }

            return html;
        }

        /// <summary>
        /// Utility function to match a regex pattern: case, whitespace, and line insensitive
        /// </summary>
        private static bool IsMatch(string s, string pattern)
        {
            return Regex.IsMatch(s, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
        }
    }
}
