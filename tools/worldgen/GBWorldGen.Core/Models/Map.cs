using GBWorldGen.Core.Models.Abstractions;
using System;
using System.Collections.Concurrent;
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
            MapData = new ConcurrentBag<Block>();
        }

        public void Add(Block b)
        {            
            ((ConcurrentBag<Block>)MapData).Add(b);
        }

        public void AddRange(IEnumerable<Block> blocks)
        {
            for (int i = 0; i < blocks.Count(); i++)
            {
                ((ConcurrentBag<Block>)MapData).Add(blocks.ElementAt(i));
            }
        }

        public void RemoveAll(IEnumerable<Block> blocks)
        {
            List<Block> bag = ((ConcurrentBag<Block>)MapData).ToList();

            // https://stackoverflow.com/a/22667558/1837080
            bag.RemoveAll(new HashSet<Block>(blocks).Contains);
            MapData = new ConcurrentBag<Block>(bag);
        }

        public bool Any(Func<BaseBlock<short>, bool> predicate)
        {
            return ((ConcurrentBag<Block>)MapData).ToList().Any(predicate);
        }

        public List<Block> Blocks()
        {
            return ((ConcurrentBag<Block>)MapData).ToArray().ToList();
        }

        public int Count()
        {            
            return ((ConcurrentBag<Block>)MapData).Count;
        }

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
