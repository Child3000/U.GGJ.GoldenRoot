using System;
using UnityEngine;

namespace GoldenRoot
{
    public class RootItem : MonoBehaviour
    {
        [SerializeField] private int _Point;

        public int Point => _Point;

        /************************************************************************************************************************/
        #if UNITY_EDITOR
        private void OnValidate()
        {
            _Point = GRUtility.AtLeast(_Point, 0);
        }
        #endif
        /************************************************************************************************************************/
    }
}