using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;
using System.Collections.Generic;

namespace GBWorldGen.Core.Algorithms.Generators
{
    // https://github.com/keijiro/PerlinNoise/blob/master/Assets/Perlin.cs
    public class PerlinNoiseGenerator : WorldData, IGenerateWorld
    {
        public int Width { get; set; }
        public int Length { get; set; }
        public float HillFrequency { get; set; }
        public int Gradient { get; set; }

        private readonly Block[] Blocks;
        Block.STYLE DefaultBlockStyle { get; set; }

        public PerlinNoiseGenerator(int width, int length, float hillFrequency = 1.35F, int gradient = 9, int? seed = null, Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            Console.WriteLine($"Creating a Perlin Noise map with a width of {width}, a length of {length}, gradient of {gradient} and default block style of {defaultBlockStyle}.");

            Width = width;
            Length = length;
            HillFrequency = hillFrequency;
            Gradient = gradient;

            perm = RandomPermutationSeed(seed);
            Blocks = new Block[Width * Length];
            DefaultBlockStyle = defaultBlockStyle;

            Initialize();
        }

        public Map Generate()
        {
            Console.WriteLine("Generating map...");

            float e = 0.0F;
            float ni = 0.0F;
            float nj = 0.0f;
            int gradient = Math.Abs(Gradient);

            for (int j = 1; j <= Width; j++)
            {
                for (int i = 1; i <= Length; i++)
                {
                    ni = ((float)i) / Width;
                    nj = ((float)j) / Length;

                    e = 1F * Noise((HillFrequency + 0.01F) * ni, (HillFrequency + 0.01F) * nj) +
                        0.5F * Noise((HillFrequency * 2.0F + 0.01F) * ni, (HillFrequency * 2.0F + 0.01F) * nj) +
                        0.25F * Noise((HillFrequency * 4.0F + 0.01F) * ni, (HillFrequency * 4.0F + 0.01F) * nj);
                    e = (float)(Math.Round(e * 60d, 4));

                    Blocks[((j - 1) * Length) + (i - 1)].Y = (short)(FloorToInt(e) + gradient);
                }
            }

            return new Map
            {
                Width = (int)(Width * 2.5d),
                Length = (int)(Length * 2.5d),
                BlockData = Blocks
            };
        }

        private void Initialize()
        {
            short originX = (short)(Width * -0.5d);
            short originZ = (short)(Length * -0.5d);
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].X = (short)(i / Length + originZ);
                Blocks[i].Y = 0;
                Blocks[i].Z = (short)(i % Width + originX);

                Blocks[i].Shape = Block.SHAPE.Box;
                Blocks[i].Direction = Block.DIRECTION.East;
                Blocks[i].Style = DefaultBlockStyle;
            }
        }

        #region Noise functions

        public float Noise(float x)
        {
            var X = FloorToInt(x) & 0xff;
            x -= Floor(x);
            var u = Fade(x);
            return Lerp(u, Grad(perm[X], x), Grad(perm[X + 1], x - 1)) * 2;
        }

        public float Noise(float x, float y)
        {
            var X = FloorToInt(x) & 0xff;
            var Y = FloorToInt(y) & 0xff;
            x -= Floor(x);
            y -= Floor(y);
            var u = Fade(x);
            var v = Fade(y);
            var A = (perm[X] + Y) & 0xff;
            var B = (perm[X + 1] + Y) & 0xff;
            return Lerp(v, Lerp(u, Grad(perm[A], x, y), Grad(perm[B], x - 1, y)),
                           Lerp(u, Grad(perm[A + 1], x, y - 1), Grad(perm[B + 1], x - 1, y - 1)));
        }

        //public static float Noise(Vector2 coord)
        //{
        //    return Noise(coord.x, coord.y);
        //}

        public float Noise(float x, float y, float z)
        {
            int X = FloorToInt(x) & 0xff;
            int Y = FloorToInt(y) & 0xff;
            int Z = FloorToInt(z) & 0xff;
            x -= Floor(x);
            y -= Floor(y);
            z -= Floor(z);
            var u = Fade(x);
            var v = Fade(y);
            var w = Fade(z);
            var A = (perm[X] + Y);// & 0xff;
            var B = (perm[X + 1] + Y) & 0xff;
            var AA = (perm[A] + Z) & 0xff;
            var BA = (perm[B] + Z) & 0xff;
            var AB = (perm[A + 1] + Z) & 0xff;
            var BB = (perm[B + 1] + Z) & 0xff;
            return Lerp(w, Lerp(v, Lerp(u, Grad(perm[AA], x, y, z), Grad(perm[BA], x - 1, y, z)),
                                   Lerp(u, Grad(perm[AB], x, y - 1, z), Grad(perm[BB], x - 1, y - 1, z))),
                           Lerp(v, Lerp(u, Grad(perm[AA + 1], x, y, z - 1), Grad(perm[BA + 1], x - 1, y, z - 1)),
                                   Lerp(u, Grad(perm[AB + 1], x, y - 1, z - 1), Grad(perm[BB + 1], x - 1, y - 1, z - 1))));
        }

        //public static float Noise(Vector3 coord)
        //{
        //    return Noise(coord.x, coord.y, coord.z);
        //}

        #endregion

        #region fBm functions

        //public static float Fbm(float x, int octave)
        //{
        //    var f = 0.0f;
        //    var w = 0.5f;
        //    for (var i = 0; i < octave; i++)
        //    {
        //        f += w * Noise(x);
        //        x *= 2.0f;
        //        w *= 0.5f;
        //    }
        //    return f;
        //}

        //public static float Fbm(Vector2 coord, int octave)
        //{
        //    var f = 0.0f;
        //    var w = 0.5f;
        //    for (var i = 0; i < octave; i++)
        //    {
        //        f += w * Noise(coord);
        //        coord *= 2.0f;
        //        w *= 0.5f;
        //    }
        //    return f;
        //}

        //public static float Fbm(float x, float y, int octave)
        //{
        //    return Fbm(new Vector2(x, y), octave);
        //}

        //public static float Fbm(Vector3 coord, int octave)
        //{
        //    var f = 0.0f;
        //    var w = 0.5f;
        //    for (var i = 0; i < octave; i++)
        //    {
        //        f += w * Noise(coord);
        //        coord *= 2.0f;
        //        w *= 0.5f;
        //    }
        //    return f;
        //}

        //public static float Fbm(float x, float y, float z, int octave)
        //{
        //    return Fbm(new Vector3(x, y, z), octave);
        //}

        #endregion

        #region Private functions

        private float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }

        private float Grad(int hash, float x)
        {
            return (hash & 1) == 0 ? x : -x;
        }

        private float Grad(int hash, float x, float y)
        {
            return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
        }

        private float Grad(int hash, float x, float y, float z)
        {
            var h = hash & 15;
            var u = h < 8 ? x : y;
            var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        private float Floor(float number)
        {
            return (float)Math.Floor(number);
        }

        private int FloorToInt(float number)
        {
            if (number % 1 == 0)
                return (int)number;
            return (int)Math.Floor(number);
        }

        private int[] RandomPermutationSeed(int? seed)
        {
            Random rand = seed.HasValue ? new Random(seed.Value) : new Random();
            List<int> values = new List<int>(512);
            for (int i = 0; i < 512; i++)
            {
                values.Add(rand.Next(0, 256));
            }

            return values.ToArray();
        }

        #endregion

        private readonly int[] perm;

        private readonly int[] KensPermlin =
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151
        };
    }
}