using System;
using UnityEngine;

namespace GoldenRoot.MenuScene
{
    public class MenuSelectionResult : MonoBehaviour
    {
        public int P1SelectionIndex = -1;
        public int P2SelectionIndex = -2;

        public static MenuSelectionResult Singleton;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(this);
        }
    }
}