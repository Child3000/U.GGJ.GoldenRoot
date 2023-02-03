using System;
using UnityEngine;

namespace GoldenRoot
{
    public class GRDebug
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        public static void LogErrorEnumNotImplement(Enum enumType)
        {
            LogError($"Enum not implement={enumType}");
        }
    }
}