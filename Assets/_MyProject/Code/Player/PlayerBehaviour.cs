using System;
using UnityEngine;
using Unity.Mathematics;

namespace GoldenRoot
{
    public class PlayerBehaviour : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private PlayerReference _PlayerReference;
        private PlayerInput PlayerInput => _PlayerReference.Input;
        /************************************************************************************************************************/
        [SerializeField] private GridMap2D _GridMap2D;

        private void Update()
        {
            if (PlayerInput.IsDig)
            {
                float3 position = this.transform.position;
                int3 gridIdx = (int3)position;

                this._GridMap2D.DigTile(gridIdx.x, gridIdx.z, 1);
            }

            if (PlayerInput.IsAttack)
            {
                Debug.Log("IsAttack");
            }
        }
    }
}
