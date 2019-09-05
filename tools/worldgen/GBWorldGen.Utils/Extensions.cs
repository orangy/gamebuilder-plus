﻿using GBWorldGen.Core.Models;
using System;
using System.IO;

namespace GBWorldGen.Misc.Utils
{
    [Obsolete("Delete me")]
    public static class Extensions
    {
        public static void Write(this BinaryWriter binaryWriter, Block block)
        {
            binaryWriter.Write(block.X);
            binaryWriter.Write(block.Y);
            binaryWriter.Write(block.Z);
            binaryWriter.Write((byte)block.Shape);
            binaryWriter.Write((byte)block.Direction);
            binaryWriter.Write((ushort)block.Style);
        }
    }
}
