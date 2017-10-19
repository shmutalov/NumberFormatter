namespace NumberFormatter
{
    public class ParseOptions
    {
        public string Locale { get; set; } = "us";

        public bool DecimalSeperatorAlwaysShown { get; set; } = false;

        /// <summary>
        /// Treats the input as a percentage (i.e. input divided by 100)
        /// </summary>
        public bool IsPercentage { get; set; } = false;

        /// <summary>
        ///  Will search if the input string ends with '%', if it does then the above option is implicitly set
        /// </summary>
        public bool AutoDetectPercentage { get; set; } = true;

        public bool IsFullLocale { get; set; } = false;

        /// <summary>
        /// Will abort the parse if it hits any unknown char
        /// </summary>
        public bool Strict { get; set; } = false;

        /// <summary>
        /// Override for group separator
        /// </summary>
        public string OverrideGroupSep { get; set; } = null;

        /// <summary>
        /// Override for decimal point separator
        /// </summary>
        public string OverrideDecSep { get; set; } = null;

        /// <summary>
        /// Override for negative sign
        /// </summary>
        public string OverrideNegSign { get; set; } = null;

        /// <summary>
        /// Will truncate the input string as soon as it hits an unknown char
        /// </summary>
        public bool AllowPostfix { get; set; } = false;
    }
}
