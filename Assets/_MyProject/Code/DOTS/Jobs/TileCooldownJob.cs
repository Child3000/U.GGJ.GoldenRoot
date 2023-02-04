using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct TileCooldownJob : IJobFor
{
    public float DeltaTime;

    public TileCooldownContainer TileCooldownContainer;

    public void Execute(int index)
    {
        float countdownTime = this.TileCooldownContainer.na_CountdownTime[index];

        countdownTime = math.max(0.0f, countdownTime - this.DeltaTime);

        this.TileCooldownContainer.na_CountdownTime[index] = countdownTime;
    }
}
