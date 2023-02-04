using Unity.Collections;

public struct TileAnimationContainer : System.IDisposable
{
    public NativeArray<float> na_Scales;
    public NativeArray<bool> na_Shrink;

    public TileAnimationContainer(int count, Allocator allocator = Allocator.Persistent)
    {
        this.na_Scales = new NativeArray<float>(count, allocator, NativeArrayOptions.UninitializedMemory);
        this.na_Shrink = new NativeArray<bool>(count, allocator, NativeArrayOptions.ClearMemory);

        for (int i = 0; i < count; i++)
        {
            this.na_Scales[i] = 1.0f;
        }
    }

    public void Dispose()
    {
        this.na_Scales.Dispose();
        this.na_Shrink.Dispose();
    }
}
