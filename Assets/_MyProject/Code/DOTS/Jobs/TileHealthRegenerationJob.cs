using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct TileHealthRegenerationJob : IJobFor
{
    public float DeltaTime;
    public float TileRegenerationInterval;

    public TileHealthRegenerationContainer TileHealthRegenerationContainer;

    public void Execute(int index)
    {
        float healthDeltaTime = this.TileHealthRegenerationContainer.na_TileHealthDeltaTime[index];
        int health = this.TileHealthRegenerationContainer.na_TileHealths[index];
        int totalHealth = this.TileHealthRegenerationContainer.na_TileTotalHealths[index];

        if (health == totalHealth)
        {
            healthDeltaTime = this.TileRegenerationInterval;
        } else if (healthDeltaTime <= 0.0f)
        {
            healthDeltaTime = this.TileRegenerationInterval;
            health += 1;
        } else
        {
            healthDeltaTime -= this.DeltaTime;
        }

        this.TileHealthRegenerationContainer.na_TileHealthDeltaTime[index] = healthDeltaTime;
        this.TileHealthRegenerationContainer.na_TileHealths[index] = health;
    }
}
