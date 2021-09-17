using System;
using System.Linq;

namespace Helpers
{
    public static class SoftwareVersionHelper
    {
        public static int Compare(string version_1, string version_2)
        {
            if (version_1 == version_2) return 0;
            var splitted_1 = version_1.Split('.').Select(x => Convert.ToInt32(x)).ToArray();
            var splitted_2 = version_2.Split('.').Select(x => Convert.ToInt32(x)).ToArray();
            for (int ii = 0; ii < splitted_1.Length; ii++)
            {
                if (splitted_1[ii] > splitted_2[ii]) return 1;
                if (splitted_1[ii] < splitted_2[ii]) return -1;
            }
            return 0;
        }
    }
}