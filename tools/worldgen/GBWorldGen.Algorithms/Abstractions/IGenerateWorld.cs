using GBWorldGen.Core.Models;

namespace GBWorldGen.Core.Algorithms
{
    /// <summary>
    ///     An interface for generating worlds.
    /// </summary>
    public interface IGenerateWorld
    {
        Block[] Generate();
    }
}
