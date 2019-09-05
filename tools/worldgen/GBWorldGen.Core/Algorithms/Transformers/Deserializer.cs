using GBWorldGen.Core.Algorithms.Transformers.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace GBWorldGen.Core.Algorithms.Transformers
{
    public class Deserializer : BaseDeserializer<short>
    {
        public override BaseMap<short> Deserialize(string serialized)
        {
            BaseMap<short> map = null;
            List<Block> returnBlocks = new List<Block>();
            List<short> xs = new List<short>();
            List<short> zs = new List<short>();
            List<short> ys = new List<short>();

            try
            {
                byte[] zippedBytes = System.Convert.FromBase64String(serialized);
                using (var zippedStream = new MemoryStream(zippedBytes, 0, zippedBytes.Length))
                using (var unzipped = new GZipStream(zippedStream, CompressionMode.Decompress))
                using (BinaryReader reader = new BinaryReader(unzipped))
                {
                    int version = reader.ReadUInt16(); // Unused.
                    uint numBlocks = reader.ReadUInt32();
                    for (int i = 0; i < numBlocks; i++)
                    {
                        short x = reader.ReadInt16();
                        short y = reader.ReadInt16();
                        short z = reader.ReadInt16();
                        byte shape = reader.ReadByte();
                        byte direction = reader.ReadByte();
                        ushort style = reader.ReadUInt16();

                        if (!xs.Contains(x)) xs.Add(x);
                        if (!zs.Contains(z)) zs.Add(z);
                        if (!ys.Contains(y)) ys.Add(y);

                        returnBlocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Z = z,
                            Shape = (Block.SHAPE)shape,
                            Direction = (Block.DIRECTION)direction,
                            Style = (Block.STYLE)style
                        });
                    }
                }

                map = new Map((short)xs.Count, (short)zs.Count, (short)ys.Count,
                    (short)(xs.Count * -0.5d), (short)(zs.Count * -0.5d), 0);
                map.MapData = new List<BaseBlock<short>>(returnBlocks);
            }
            catch (Exception)
            {
                //Debug.WriteLine($"Error occurred while deserializing: {ex.Message}.");
            }

            return map;
        }
    }
}
