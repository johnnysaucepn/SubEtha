namespace SubEtha.Domain
{
    public class LocalisedString
    {
        public string Symbol { get; set; }
        public string Text { get; set; }

        public LocalisedString(string symbol, string text)
        {
            Symbol = symbol;
            Text = text;
        }
    }
}
