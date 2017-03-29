namespace numberformatter_net
{
    public class FormatOptions
    {
        public string Format { get; set; } = "#,###.00";

        public string Locale { get; set; } = "us";

        public bool DecimalSeparatorAlwaysShown { get; set; } = false;

        public bool NanForceZero { get; set; } = true;

        public bool Round { get; set; } = true;

        public bool IsFullLocale { get; set; } = false;

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
        /// Treats the input as a percentage (i.e. input divided by 100)
        /// </summary>
        public bool IsPercentage { get; set; } = false;

        /// <summary>
        ///  Will search if the input string ends with '%', if it does then the above option is implicitly set
        /// </summary>
        public bool AutoDetectPercentage { get; set; } = true;
    }
}
