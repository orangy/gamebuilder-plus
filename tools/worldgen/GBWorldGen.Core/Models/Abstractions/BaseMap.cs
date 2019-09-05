using System.Collections.Generic;

namespace GBWorldGen.Core.Models.Abstractions
{
    /// <summary>
    /// A base class representing a 3D block world.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the map's block coordinate values (x, y, z)</typeparam>
    public abstract class BaseMap<T>
    {
        private T width;
        private T length;
        private T height;

        /// <summary>
        /// Returns the width in <see cref="BaseBlock{T}"/> of the map.
        /// </summary>
        public T Width { get { return GetWidth(); } set { width = value; } }

        /// <summary>
        /// Returns the length in <see cref="BaseBlock{T}"/> of the map.
        /// </summary>
        public T Length { get { return GetLength(); } set { length = value; } }

        /// <summary>
        /// Returns the length in <see cref="BaseBlock{T}"/> of the map.
        /// </summary>
        public T Height { get { return GetHeight(); } set { height = value; } }

        /// <summary>
        /// Returns the origin position in <see cref="T"/> where width begins.
        /// </summary>
        public T OriginWidth { get; set; }

        /// <summary>
        /// Returns the origin position in <see cref="T"/> where length begins.
        /// </summary>
        public T OriginLength { get; set; }

        /// <summary>
        /// Returns the origin position in <see cref="T"/> where height begins.
        /// </summary>
        public T OriginHeight { get; set; }

        /// <summary>
        /// The minimum allowed value in <see cref="T"/> of width for the map.
        /// </summary>
        public T MinWidth { get; set; }

        /// <summary>
        /// The minimum allowed value in <see cref="T"/> of length for the map.
        /// </summary>
        public T MinLength { get; set; }

        /// <summary>
        /// The minimum allowed value in <see cref="T"/> of height for the map.
        /// </summary>
        public T MinHeight { get; set; }

        /// <summary>
        /// The maximum allowed value in <see cref="T"/> of width for the map.
        /// </summary>
        public T MaxWidth { get; set; }

        /// <summary>
        /// The maximum allowed value in <see cref="T"/> of length for the map.
        /// </summary>
        public T MaxLength { get; set; }

        /// <summary>
        /// The maximum allowed value in <see cref="T"/> of height for the map.
        /// </summary>
        public T MaxHeight { get; set; }
        public List<BaseBlock<T>> MapData { get; set; }

        public BaseMap(T width, T length, T height, T originWidth, T originLength, T originHeight, T minWidth, T minLength, T minHeight, T maxWidth, T maxLength, T maxHeight)
        {
            Width = width;
            Length = length;
            Height = height;
            OriginWidth = originWidth;
            OriginLength = originLength;
            OriginHeight = originHeight;
            MinWidth = minWidth;
            MinLength = minLength;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxLength = maxLength;
            MaxHeight = maxHeight;

            MapData = new List<BaseBlock<T>>();
        }

        public virtual void Add(BaseBlock<T> block)
        {
            MapData.Add(block);
        }

        #region Private methods
        public virtual T GetWidth() { return width; }

        public virtual T GetLength() { return length; }

        public virtual T GetHeight() { return height; }
        #endregion
    }
}
