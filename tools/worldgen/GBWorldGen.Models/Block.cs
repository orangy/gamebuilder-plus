namespace GBWorldGen.Models
{
    public struct Block
    {
        public short x;
        public short y;
        public short z;
        public SHAPE shape;
        public DIRECTION direction;
        public STYLE style;

        public Block(short x, short y, short z, SHAPE shape, DIRECTION direction, STYLE style)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.shape = shape;
            this.direction = direction;
            this.style = style;
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
    }
}
