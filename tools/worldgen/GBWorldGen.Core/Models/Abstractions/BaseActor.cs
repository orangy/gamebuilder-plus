namespace GBWorldGen.Core.Models.Abstractions
{
    public abstract class BaseActor<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public BaseActor() { }
        public BaseActor(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
