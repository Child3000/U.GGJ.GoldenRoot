using System;
using UnityEngine;

namespace GoldenRoot
{
    public class GridMap2D : MonoBehaviour
    {
        [SerializeField] private Vector2 _CellSize = new Vector2(1, 1);
        [SerializeField] private Vector2Int _GridSize = new Vector2Int(5, 5);
        [SerializeField] private GameObject _TilePrefab;

        private GameObject[] _GridTiles;

        /************************************************************************************************************************/

        // private Vector3 GetCenterCellPosition(int x, int y) => GetCellPosition(x, y) + new Vector3(_CellSize.x, 0f, _CellSize.y) * 0.5f;

        // private Vector3 GetCellPosition(int x, int y) => GridOrigin + new Vector3(x * _CellSize.x, 0f, y * _CellSize.y);

        // private Vector3 GridOrigin => transform.position;

        /************************************************************************************************************************/

        private void Awake()
        {
            this._GridTiles = new GameObject[_GridSize.x * _GridSize.y];

            for (int x = 0; x < this._GridSize.x; x++)
            {
                for (int y = 0; y < this._GridSize.y; y++)
                {
                    int flattenIdx = MathUtil.FlattenIndex(x, y, _GridSize.y);
                    GameObject tile = Instantiate(_TilePrefab, this.transform);
                    tile.transform.position = new Vector3(x + 0.5f, 0.0f, y + 0.5f);

                    this._GridTiles[flattenIdx] = tile;
                }
            }
        }

        // #if UNITY_EDITOR
        // private void OnDrawGizmos()
        // {
        //     // Draw center position.
        //     for (int x = 0; x < _GridSize.x; x++)
        //     {
        //         for (int y = 0; y < _GridSize.y; y++)
        //         {
        //             Gizmos.DrawSphere(GetCenterCellPosition(x, y), 0.1f);
        //         }
        //     }
            
        //     // Draw grid line.
        //     for (int x = 0; x <= _GridSize.x; x++)
        //     {
        //         Gizmos.DrawLine(GetCellPosition(x, 0), GetCellPosition(x, _GridSize.y));
        //     }

        //     for (int y = 0; y <= _GridSize.y; y++)
        //     {
        //         Gizmos.DrawLine(GetCellPosition(0, y), GetCellPosition(_GridSize.x, y));
        //     }
        // }
        // #endif
        /************************************************************************************************************************/
    }
}