using GBWorldGen.Core.Models.Abstractions;

namespace GBWorldGen.Core.Algorithms.Transformers.Abstractions
{
    /// <summary>
    /// An abstract class to encapsulate the logic necessary to deserialize
    /// a <see cref="string"/> into a <see cref="BaseMap{T}"/>.
    /// </summary>
    /// <typeparam name="T">The value type that will hold the map's block coordinate values (x, y, z)</typeparam>
    public abstract class BaseDeserializer<T>
    {
        public abstract BaseMap<T> Deserialize(string serialized);
    }
}
