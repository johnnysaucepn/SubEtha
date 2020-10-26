using System;

namespace Howatworks.Matrix.Domain
{
    public class LocalisedString : IEquatable<LocalisedString>
    {
        public string Symbol { get; set; }
        public string Text { get; set; }

        public LocalisedString(string symbol, string text)
        {
            Symbol = symbol;
            Text = text;
        }

        public bool Equals(LocalisedString other)
        {
            if (other == null) return false;

            if (!string.Equals(Symbol, other.Symbol)) return false;
            if (!string.Equals(Text, other.Text)) return false;

            return true;
        }
    }
}
