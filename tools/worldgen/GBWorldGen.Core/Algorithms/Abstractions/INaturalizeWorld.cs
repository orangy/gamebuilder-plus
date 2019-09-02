using GBWorldGen.Core.Models;

namespace GBWorldGen.Core.Algorithms.Abstractions
{
    /// <summary>
    ///     An interface for naturalizing (touching up) a world.
    /// </summary>
    public interface INaturalizeWorld
    {
        Map Naturalize(Map map);
    }
}
