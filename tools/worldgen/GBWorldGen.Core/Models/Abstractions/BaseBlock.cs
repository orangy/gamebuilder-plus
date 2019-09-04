namespace GBWorldGen.Core.Models.Abstractions
{
    /// <summary>
    /// A base class implementing a world-terrain block.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the block's coordinates values (x, y, z)</typeparam>
    public abstract class BaseBlock<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public BaseBlock(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Overrides
        public override string ToString()
        {
            return $"(X:{X}, Y:{Y}, Z:{Z})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is BaseBlock<T>)
            {
                BaseBlock<T> compareTo = (BaseBlock<T>)obj;
                return X.Equals(compareTo.X) &&
                    Y.Equals(compareTo.Y) &&
                    Z.Equals(compareTo.Z);
            }

            return false;
        }

        public override int GetHashCode()
        {
            // This forces the compiler to call Equals(object obj)
            return 0;
        }
        #endregion        
    }
}
