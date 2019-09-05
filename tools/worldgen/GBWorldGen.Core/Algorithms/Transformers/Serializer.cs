using GBWorldGen.Core.Algorithms.Transformers.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using System;
using System.IO;
using System.IO.Compression;

namespace GBWorldGen.Core.Algorithms.Transformers
{
    public class Serializer : BaseSerializer<short>
    {
        public override string Serialize(BaseMap<short> map)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                using (BinaryWriter binaryWriter = new BinaryWriter(gzipStream))
                {
                    binaryWriter.Write((ushort)0); // version, unused
                    binaryWriter.Write((uint)map.MapData.Count);

                    for (int i = 0; i < map.MapData.Count; i++)
                    {
                        binaryWriter.Write((Block)map.MapData[i]);
                    }
                }

                result = memoryStream.ToArray();
            }

            return Convert.ToBase64String(result);
        }
    }
}
