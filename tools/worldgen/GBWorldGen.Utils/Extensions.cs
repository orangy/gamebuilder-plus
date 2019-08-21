using System.IO;
using GBWorldGen.Models;

namespace GBWorldGen.Utils
{
    public static class Extensions
    {
        public static void Write(this BinaryWriter binaryWriter, Block block)
        {
            binaryWriter.Write(block.x);
            binaryWriter.Write(block.y);
            binaryWriter.Write(block.z);
            binaryWriter.Write((byte)block.shape);
            binaryWriter.Write((byte)block.direction);
            binaryWriter.Write((ushort)block.style);
        }
    }
}
