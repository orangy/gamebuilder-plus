using GBWorldGen.Core.Models;
using GBWorldGen.Misc.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GBWorldGen.Core.Algorithms.Transformers
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
                    {
                        binaryWriter.Write(map[i]);
                    }
                }

                result = memoryStream.ToArray();
            }

            return Convert.ToBase64String(result);
        }
    }

    public static class Deserializer
    {
        public static bool DeserializeMap(string mapData)
        {
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

                        Block.SHAPE testShape = (Block.SHAPE)shape;
                        Block.DIRECTION testDirection = (Block.DIRECTION)direction;
                        Block.STYLE testStyle = (Block.STYLE)style;

                        //this.SetCellValue(
                        //  new Cell(x, y, z),
                        //  new CellValue
                        //  {
                        //      blockType = (BlockShape)shape,
                        //      direction = (BlockDirection)direction,
                        //      style = (BlockStyle)style
                        //  });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while deserializing: {ex.Message}.");
                return false;
            }

            return true;
        }
    }
}
