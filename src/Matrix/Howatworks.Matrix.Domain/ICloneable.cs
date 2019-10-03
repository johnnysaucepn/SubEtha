namespace Howatworks.Matrix.Domain
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}