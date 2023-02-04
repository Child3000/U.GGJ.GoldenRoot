using Unity.Mathematics;
using Unity.Jobs;

public struct TileAnimationJob : IJobFor
{
    public float DeltaTime;
    public float EnlargeSize;
    public float EnlargeSpeed;
    public float ShrinkSpeed;

    public TileAnimationContainer TileAnimationContainer;

    public void Execute(int index)
    {
        bool enlarge = this.TileAnimationContainer.na_Enlarge[index];
        float scale = this.TileAnimationContainer.na_Scales[index];

        if (enlarge)
        {
            scale = math.min(
                this.EnlargeSize, scale + this.DeltaTime * this.EnlargeSpeed
            );

            // if already reached the largest size, start shrinking
            if (scale >= this.EnlargeSize)
            {
                enlarge = false;
            }
        } else
        {
            // shrink back to size of 1
            scale = math.max(
                1.0f, scale - this.DeltaTime * this.ShrinkSpeed
            );
        }

        this.TileAnimationContainer.na_Scales[index] = scale;
        this.TileAnimationContainer.na_Enlarge[index] = enlarge;
    }
}
