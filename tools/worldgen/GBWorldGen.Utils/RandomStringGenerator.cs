using System;

namespace GBWorldGen.Misc.Utils
{
    public static class RandomStringGenerator
    {
        public static string Generate(int length)
        {
            // https://stackoverflow.com/a/1344258/1837080
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
