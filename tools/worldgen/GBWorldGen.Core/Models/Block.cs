using GBWorldGen.Core.Models.Abstractions;
using System;

namespace GBWorldGen.Core.Models
{
    /// <summary>
    /// A block in the world of Game Builder.
    /// </summary>
    public class Block : BaseBlock<short>
    {
        public SHAPE Shape { get; set; } = SHAPE.Box;
        public DIRECTION Direction { get; set; } = DIRECTION.East;
        public STYLE Style { get; set; } = STYLE.Grass;

        #region Enums
        /// <summary>
        /// The shape of the block in the world.
        /// </summary>
        public enum SHAPE : byte
        {
            Empty = 0,
            Box,
            Triangle,
            Ramp,
            Corner
        }

        /// <summary>
        /// The direction of the block is facing in the world.
        /// </summary>
        public enum DIRECTION : byte
        {
            North = 0,
            East,
            South,
            West
        }

        /// <summary>
        /// The style of the block in the world.
        /// </summary>
        public enum STYLE : ushort
        {
            White = 0,
            Gray,
            Red,
            Yellow,
            Green,
            Cyan,
            Blue,
            Pink,
            LightBlue,
            LightOrange,
            Salmon,
            BrightRed,
            Burgundy,
            Teal,
            BlueGreen,
            LightPink,
            Stone,
            Space,
            Grass,
            Snow,
            Dirt,
            GrassStone,
            GrayCraters,
            Ice,
            Lava,
            Sand,
            Water,
            Wood,
            RedCraters,
            IndustrialGreen,
            IndustrialRed,
            GrayBricks,
            MetalBeige,
            RedBricks,
            Road,
            RoadWCrossing,
            RoadWWhiteBrokenLine,
            RoadWYellowContinuousLine,
            Pavement,
            PavementConcaveCorner,
            PavementConvexCorner
        }
        #endregion

        public Block() { }
        public Block(short x, short y, short z, SHAPE shape, DIRECTION direction, STYLE style)
            : base(x, y, z)
        {
            Shape = shape;
            Direction = direction;
            Style = style;
        }

        #region Private methods
        private string EnumName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
        #endregion        

        #region Overrides
        public override string ToString()
        {
            return $"(X:{X}, Y:{Y}, Z:{Z}) (Shape:{EnumName(Shape)}, Direction:{EnumName(Direction)}, Style:{EnumName(Style)})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Block)
            {
                Block compareTo = (Block)obj;
                return X == compareTo.X &&
                    Y == compareTo.Y &&
                    Z == compareTo.Z &&
                    Shape == compareTo.Shape &&
                    Direction == compareTo.Direction &&
                    Style == compareTo.Style;
            }

            return false;
        }

        public override int GetHashCode()
        {
            // This forces the compiler to call Equals(object obj)
            return 0;
        }
        #endregion               
    }
}
