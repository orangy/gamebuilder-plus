using GBWorldGen.Core.Models.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace GBWorldGen.Core.Models
{
    /// <summary>
    /// A map in the world of Game Builder.
    /// </summary>
    public class Map : BaseMap<short>
    {
        public const short MINWIDTH = -500;
        public const short MINLENGTH = -500;
        public const short MINHEIGHT = -20;
        public const short MAXWIDTH = 500;
        public const short MAXLENGTH = 500;
        public const short MAXHEIGHT = 130;

        public short VoosWidth { get { return (short)(Width * 2.5d); } }
        public short VoosLength { get { return (short)(Length * 2.5d); } }

        public Map(short width, short length, short height, short originWidth = 0, short originLength = 0, short originHeight = 0)
            : base(width, length, height, 
                  originWidth, originLength, originHeight,
                  MINWIDTH, MINLENGTH, MINHEIGHT,
                  MAXWIDTH, MAXLENGTH, MAXHEIGHT)
        {
            MapData = new List<BaseBlock<short>>(width * length * height);
        }

        //public static List<Block> ToList(Block[,,] blocks)
        //{
        //    List<Block> blockList = new List<Block>(
        //        blocks.GetLength(0) *
        //        blocks.GetLength(1) *
        //        blocks.GetLength(2));

        //    for (int x = 0; x < blocks.GetLength(0); x++)
        //        for (int y = 0; y < blocks.GetLength(1); y++)
        //            for (int z = 0; z < blocks.GetLength(2); z++)
        //            {
        //                blockList.Add(blocks[x, y, z]);
        //            }

        //    return blockList;
        //}

        //public static Block[,,] ToBlock3DArray(List<Block> blocks, int width, int length, int height, int minWorldY, int maxWorldY)
        //{
        //    short originX = (short)(width * 0.5d);
        //    short originZ = (short)(length * 0.5d);
        //    Block[,,] returnArray = new Block[width, height, length];
        //    Dictionary<(int, int), int> indexCounter = new Dictionary<(int, int), int>();

        //    // Necessary to get the y-indecees right
        //    blocks.Sort((a, b) =>
        //    {
        //        if (a.X > b.X) return 1;
        //        if (a.X < b.X) return -1;
        //        if (a.Z > b.Z) return 1;
        //        if (a.Z < b.Z) return -1;
        //        if (a.Y > b.Y) return 1;
        //        if (a.Y < b.Y) return -1;

        //        return 0;
        //    });

        //    for (int i = 0; i < blocks.Count; i++)
        //    {
        //        if (!indexCounter.ContainsKey((blocks[i].X + originX, blocks[i].Z + originZ)))
        //            indexCounter[(blocks[i].X + originX, blocks[i].Z + originZ)] = 0;

        //        returnArray[
        //            blocks[i].X + originX,
        //            indexCounter[(blocks[i].X + originX, blocks[i].Z + originZ)], 
        //            blocks[i].Z + originZ] = blocks[i];
        //        indexCounter[(blocks[i].X + originX, blocks[i].Z + originZ)]++;
        //    }

        //    return returnArray;
        //}

        #region Private methods

        /// <summary>
        /// Returns the width of the map in <see cref="short" /> values. 
        /// This supports the map to have non-contiguous blocks and count the
        /// non-contiguous block as part of the width./>
        /// </summary>
        /// <returns></returns>
        public override short GetWidth()
        {
            short minX = 0;
            short maxX = 0;
            for (int i = 0; i < MapData.Count(); i++)
            {
                if (MapData.ElementAt(i).X < minX) minX = MapData.ElementAt(i).X;
                else if (MapData.ElementAt(i).X > maxX) maxX = MapData.ElementAt(i).X;
            }
            
            return (short)(maxX - minX + 1);
        }

        /// <summary>
        /// Returns the length of the map in <see cref="short" /> values. 
        /// This supports the map to have non-contiguous blocks and count the
        /// non-contiguous block as part of the length./>
        /// </summary>
        /// <returns></returns>
        public override short GetLength()
        {
            short minZ = 0;
            short maxZ = 0;
            for (int i = 0; i < MapData.Count(); i++)
            {
                if (MapData.ElementAt(i).Z < minZ) minZ = MapData.ElementAt(i).Z;
                else if (MapData.ElementAt(i).Z > maxZ) maxZ = MapData.ElementAt(i).Z;
            }

            return (short)(maxZ - minZ + 1);
        }

        /// <summary>
        /// Returns the height of the map in <see cref="short" /> values. 
        /// This supports the map to have non-contiguous blocks and count the
        /// non-contiguous block as part of the height./>
        /// </summary>
        /// <returns></returns>
        public override short GetHeight()
        {
            short minY = 0;
            short maxY = 0;
            for (int i = 0; i < MapData.Count(); i++)
            {
                if (MapData.ElementAt(i).Y < minY) minY = MapData.ElementAt(i).Y;
                else if (MapData.ElementAt(i).Y > maxY) maxY = MapData.ElementAt(i).Y;
            }

            return (short)(maxY - minY + 1);
        }        
        #endregion
    }
}
