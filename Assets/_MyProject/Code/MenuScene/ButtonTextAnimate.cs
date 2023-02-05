using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GoldenRoot.MenuScene
{
    public class ButtonTextAnimate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TMP_Text _ButtonText;

        [SerializeField] private Color _PressedColor = Color.gray;
        
        private Tween tween;

        private Color NormalColor { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            NormalColor = _ButtonText.color;
            _ButtonText.transform.DOScale(1.1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            _ButtonText.transform.DOScale(1f, 0.1f);
            _ButtonText.color = NormalColor;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _ButtonText.color = _PressedColor;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _ButtonText.color = NormalColor;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            _ButtonText = gameObject.GetComponentInChildren<TMP_Text>();
        }
        #endif
    }
}