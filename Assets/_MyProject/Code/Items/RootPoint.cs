using UnityEngine;

namespace GoldenRoot
{
    public class RootPoint : MonoBehaviour
    {
        [SerializeField] private float _Value;

        public float Value => _Value;
    }
}