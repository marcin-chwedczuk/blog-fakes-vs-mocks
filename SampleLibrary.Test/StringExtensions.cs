using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SampleLibrary.Test {
    public static class StringExtensions {
        public static bool ContainsAllCaseInsensitive(this string str, params string[] substrings) {
            return substrings.All(substring => 
                str.IndexOf(substring, StringComparison.InvariantCultureIgnoreCase) != -1);
        }
    }
}