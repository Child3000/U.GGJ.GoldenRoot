using System;
using UnityEngine;

namespace GoldenRoot
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerReference _PlayerReference;
        
        public Vector2 MoveAxis => new Vector2(
                                IsKey(GlobalInputMapping.InputMap.MoveRight).ToInt() - IsKey(GlobalInputMapping.InputMap.MoveLeft).ToInt(),
                                IsKey(GlobalInputMapping.InputMap.MoveUp).ToInt() - IsKey(GlobalInputMapping.InputMap.MoveDown).ToInt());

        public bool IsDig => GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.DigAction, _PlayerReference.Type);

        private bool IsKey(GlobalInputMapping.InputMap action) => GlobalInputMapping.IsKey(action, _PlayerReference.Type);
        
        /************************************************************************************************************************/
        #if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _PlayerReference);
        }
        #endif
        /************************************************************************************************************************/
    }
}