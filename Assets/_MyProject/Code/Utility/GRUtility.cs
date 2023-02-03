using System;
using UnityEngine;

namespace GoldenRoot
{
    public static class GRUtility
    {
        public static int ToInt(this bool boolean)
        {
            return Convert.ToInt32(boolean);
        }
    }
}