using GBWorldGen.Models;

namespace GBWorldGen.Algorithms
{
    public interface IAlgorithm
    {
        Block[] Generate();
    }
}
