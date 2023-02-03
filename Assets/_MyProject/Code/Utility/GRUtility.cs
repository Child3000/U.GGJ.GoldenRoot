using System;
using UnityEngine;

namespace GoldenRoot
{
    public static class GRUtility
    {
        public static float AtLeast(float val, float min)
        {
            if (val < min) val = min;
            return val;
        }
        
        public static int ToInt(this bool boolean)
        {
            return Convert.ToInt32(boolean);
        }

        public static bool GetComponentInParentOrChildren<T>(this GameObject obj, ref T component)
        {
            if (obj == null) return false;
            var c = obj.GetComponentInParent<T>();
            if (c == null)
            {
                c = obj.GetComponentInChildren<T>();
            }
            component = c;
            return component != null;
        }
    }
}