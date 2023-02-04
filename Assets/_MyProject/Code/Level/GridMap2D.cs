using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;

namespace GoldenRoot
{
    public class GridMap2D : MonoBehaviour, System.IDisposable
    {
        [Header("Grids")]
        [SerializeField] private int2 _GridSize = new int2(5, 5);
        [SerializeField] private GameObject _TilePrefab;

        [SerializeField, Range(0.0f, 8.0f)] private float _ShrinkSpeed;
        [SerializeField, Range(0.0f, 8.0f)] private float _EnlargeSpeed;
        [SerializeField] private int _TileMinHealth;
        [SerializeField] private int _TileMaxHealth;
        [SerializeField, Range(0.0f, 1.0f)] private float _TileShrinkSize;
        [SerializeField] private float _TileCooldownDuration;

        [Space, Header("Roots")]
        [SerializeField] private RootItem[] _RootTypes;

        /************************************************************************************************************************/
        public int CellCount => this._GridSize.x * this._GridSize.y;
        public int RootTypeCount => this._RootTypes.Length;

        private GameObject[] _Tiles;
        private Material[] _TileMaterials;
        private int[] _TileHealths;
        private int[] _TileTotalHealths;
        private int[] _TileRootIndices;
        /************************************************************************************************************************/

        private TileAnimationContainer _TileAnimationContainer;
        private TileCooldownContainer _TileCooldownContainer;

        private void Start()
        {
            this._Tiles = new GameObject[this.CellCount];
            this._TileMaterials = new Material[this.CellCount];
            this._TileHealths = new int[this.CellCount];
            this._TileTotalHealths = new int[this.CellCount];
            this._TileRootIndices = new int[this.CellCount];

            this._TileAnimationContainer = new TileAnimationContainer(this.CellCount);
            this._TileCooldownContainer = new TileCooldownContainer(this.CellCount);

            for (int x = 0; x < this._GridSize.x; x++)
            {
                for (int y = 0; y < this._GridSize.y; y++)
                {
                    int flattenIdx = MathUtil.FlattenIndex(x, y, this._GridSize.y);
                    GameObject tile = Instantiate(_TilePrefab, this.transform);
                    tile.transform.position = new Vector3(x + 0.5f, 0.0f, y + 0.5f);

                    this._Tiles[flattenIdx] = tile;
                    this._TileMaterials[flattenIdx] = tile.GetComponentInChildren<MeshRenderer>().material;
                    int health = this.GetRandomHealth();
                    this._TileHealths[flattenIdx] = health;
                    this._TileTotalHealths[flattenIdx] = health;
                    this._TileRootIndices[flattenIdx] = this.GetRandomRootIndex();
                }
            }
        }

        private void Update()
        {
            // perform animation
            TileAnimationJob tileAnimationJob = new TileAnimationJob
            {
                DeltaTime = Time.deltaTime,
                ShrinkSize = this._TileShrinkSize,
                ShrinkSpeed = this._ShrinkSpeed,
                EnlargeSpeed = this._EnlargeSpeed,

                TileAnimationContainer = this._TileAnimationContainer,
            };

            // perform cooldown for each tile
            TileCooldownJob tileCooldownJob = new TileCooldownJob
            {
                DeltaTime = Time.deltaTime,

                TileCooldownContainer = this._TileCooldownContainer,
            };

            JobHandle jobHandle0 = tileAnimationJob.ScheduleParallel(this.CellCount, 128, default);
            JobHandle jobHandle1 = tileCooldownJob.ScheduleParallel(this.CellCount, 128, default);

            JobHandle jobHandle = JobHandle.CombineDependencies(jobHandle0, jobHandle1);

            jobHandle.Complete();

            // update tile scales
            for (int t = 0; t < this.CellCount; t++)
            {
                float scale = _TileAnimationContainer.na_Scales[t];

                this._Tiles[t].transform.localScale = Vector3.one * scale;
                this._TileMaterials[t].SetFloat(ShaderID._Pulse, (1.0f - scale) / this._TileShrinkSize);

                if (this._TileCooldownContainer.na_CountdownTime[t] == 0.0f)
                {
                    int totalHealth = this._TileTotalHealths[t];
                    int health = this._TileHealths[t];
                    float ratio = (float)health / (float)totalHealth;

                    this._TileMaterials[t].SetFloat(ShaderID._Transition, math.lerp(1.0f, -1.0f, ratio));
                } else
                {
                    this._TileMaterials[t].SetFloat(ShaderID._Transition, 1.0f);
                }
            }
        }

        /// <summary>Apply damage to a tile and animate it.</summary>
        public void DigTile(int x, int y, int damage, PlayerReference.PlayerID playerID)
        {
            if (x < 0 || y < 0)
            {
                return;
            }

            if (x >= _GridSize.x || y >= _GridSize.y)
            {
                return;
            }

            int flattenIdx = MathUtil.FlattenIndex(x, y, this._GridSize.y);

            // perform tile animation
            this._TileAnimationContainer.na_Shrink[flattenIdx] = true;

            // if not cooldown yet, do nothing
            if (this._TileCooldownContainer.na_CountdownTime[flattenIdx] > 0.0f)
            {
                Debug.Log($"tile: {x}, {y} is on cooldown...");
                return;
            }

            int health = this._TileHealths[flattenIdx];
            int rootIdx = this._TileRootIndices[flattenIdx];
            health -= damage;

            if (health <= 0)
            {
                Debug.Assert(rootIdx < this.RootTypeCount, "given rootIdx is larger than RootTypeCount.");

                RootItem rootItem = this._RootTypes[rootIdx];
                Debug.Log($"{playerID} broke tile: {x}, {y}, and got {rootItem.Point} points.");

                // reward player with points
                GamePointManager.Singleton.AddPoints(playerID, rootItem.Point);
                // put tile in cooldown
                this._TileCooldownContainer.na_CountdownTime[flattenIdx] = this._TileCooldownDuration;

                // generate new health and root root index
                health = this.GetRandomHealth();
                rootIdx = this.GetRandomRootIndex();

                this._TileTotalHealths[flattenIdx] = health;
            }

            this._TileHealths[flattenIdx] = health;
            this._TileRootIndices[flattenIdx] = rootIdx;
        }

        private int GetRandomHealth()
        {
            return UnityEngine.Random.Range(
                this._TileMinHealth, this._TileMaxHealth + 1
            );
        }

        private int GetRandomRootIndex()
        {
            int probablitySum = 0;

            for (int r = 0; r < this.RootTypeCount; r++)
            {
                probablitySum += this._RootTypes[r].Probability;
            }

            int randProb = UnityEngine.Random.Range(0, probablitySum);

            int probAccumulation = 0;
            for (int r = 0; r < this.RootTypeCount; r++)
            {
                probAccumulation += this._RootTypes[r].Probability;
                if (randProb < probAccumulation)
                {
                    return r;
                }
            }

            return 0;
        }

        public void Dispose()
        {
            this._TileAnimationContainer.Dispose();
            this._TileCooldownContainer.Dispose();
        }

        private void OnDestroy()
        {
            this.Dispose();
        }

        /************************************************************************************************************************/
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // draw vertical grid lines
            for (int x = 0; x <= _GridSize.x; x++)
            {
                Gizmos.DrawLine(new Vector3(x, 0.0f, 0.0f), new Vector3(x, 0.0f, _GridSize.y));
            }

            // draw horizontal grid lines
            for (int y = 0; y <= _GridSize.y; y++)
            {
                Gizmos.DrawLine(new Vector3(0.0f, 0.0f, y), new Vector3(_GridSize.x, 0.0f, y));
            }
        }

        private void OnValidate()
        {
            for (int r = 0; r < this._RootTypes.Length; r++)
            {
                this._RootTypes[r].OnValidate();
            }
        }
        #endif
        /************************************************************************************************************************/
    }
}