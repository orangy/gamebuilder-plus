using GBWorldGen.Models;
using System;
using System.IO;
using System.IO.Compression;

namespace GBWorldGen.Utils
{
    public static class Serializer
    {
        public static string SerializeMap(Block[] map)
        {
            byte[] result;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                using (BinaryWriter binaryWriter = new BinaryWriter(gzipStream))
                {
                    binaryWriter.Write((ushort)0); // version, unused
                    binaryWriter.Write((uint)map.Length);

                    for (uint i = 0; i < map.Length; i++)
                        binaryWriter.Write(map[i]);
                }

                result = memoryStream.ToArray();
            }

            return Convert.ToBase64String(result);
        }
    }
}
