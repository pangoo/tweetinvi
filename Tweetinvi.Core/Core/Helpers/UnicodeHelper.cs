﻿using System;
using System.Globalization;
using System.Text;

namespace Tweetinvi.Core.Core.Helpers
{
    public static class UnicodeHelper
    {
        public static string UnicodeSubstring(string str, int startIndex)
        {
            if (str == null)
            {
                return null;
            }

            var sbuilder = new StringBuilder();

            Func<string, int, bool> shouldCountTwice = (string str2, int i2) =>
            {
                if (char.IsSurrogatePair(str2, i2))
                {
                    var grapheme = $"{str2[i2]}{str2[i2 + 1]}";

                    UnicodeCategory characterChategory = CharUnicodeInfo.GetUnicodeCategory(grapheme, 0);

                    return characterChategory == UnicodeCategory.ModifierSymbol;
                }

                return false;
            };

            var i = 0;
            for (; i < startIndex; ++i)
            {
                if (char.IsSurrogatePair(str, i))
                {
                    ++i;
                    ++startIndex;

                    var grapheme = $"{str[i]}{str[i + 1]}";

                    UnicodeCategory characterChategory = CharUnicodeInfo.GetUnicodeCategory(grapheme, 0);

                    if (characterChategory == UnicodeCategory.ModifierSymbol)
                    {
                        ++startIndex;
                    }
                }
            }

            for (int j = 0; i + j < str.Length; ++j)
            {
                if (char.IsSurrogatePair(str, i + j))
                {
                    var grapheme = $"{str[i + j]}{str[i + j + 1]}";

                    UnicodeCategory characterChategory = CharUnicodeInfo.GetUnicodeCategory(grapheme, 0);

                    ++j;

                    if (characterChategory == UnicodeCategory.ModifierSymbol)
                    {
                        continue;
                    }

                    sbuilder.Append(grapheme);
                }
                else
                {
                    sbuilder.Append(str[i + j]);
                }
            }

            return sbuilder.ToString();
        }

        public static bool AnyUnicode(string str)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if (char.IsSurrogatePair(str, i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the UTF32 length of a string
        /// </summary>
        public static int UTF32Length(this string str)
        {
            var length = 0;

            for (var i = 0; i < str.Length; i += char.IsSurrogatePair(str, i) ? 2 : 1)
            {
                ++length;
            }

            return length;
        }
    }
}