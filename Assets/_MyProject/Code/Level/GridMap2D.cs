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

        [Space, Header("Roots")]
        [SerializeField] private RootItem[] _RootTypes;

        private int _FlattenCount => _GridSize.x * _GridSize.y;
        private GameObject[] _Tiles;
        private int[] _TileHealths;

        private TileAnimationContainer _TileAnimationContainer;

        private void Awake()
        {
            this._Tiles = new GameObject[this._FlattenCount];
            this._TileHealths = new int[this._FlattenCount];

            this._TileAnimationContainer = new TileAnimationContainer(this._FlattenCount);

            for (int x = 0; x < this._GridSize.x; x++)
            {
                for (int y = 0; y < this._GridSize.y; y++)
                {
                    int flattenIdx = MathUtil.FlattenIndex(x, y, this._GridSize.y);
                    GameObject tile = Instantiate(_TilePrefab, this.transform);
                    tile.transform.position = new Vector3(x + 0.5f, 0.0f, y + 0.5f);

                    this._Tiles[flattenIdx] = tile;
                    this._TileHealths[flattenIdx] = this.GetRandomHealth();
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

                TileAnimationContainer = this._TileAnimationContainer
            };

            JobHandle jobHandle = default;
            jobHandle = tileAnimationJob.ScheduleParallel(this._FlattenCount, 128, jobHandle);

            jobHandle.Complete();

            // update tile scales
            for (int t = 0; t < this._FlattenCount; t++)
            {
                this._Tiles[t].transform.localScale = Vector3.one *
                    _TileAnimationContainer.na_Scales[t];
            }

            if (Input.GetKey(KeyCode.Space))
            {
                this.DigTile(0, 0, 0);
            }
        }

        /// <summary>Apply damage to a tile and animate it.</summary>
        public void DigTile(int x, int y, int damage)
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

            int health = this._TileHealths[flattenIdx];
            health -= damage;

            if (health <= 0)
            {
                health = this.GetRandomHealth();
            }

            this._TileHealths[flattenIdx] = health;
            this._TileAnimationContainer.na_Shrink[flattenIdx] = true;
        }

        private int GetRandomHealth()
        {
            return UnityEngine.Random.Range(
                this._TileMinHealth, this._TileMaxHealth + 1
            );
        }

        public void Dispose()
        {
            this._TileAnimationContainer.Dispose();
        }

        private void OnDestroy()
        {
            this.Dispose();
        }

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
    }
}