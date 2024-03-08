// dylan@hathora.dev

using System.Text.RegularExpressions;

namespace Hathora.Core.Scripts.Runtime.Common.Extensions
{
    /// <summary>Extension methods for string.</summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string to PascalCase. Eg: "fooBar" -> "Foo Bar".
        /// * For 2+ CAPS in a row, like "WashingtonDC", it will be "Washington DC".
        /// --------
        /// * Inserts a space before each uppercase letter that is either:
        /// * Followed by a lowercase letter, or
        /// * Preceded by a lowercase letter and not followed by an uppercase letter.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            
            // ##############################################
            // "WashingtonDC" becomes "Washington DC"
            // "WashingtonDc" becomes "Washington Dc"
            // "WashingtonDCFoo" becomes "Washington DC Foo"
            // ##############################################
            return Regex.Replace(
                str,
                @"(?<=\p{Ll})(?=\p{Lu})|(?<=\p{Lu})(?=\p{Lu}\p{Ll})",
                " ");
        }
    }
}
