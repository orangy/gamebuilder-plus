using GBWorldGen.Core.Models.Abstractions;

namespace GBWorldGen.Core.Algorithms.Transformers.Abstractions
{
    /// <summary>
    /// An abstract class to encapsulate the logic necessary to serialize
    /// a <see cref="BaseMap{T}"/> into a <see cref="string"/>.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the map's block coordinate values (x, y, z)</typeparam>
    public abstract class BaseSerializer<T>
    {
        public abstract string Serialize(BaseMap<T> map);
    }
}
