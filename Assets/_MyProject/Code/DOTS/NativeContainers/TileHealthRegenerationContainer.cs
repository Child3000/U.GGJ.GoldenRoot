using Unity.Collections;

public struct TileHealthRegenerationContainer : System.IDisposable
{
    public NativeArray<int> na_TileHealths;
    [ReadOnly] public NativeArray<int> na_TileTotalHealths;
    public NativeArray<float> na_TileHealthDeltaTime;

    public TileHealthRegenerationContainer(int count, Allocator allocator = Allocator.Persistent)
    {
        this.na_TileHealths = new NativeArray<int>(count, allocator);
        this.na_TileTotalHealths = new NativeArray<int>(count, allocator);
        this.na_TileHealthDeltaTime = new NativeArray<float>(count, allocator);
    }

    public void Dispose()
    {
        this.na_TileHealths.Dispose();
        this.na_TileTotalHealths.Dispose();
        this.na_TileHealthDeltaTime.Dispose();
    }
}
