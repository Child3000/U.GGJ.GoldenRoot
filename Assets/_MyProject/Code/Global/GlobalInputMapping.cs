using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoldenRoot
{
    /// <summary>
    /// A singleton that manages the inputs mapping of an action to actual physical key.
    /// </summary>
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class GlobalInputMapping : MonoBehaviour
    {
        /************************************************************************************************************************/
        
        private const int DefaultExecutionOrder = -5000;

        /************************************************************************************************************************/

        public enum PlayerType
        {
            P1, P2
        };
        
        public enum InputMap
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            DigAction    
        }
        
        [System.Serializable]
        public class InputBinding
        {
            public InputMap Action;
            public KeyCode Keyboard;
            public KeyCode Gamepad;
        }

        /************************************************************************************************************************/
        // P1 Binding
        /************************************************************************************************************************/
        
        [SerializeField] private InputBinding[] _InputBindingsP1;

        private Dictionary<InputMap, InputBinding> _InputMapDictP1;

        private Dictionary<InputMap, InputBinding> InputMapDictP1
        {
            get
            {
                if (_InputMapDictP1 == null)
                {
                    _InputMapDictP1 = new Dictionary<InputMap, InputBinding>();
                    foreach (var binding in _InputBindingsP1)
                    {
                        _InputMapDictP1.Add(binding.Action, binding);
                    }
                }

                return _InputMapDictP1;
            }

            set => _InputMapDictP1 = value;
        }

        /************************************************************************************************************************/
        // P2 Binding
        /************************************************************************************************************************/
        
        [SerializeField] private InputBinding[] _InputBindingsP2;

        private Dictionary<InputMap, InputBinding> _InputMapDictP2;

        private Dictionary<InputMap, InputBinding> InputMapDictP2
        {
            get
            {
                if (_InputMapDictP2 == null)
                {
                    _InputMapDictP2 = new Dictionary<InputMap, InputBinding>();
                    foreach (var binding in _InputBindingsP2)
                    {
                        _InputMapDictP2.Add(binding.Action, binding);
                    }
                }

                return _InputMapDictP2;
            }

            set => _InputMapDictP2 = value;
        }

        /************************************************************************************************************************/
        // Dynamic Player Map
        /************************************************************************************************************************/

        private Dictionary<InputMap, InputBinding> GetDictType(PlayerType type)
        {
            switch (type)
            {
                case PlayerType.P1: return InputMapDictP1;
                case PlayerType.P2: return InputMapDictP2;
                default: GRDebug.LogErrorEnumNotImplement(type); return null;
            }
        }
        
        /************************************************************************************************************************/
        // Singleton
        /************************************************************************************************************************/

        private static GlobalInputMapping Singleton = null;
        
        /************************************************************************************************************************/
        
        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Singleton = this;
            InputMapDictP1 = InputMapDictP1;
        }

        /************************************************************************************************************************/
        
        public static bool IsDown(InputMap action, PlayerType type)
        {
            var binding = Singleton.GetDictType(type)[action];
            return Input.GetKeyDown(binding.Keyboard) ||
                   Input.GetKeyDown(binding.Gamepad);
        }

        public static bool IsUp(InputMap action, PlayerType type)
        {
            var binding = Singleton.GetDictType(type)[action];
            return Input.GetKeyUp(binding.Keyboard) ||
                   Input.GetKeyUp(binding.Gamepad);
        }

        public static bool IsKey(InputMap action, PlayerType type)
        {
            var binding = Singleton.GetDictType(type)[action];
            return Input.GetKey(binding.Keyboard) ||
                   Input.GetKey(binding.Gamepad);
        }

        /************************************************************************************************************************/
    }
}