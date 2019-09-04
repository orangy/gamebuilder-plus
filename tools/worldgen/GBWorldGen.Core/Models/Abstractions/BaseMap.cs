using System.Collections.Generic;

namespace GBWorldGen.Core.Models.Abstractions
{
    /// <summary>
    /// A base class representing a 3D block world.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the map's block coordinate values (x, y, z)</typeparam>
    public abstract class BaseMap<T>
    {
        public T Width { get { return GetWidth(); } set { Width = value; } }
        public T Length { get { return GetLength(); } set { Length = value; } }
        public T Height { get { return GetHeight(); } set { Height = value; } }
        public ICollection<BaseBlock<T>> MapData { get; set; }

        public BaseMap(T width, T length, T height)
        {
            Width = width;
            Length = length;
            Height = height;
        }

        public virtual void Add(BaseBlock<T> block)
        {
            MapData.Add(block);
        }

        #region Private methods
        public virtual T GetWidth() { return Width; }

        public virtual T GetLength() { return Length; }

        public virtual T GetHeight() { return Height; }
        #endregion
    }
}
