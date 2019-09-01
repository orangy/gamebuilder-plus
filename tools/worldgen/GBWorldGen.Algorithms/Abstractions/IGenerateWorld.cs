using GBWorldGen.Core.Models;

namespace GBWorldGen.Core.Algorithms.Abstractions
{
    /// <summary>
    ///     An interface for generating worlds.
    /// </summary>
    public interface IGenerateWorld
    {
        Map Generate(params float[] values);
    }
}
