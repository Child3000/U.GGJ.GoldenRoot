using System;
using UnityEngine;

namespace GoldenRoot
{
    public class GridMap2D : MonoBehaviour
    {
        [SerializeField] private Vector2 _CellSize = new Vector2(1, 1);
        [SerializeField] private Vector2Int _GridSize = new Vector2Int(5, 5);

        /************************************************************************************************************************/
        
        private Vector3 GetCenterCellPosition(int x, int y) => GetCellPosition(x, y) + new Vector3(_CellSize.x, 0f, _CellSize.y) * 0.5f;
        
        private Vector3 GetCellPosition(int x, int y) => GridOrigin + new Vector3(x * _CellSize.x, 0f, y * _CellSize.y);
        
        private Vector3 GridOrigin => transform.position;
        
        /************************************************************************************************************************/
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Draw center position.
            for (int x = 0; x < _GridSize.x; x++)
            {
                for (int y = 0; y < _GridSize.y; y++)
                {
                    Gizmos.DrawSphere(GetCenterCellPosition(x, y), 0.1f);
                }
            }
            
            // Draw grid line.
            for (int x = 0; x <= _GridSize.x; x++)
            {
                Gizmos.DrawLine(GetCellPosition(x, 0), GetCellPosition(x, _GridSize.y));
            }

            for (int y = 0; y <= _GridSize.y; y++)
            {
                Gizmos.DrawLine(GetCellPosition(0, y), GetCellPosition(_GridSize.x, y));
            }
        }
        #endif
        /************************************************************************************************************************/
    }
}