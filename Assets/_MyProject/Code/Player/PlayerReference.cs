using UnityEngine;

namespace GoldenRoot
{
    public class PlayerReference : MonoBehaviour
    {
        public enum PlayerID
        {
            P1, 
            P2
        };
        
        [SerializeField] private PlayerID _Type;
        public PlayerID Type => _Type;

        [SerializeField] private PlayerInput _Input;
        public PlayerInput Input => _Input;

        [SerializeField] private PlayerMovement _Movement;
        public PlayerMovement Movement => _Movement;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Input);
            gameObject.GetComponentInParentOrChildren(ref _Movement);
        }
#endif
    }
}