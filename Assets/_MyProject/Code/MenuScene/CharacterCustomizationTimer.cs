using System;
using System.Collections;
using System.Linq;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldenRoot.MenuScene
{
    public class CharacterCustomizationTimer : MonoBehaviour
    {
        public event Callback OnTimerEnd;
        
        [SerializeField] private TMP_Text _TimerText;

        [SerializeField] private float _SelectionTimeInSeconds;

        [SerializeField] private SceneReference _MainScene;

        [SerializeField] private CharacterCustomizationPlayerSelectionArea[] _SelectionArea;
        
        private float _CountdownTime;

        private void Awake()
        {
            _CountdownTime = _SelectionTimeInSeconds;
            StartCoroutine(CorouAwake());
        }

        private IEnumerator CorouAwake()
        {
            bool timeEndWithNormalSelection = false;
            
            while (true)
            {
                _TimerText.text = $"{Mathf.CeilToInt(_CountdownTime) / 60}:{(Mathf.CeilToInt(_CountdownTime) % 60):00}";
                _CountdownTime = Mathf.Clamp(_CountdownTime - Time.deltaTime, 0, _SelectionTimeInSeconds);
                
                if (_CountdownTime == 0f)
                {
                    OnTimerEnd?.Invoke();
                    break;
                }
                else if (IsAllSelectionReady())
                {
                    OnTimerEnd?.Invoke();
                    timeEndWithNormalSelection = true;
                    break;
                }
                
                yield return null;
            }
            
            // Grave period of 3 seconds to transite to Main scene, if doesn't end timer in normal pace.
            _CountdownTime = Mathf.Min(_CountdownTime, 3f);
            while (true)
            {
                _TimerText.text = $"<color=red>{Mathf.CeilToInt(_CountdownTime) / 60}:{(Mathf.CeilToInt(_CountdownTime) % 60):00}</color>";
                _CountdownTime = Mathf.Clamp(_CountdownTime - Time.deltaTime, 0, _SelectionTimeInSeconds);

                if (_CountdownTime == 0f)
                {
                    break;
                }
                yield return null;
            }


            SceneManager.LoadSceneAsync(_MainScene.Name);
        }

        private bool IsAllSelectionReady()
        {
            bool isAllReady = _SelectionArea.All(x => x.IsReady);
            return isAllReady;
        }
    }
}