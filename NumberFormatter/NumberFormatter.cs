using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NumberFormatter
{
    public static class NumberFormatter
    {
        private static readonly Dictionary<string, int> Locales = new Dictionary<string, int>();

        private static readonly string[] LocalesLikeUs =
        {
            "ae", "au", "ca", "cn", "eg", "gb", "hk", "il", "in", "jp", "sk", "th",
            "tw", "us"
        };

        private static readonly string[] LocalesLikeDe = { "at", "br", "de", "dk", "es", "gr", "it", "nl", "pt", "tr", "vn" };
        private static readonly string[] LocalesLikeFr = { "bg", "cz", "fi", "fr", "no", "pl", "ru", "se" };
        private static readonly string[] LocalesLikeCh = { "ch" };

        private static readonly List<KeyValuePair<string, string>> LocaleFormatting = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>(".", ","),
            new KeyValuePair<string, string>(",", "."),
            new KeyValuePair<string, string>(",", " "),
            new KeyValuePair<string, string>(".", "'")
        };

        private static readonly string[][] AllLocales = { LocalesLikeUs, LocalesLikeDe, LocalesLikeFr, LocalesLikeCh };

        private const double Tolerance = 0.000000000000000000001;

        private static void Init()
        {
            // write the arrays into the hashtable
            for (var localeGroupIdx = 0; localeGroupIdx < AllLocales.Length; localeGroupIdx++)
            {
                var localeGroup = AllLocales[localeGroupIdx];
                foreach (var item in localeGroup)
                {
                    Locales.Add(item, localeGroupIdx);
                }
            }
        }

        private static FormatData FormatCodes(string locale, bool isFullLocale)
        {
            if (Locales.Count == 0)
                Init();

            // default values
            var dec = ".";
            var group = ",";
            var neg = "-";

            if (isFullLocale == false)
            {
                // Extract and convert to lower-case any language code from a real 'locale' formatted string, if not use as-is
                // (To prevent locale format like : "fr_FR", "en_US", "de_DE", "fr_FR", "en-US", "de-DE")
                if (locale.IndexOf('_') != -1)
                {
                    locale = locale.Split('_')[1].ToLower();
                }
                else if (locale.IndexOf('-') != -1)
                {
                    locale = locale.Split('-')[1].ToLower();
                }
            }

            // hashtable lookup to match locale with codes
            int codesIndex;

            if (Locales.TryGetValue(locale, out codesIndex))
            {

                if (codesIndex < LocaleFormatting.Count)
                {
                    var codes = LocaleFormatting[codesIndex];
                    dec = codes.Key;
                    group = codes.Value;
                }
            }

            return new FormatData(dec, group, neg);
        }

        /// <summary>
        /// First parses a string and reformats it with the given options.
        /// </summary>
        /// <param name="numberString"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string FormatNumber(string numberString, FormatOptions options)
        {
            var format = options.Format;
            var validFormat = "0#-,.";

            // strip all the invalid characters at the beginning and the end
            // of the format, and we'll stick them back on at the end
            // make a special case for the negative sign "-" though, so 
            // we can have formats like -$23.32
            var prefix = "";
            var negativeInFront = false;

            for (var i = 0; i < format.Length; i++)
            {
                var charAt = format[i];

                if (validFormat.IndexOf(charAt) == -1)
                {
                    prefix = prefix + charAt;
                }
                else if (i == 0 && charAt == '-')
                {
                    negativeInFront = true;
                }
                else
                    break;
            }

            var suffix = "";
            for (var i = format.Length - 1; i >= 0; i--)
            {
                var charAt = format[i];

                if (validFormat.IndexOf(charAt) == -1)
                    suffix = charAt + suffix;
                else
                    break;
            }

            format = format.Substring(prefix.Length);
            options.Format = format.Substring(0, format.Length - suffix.Length);

            // now we need to convert it into a number
            double number;
            if (!double.TryParse(numberString, out number))
            {
                number = double.NaN;
            }

            return FormatNumber(number, options, suffix, prefix, negativeInFront);
        }

        /// <summary>
        /// Formats a number into a string, using the given formatting options
        /// </summary>
        /// <param name="number"></param>
        /// <param name="options"></param>
        /// <param name="suffix"></param>
        /// <param name="prefix"></param>
        /// <param name="negativeInFront"></param>
        /// <returns></returns>
        public static string FormatNumber(double number, FormatOptions options, string suffix, string prefix, bool negativeInFront)
        {
            var formatData = FormatCodes(options.Locale.ToLower(), options.IsFullLocale);

            var format = options.Format;

            var dec = formatData.Dec;
            var group = formatData.Group;
            var neg = formatData.Neg;

            // check overrides
            if (options.OverrideGroupSep != null)
            {
                group = options.OverrideGroupSep;
            }

            if (options.OverrideDecSep != null)
            {
                dec = options.OverrideDecSep;
            }

            if (options.OverrideNegSign != null)
            {
                neg = options.OverrideNegSign;
            }

            // Check NAN handling
            var forcedToZero = false;
            if (double.IsNaN(number))
            {
                if (options.NanForceZero)
                {
                    number = 0;
                    forcedToZero = true;
                }
                else
                {
                    return "";
                }
            }

            // special case for percentages
            if (options.IsPercentage || (options.AutoDetectPercentage && suffix.Length > 0 && suffix[suffix.Length - 1] == '%'))
            {
                number = number * 100;
            }

            var returnString = "";
            if (format.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                var decimalPortion = dec;
                var decimalFormat = format.Substring(format.LastIndexOf(".", StringComparison.Ordinal) + 1);

                // round or truncate number as needed
                if (options.Round)
                {
                    number = Math.Round(number, decimalFormat.Length);
                }
                else
                {
                    var numStr = number.ToString();
                    
                    var idx = numStr.LastIndexOf('.');
                    if (idx > 0)
                    {
                        numStr = numStr.Substring(0, idx + decimalFormat.Length + 1);
                    }

                    double.TryParse(numStr, out number);
                }

                var decimalValue = number - Math.Truncate(number);

                var decimalString = Math.Round(decimalValue, decimalFormat.Length).ToString();
                decimalString = decimalString.Substring(decimalString.LastIndexOf('.') + 1);

                for (var i = 0; i < decimalFormat.Length; i++)
                {
                    var charAtFmt = decimalFormat[i];
                    var strAt = i < decimalString.Length 
                        ? decimalString[i].ToString()
                        : "";

                    if (charAtFmt == '#')
                    {
                        if (strAt != "0")
                        {
                            decimalPortion += strAt;
                            continue;
                        }

                        if (strAt == "0")
                        {
                            var notParsed = decimalString.Substring(i);

                            if (Regex.IsMatch(notParsed, "[1-9]"))
                            {
                                decimalPortion += strAt;
                                continue;
                            }

                            break;
                        }
                    }

                    if (charAtFmt == '0')
                        decimalPortion += strAt == "" ? "0" : strAt;
                }

                returnString += decimalPortion;
            }
            else
            {
                number = Math.Round(number);
            }

            var ones = number < 0 
                ? Math.Ceiling(number) 
                : Math.Floor(number);

            var onesFormat = format.IndexOf(".", StringComparison.Ordinal) == -1 
                ? format 
                : format.Substring(0, format.IndexOf(".", StringComparison.Ordinal));

            var onePortion = "";
            if (!(Math.Abs(ones) < Tolerance && onesFormat.Substring(onesFormat.Length - 1) == "#") || forcedToZero)
            {
                // find how many digits are in the group
                var oneText = Math.Abs(ones).ToString();

                var groupLength = 9999;
                if (onesFormat.LastIndexOf(",", StringComparison.Ordinal) != -1)
                    groupLength = onesFormat.Length - onesFormat.LastIndexOf(",", StringComparison.Ordinal) - 1;

                var groupCount = 0;
                for (var i = oneText.Length - 1; i > -1; i--)
                {
                    onePortion = oneText[i] + onePortion;
                    groupCount++;

                    if (groupCount == groupLength && i != 0)
                    {
                        onePortion = group + onePortion;
                        groupCount = 0;
                    }
                }

                // account for any pre-data padding
                if (onesFormat.Length > onePortion.Length)
                {
                    var padStart = onesFormat.IndexOf('0');
                    if (padStart != -1)
                    {
                        var padLen = onesFormat.Length - padStart;

                        // pad to left with 0's or group char
                        var pos = onesFormat.Length - onePortion.Length - 1;
                        while (onePortion.Length < padLen)
                        {
                            var padChar = onesFormat[pos].ToString();
                            // replace with real group char if needed
                            if (padChar == ",")
                                padChar = group;

                            onePortion = padChar + onePortion;
                            pos--;
                        }
                    }
                }
            }

            if (onePortion == null && onesFormat.IndexOf('0', onesFormat.Length - 1) != -1)
                onePortion = "0";

            returnString = onePortion + returnString;

            // handle special case where negative is in front of the invalid characters
            if (number < 0 && negativeInFront && prefix.Length > 0)
                prefix = neg + prefix;
            else if (number < 0)
                returnString = neg + returnString;

            if (!options.DecimalSeparatorAlwaysShown)
            {
                if (returnString.LastIndexOf(dec, StringComparison.Ordinal) == returnString.Length - 1)
                {
                    returnString = returnString.Substring(0, returnString.Length - 1);
                }
            }

            returnString = prefix + returnString + suffix;
            return returnString;
        }
    }
}
