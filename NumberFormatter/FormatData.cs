namespace NumberFormatter
{
    internal class FormatData
    {
        public string Dec { get; set; }
        public string Group { get; set; }
        public string Neg { get; set; }

        public FormatData(string dec, string group, string neg)
        {
            Dec = dec;
            Group = group;
            Neg = neg;
        }
    }
}
