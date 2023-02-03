using System;
using System.Collections;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldenRoot
{
    public class LoadSceneAdditive : MonoBehaviour
    {
        [SerializeField] private SceneReference[] _ScenesToLoad;

        [SerializeField] private LoadMode _LoadMode = LoadMode.Sequential;
        
        public enum LoadMode
        {
            Sequential,
            Asynchronous
        };
        
        private void Awake()
        {
            StartCoroutine(CorouAwake());
        }

        private IEnumerator CorouAwake()
        {
            foreach (var scene in _ScenesToLoad)
            {
                var isLoaded = scene.LoadedScene.IsValid();
                if (isLoaded) continue;
                
                var op = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
                if (_LoadMode == LoadMode.Sequential) yield return op;
            }
        }
    }
}