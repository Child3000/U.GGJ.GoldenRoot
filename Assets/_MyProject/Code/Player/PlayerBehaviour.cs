using System;
using UnityEngine;

namespace GoldenRoot
{
    public class PlayerBehaviour : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private PlayerReference _PlayerReference;
        private PlayerInput PlayerInput => _PlayerReference.Input;
        /************************************************************************************************************************/

        private void Update()
        {
            if (PlayerInput.IsDig)
            {
                // Do dig thing.
            }
        }
    }
}