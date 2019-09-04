using GBWorldGen.Core.Models;
using GBWorldGen.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GBWorldGen.Core.Algorithms.Transformers
{
    public static class Serializer
    {
        public static string SerializeMap(Map map)
        {
            byte[] result;
            int actual = Map.ToList(map.BlockData).Where(b => b.Shape != Block.SHAPE.Empty).Count();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                using (BinaryWriter binaryWriter = new BinaryWriter(gzipStream))
                {
                    binaryWriter.Write((ushort)0); // version, unused
                    binaryWriter.Write((uint)actual);

                    for (uint x = 0; x < map.BlockData.GetLength(0); x++)
                        for (uint y = 0; y < map.BlockData.GetLength(1); y++)
                            for (uint z = 0; z < map.BlockData.GetLength(2); z++)
                                if (map.BlockData[x, y, z].Shape != Block.SHAPE.Empty)
                                    binaryWriter.Write(map.BlockData[x, y, z]);
                }

                result = memoryStream.ToArray();
            }

            return Convert.ToBase64String(result);
        }
    }

    public static class Deserializer
    {
        public static Block[] DeserializeMap(string mapData)
        {
            List<Block> returnBlocks = new List<Block>();

            try
            {
                byte[] zippedBytes = System.Convert.FromBase64String(mapData);
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

                        Block.SHAPE actualShape = (Block.SHAPE)shape;
                        Block.DIRECTION actualDirection = (Block.DIRECTION)direction;
                        Block.STYLE actualStyle = (Block.STYLE)style;

                        returnBlocks.Add(new Block
                        {
                            X = x,
                            Y = y,
                            Z = z,
                            Shape = actualShape,
                            Direction = actualDirection,
                            Style = actualStyle
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while deserializing: {ex.Message}.");
            }

            return returnBlocks.ToArray();
        }
    }
}
