using System.Collections;
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
        [SerializeField] private GameObject _GameTimeCanvas;

        public static GameTimeManager Singleton;
        private Coroutine _RunningCoroutine;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            } else
            {
                Destroy(this.gameObject);
            }
        }

        public event Callback OnTimerEnd;

        public void SetCanvasActive(bool value)
        {
            this._GameTimeCanvas.SetActive(value);
        }

        public void StartCountdown()
        {
            if (this._RunningCoroutine != null)
            {
                StopCoroutine(this._RunningCoroutine);
            }

            this._RunningCoroutine = StartCoroutine(CorouCountdown(_TotalCountdownSeconds));
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