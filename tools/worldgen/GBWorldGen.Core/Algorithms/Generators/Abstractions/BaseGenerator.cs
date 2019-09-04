using GBWorldGen.Core.Models.Abstractions;
using System;

namespace GBWorldGen.Core.Algorithms.Generators.Abstractions
{
    /// <summary>
    /// A base generator class that is used to create a world.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the block's coordinates values (x, y, z)</typeparam>
    public abstract class BaseGenerator<T>
    {
        protected T Width { get; set; }
        protected T Length { get; set; }
        protected T Height { get; set; }
        protected BaseMap<T> GeneratedMap { get; set; }
        protected GetNoise2DDelegate Noise2D { get; set; }
        protected GetNoise3DDelegate Noise3D { get; set; }

        public BaseGenerator(T width, T length)
        {
            Width = width;
            Length = length;
        }

        public BaseGenerator(T width, T length, T height)
        {
            Width = width;
            Length = length;
            Height = height;
        }

        public abstract BaseMap<T> GenerateMap();

        public delegate float GetNoise2DDelegate(float x, float z);
        public delegate float GetNoise3DDelegate(float x, float z, float y);
    }
}
