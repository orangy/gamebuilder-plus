using GBWorldGen.Core.Models;
using System;
using Xunit;

namespace ModelsTests
{
    public class ModelsTests
    {
        [Fact]
        public void Block_Equality_Finds_Duplicate()
        {
            Block block1 = new Block
            {
                X = 14,
                Y = 10,
                Z = -2,
                Style = Block.STYLE.Burgundy,
                Direction = Block.DIRECTION.North,
                Shape = Block.SHAPE.Triangle
            };
            Block block2 = new Block
            {
                X = 14,
                Y = 10,
                Z = -2,
                Style = Block.STYLE.Burgundy,
                Direction = Block.DIRECTION.North,
                Shape = Block.SHAPE.Triangle
            };

            Assert.True(block1.Equals(block2));
        }

        [Fact]
        public void Block_Equality_Does_Not_Find_Duplicate()
        {
            Block block1 = new Block
            {
                X = 14,
                Y = 10,
                Z = -2,
                Style = Block.STYLE.Burgundy,
                Direction = Block.DIRECTION.North,
                Shape = Block.SHAPE.Triangle
            };
            Block block2 = new Block
            {
                X = 14,
                Y = 8,
                Z = -2,
                Style = Block.STYLE.Burgundy,
                Direction = Block.DIRECTION.North,
                Shape = Block.SHAPE.Triangle
            };

            Assert.False(block1.Equals(block2));
        }
    }
}
