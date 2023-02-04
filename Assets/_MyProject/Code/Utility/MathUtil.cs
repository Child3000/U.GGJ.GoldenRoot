using UnityEngine;

public static class MathUtil
{
    public static int FlattenIndex(int x, int y, int width)
    {
        return x + y * width;
    }
}
