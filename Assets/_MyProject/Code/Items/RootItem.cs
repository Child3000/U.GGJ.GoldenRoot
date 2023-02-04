using UnityEngine;

namespace GoldenRoot
{
    [System.Serializable]
    public struct RootItem
    {
        [SerializeField] private int _Probablity;
        [SerializeField] private int _Point;
        [SerializeField] private GameObject _RootPrefab;

        public int Point => _Point;

        /************************************************************************************************************************/
        #if UNITY_EDITOR
        public void OnValidate()
        {
            _Point = GRUtility.AtLeast(_Point, 0);
        }
        #endif
        /************************************************************************************************************************/
    }
}