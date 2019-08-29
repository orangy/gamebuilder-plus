using System;

namespace GBWorldGen.Core.Models
{
    public struct Block
    {
        public short X;
        public short Y;
        public short Z;
        public SHAPE Shape;
        public DIRECTION Direction;
        public STYLE Style;

        public Block(short x, short y, short z, SHAPE shape, DIRECTION direction, STYLE style)
        {
            X = x;
            Y = y;
            Z = z;
            Shape = shape;
            Direction = direction;
            Style = style;
        }

        public enum SHAPE : byte
        {
            Empty = 0,
            Box,
            Triangle,
            Ramp,
            Corner
        }

        public enum DIRECTION : byte
        {
            North = 0,
            East,
            South,
            West
        }

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
            return 0;
        }

        public override string ToString()
        {
            return $"(X:{X}, Y:{Y}, Z:{Z}) (Shape:{EnumName(Shape)}, Direction:{EnumName(Direction)}, Style:{EnumName(Style)})";
        }

        private string EnumName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
    }
}
