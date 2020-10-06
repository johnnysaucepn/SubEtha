namespace Howatworks.SubEtha.Journal.Cooked
{
    public class Localised<T>
    {
        public Localised(T value, string text)
        {
            Value = value;
            Text = text;
        }

        public T Value { get; set; }
        public string Text { get; set; }
    }
}
