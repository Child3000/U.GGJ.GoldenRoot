using Unity.Collections;

public struct TileCooldownContainer : System.IDisposable
{
    public NativeArray<float> na_CountdownTime;

    public TileCooldownContainer(int count, Allocator allocator = Allocator.Persistent)
    {
        this.na_CountdownTime = new NativeArray<float>(count, allocator, NativeArrayOptions.ClearMemory);
    }

    public void Dispose()
    {
        this.na_CountdownTime.Dispose();
    }
}
