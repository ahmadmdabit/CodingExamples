using System;

namespace Helpers
{
    public static class LSBHelper
    {
        public static string GetFirstLSB(int number, EndieanType endieanType = EndieanType.LittleEndian)
        {
            string binary = Convert.ToString(number, 2);
            if (binary.Length < 2) binary = $"0{binary}";
            return endieanType == EndieanType.LittleEndian ? binary.Substring(binary.Length - 1) : binary.Substring(0, 1);
        }
        public static string GetSecondLSB(int number, EndieanType endieanType = EndieanType.LittleEndian)
        {
            string binary = Convert.ToString(number, 2);
            if (binary.Length < 2) binary = $"0{binary}";
            //Console.WriteLine($"{number} {binary}");
            return endieanType == EndieanType.LittleEndian ? binary.Substring(binary.Length - 2, 1) : binary.Substring(1, 1);
        }
    }

    public enum EndieanType
    {
        LittleEndian, //R
        BigEndian, //L
    }
}
