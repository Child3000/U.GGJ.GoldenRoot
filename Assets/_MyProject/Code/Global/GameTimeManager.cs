using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GoldenRoot
{
    public delegate void Callback();
    
    public class GameTimeManager : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private float _TotalCountdownSeconds = 60f;
        /************************************************************************************************************************/
        [SerializeField] private TMP_Text _TimerText;
        /************************************************************************************************************************/
        
        public enum AutoRunOption
        {
            /// <summary>
            /// Do not start countdown automatically.
            /// </summary>
            None,
            
            /// <summary>
            /// Start countdown in Unity Awake() callback.
            /// </summary>
            Awake,
            
            /// <summary>
            /// Start countdown in Unity Start() callback.
            /// </summary>
            Start
        }

        [SerializeField] private AutoRunOption _AutoRunOption = AutoRunOption.None;
        

        private void Awake()
        {
            if (_AutoRunOption == AutoRunOption.Awake)
            {
                StartCountdown();
            }
        }

        private void Start()
        {
            if (_AutoRunOption == AutoRunOption.Start)
            {
                StartCountdown();
            }
        }

        public event Callback OnTimerEnd;

        public void StartCountdown()
        {
            StartCoroutine(CorouCountdown(_TotalCountdownSeconds));
        }
        
        private IEnumerator CorouCountdown(float t)
        {
            float countdown = t;
            
            while (true)
            {
                countdown = GRUtility.AtLeast(countdown - Time.deltaTime, 0f);
                UpdateTimerUi(countdown);
                if (countdown == 0f)
                {
                    if (OnTimerEnd != null)
                        OnTimerEnd?.Invoke();

                    break;
                }    
                yield return null;
            }
        }

        private void UpdateTimerUi(float t)
        {
            if (_TimerText)
            {
                _TimerText.text = Mathf.CeilToInt(t).ToString();
            }
        }

        /************************************************************************************************************************/
        #if UNITY_EDITOR
        private void OnValidate()
        {
            _TotalCountdownSeconds = GRUtility.AtLeast(_TotalCountdownSeconds, 0f);
        }
        #endif
        /************************************************************************************************************************/
    }
}