using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct TileAnimationJob : IJobFor
{
    public float DeltaTime;
    public float ShrinkSize;
    public float ShrinkSpeed;
    public float EnlargeSpeed;

    public TileAnimationContainer TileAnimationContainer;

    public void Execute(int index)
    {
        bool shrink = this.TileAnimationContainer.na_Shrink[index];
        float scale = this.TileAnimationContainer.na_Scales[index];

        if (shrink)
        {
            scale = math.max(
                this.ShrinkSize, scale - this.DeltaTime * this.ShrinkSpeed
            );

            // if already reached the largest size, start shrinking
            if (scale <= this.ShrinkSize)
            {
                shrink = false;
            }
        } else
        {
            // enlarge back to size of 1
            scale = math.min(
                1.0f, scale + this.DeltaTime * this.EnlargeSpeed
            );
        }

        this.TileAnimationContainer.na_Scales[index] = scale;
        this.TileAnimationContainer.na_Shrink[index] = shrink;
    }
}
