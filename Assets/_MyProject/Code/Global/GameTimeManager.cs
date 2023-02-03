using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoldenRoot
{
    public delegate void Callback();
    
    public class GameTimeManager : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private float _TotalCountdownSeconds = 60f;
        /************************************************************************************************************************/

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
                if (countdown == 0f)
                {
                    if (OnTimerEnd != null)
                        OnTimerEnd?.Invoke();

                    break;
                }    
                yield return null;
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