using GBWorldGen.Core.Algorithms.Transformers.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GBWorldGen.Core.Algorithms.Transformers
{
    public class Serializer : BaseSerializer<short>
    {
        public override string Serialize(BaseMap<short> param)
        {
            Map map = param as Map;
            List<Block> blocks = map.Blocks();
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                using (BinaryWriter binaryWriter = new BinaryWriter(gzipStream))
                {
                    binaryWriter.Write((ushort)0); // version, unused
                    binaryWriter.Write((uint)blocks.Count);

                    for (int i = 0; i < blocks.Count; i++)
                    {
                        binaryWriter.Write((Block)blocks[i]);
                    }
                }

                result = memoryStream.ToArray();
            }

            return Convert.ToBase64String(result);
        }
    }
}
