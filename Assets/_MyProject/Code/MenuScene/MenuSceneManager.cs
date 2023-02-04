using System;
using System.Collections;
using Eflatun.SceneReference;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldenRoot.MenuScene
{
    public class MenuSceneManager : MonoBehaviour
    {
        [SerializeField] private SceneReference _MenuScene;

        private Action<MenuSelectionResult> _ResultCallback;
        
        // @Sample code to test its functionality
        //private IEnumerator Start()
        //{
        //    yield return null;
        //    
        //    Camera cam = Camera.main;
        //    cam.gameObject.SetActive(false);
        //    StartMenuSceneInAdditiveMode(SceneManager.GetActiveScene(), (s) =>
        //    {
        //        cam.gameObject.SetActive(true);
        //        Debug.Log("Get backs the result");
        //    });
        //}

        public void StartMenuSceneInAdditiveMode(Scene fromScene, Action<MenuSelectionResult> resultCallback)
        {
            _ResultCallback = resultCallback;
            StartCoroutine(CorouLoadAdditive(_MenuScene));
        }

        private IEnumerator CorouLoadAdditive(SceneReference scene)
        {
            // Ignore if the scene is loaded.
            var isLoaded = scene.LoadedScene.IsValid();
            if (isLoaded) yield break;
            
            // Start loading scene additively.
            var op = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
            yield return op;

            // this script didn't handle the responsibility for the scene to carry out its task, but we keen to know when scene finished all operations.
            CharacterCustomizationTimer.OnSceneEnd += OnMenuSceneEnded;
        }

        private void OnMenuSceneEnded(MenuSelectionResult resultInstance)
        {
            CharacterCustomizationTimer.OnSceneEnd -= OnMenuSceneEnded;

            SceneManager.UnloadSceneAsync(_MenuScene.Name);
            
            if (_ResultCallback != null)
                _ResultCallback.Invoke(resultInstance);
        }
    }
}