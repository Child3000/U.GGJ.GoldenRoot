using System;
using UnityEngine;

namespace GoldenRoot.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private GlobalInputMapping.PlayerType _PlayerType;
        
        public Vector2 MoveAxis => new Vector2(
                                GlobalInputMapping.IsKey(GlobalInputMapping.InputMap.MoveRight, _PlayerType).ToInt() - GlobalInputMapping.IsKey(GlobalInputMapping.InputMap.MoveLeft, _PlayerType).ToInt(),
                                GlobalInputMapping.IsKey(GlobalInputMapping.InputMap.MoveUp, _PlayerType).ToInt() - GlobalInputMapping.IsKey(GlobalInputMapping.InputMap.MoveDown, _PlayerType).ToInt());

        public bool IsDig => GlobalInputMapping.IsDown(GlobalInputMapping.InputMap.DigAction, _PlayerType);

        private void Update()
        {
            GRDebug.Log($"MoveAxis:{MoveAxis}");
            GRDebug.Log($"IsDig:{IsDig}");
        }
    }
}