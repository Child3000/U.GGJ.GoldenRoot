using System;
using UnityEngine;
using UnityEngine.UI;

namespace GoldenRoot
{
    public class PlayerUi : WorldCanvasFaceCamera
    {
        [SerializeField] private PlayerReference _PlayerReference;
        
        [SerializeField] private Image _AttackCooldownImg;
        [SerializeField] private Image _AttackCooldownImgBack;
        [SerializeField] private Image _StunnedCooldownImg;
        [SerializeField] private Image _StunnedCooldownImgBack;

        protected override void Update()
        {
            base.Update();
            _AttackCooldownImgBack.gameObject.SetActive(_PlayerReference.Behaviour.IsStunActionInCooldown);
            _AttackCooldownImg.gameObject.SetActive(_PlayerReference.Behaviour.IsStunActionInCooldown);
            _AttackCooldownImg.fillAmount = 1.0f - _PlayerReference.Behaviour.StunActionRegenerateRatioProgress;
            
            _StunnedCooldownImg.gameObject.SetActive(_PlayerReference.Behaviour.IsStunned);
            _StunnedCooldownImgBack.gameObject.SetActive(_PlayerReference.Behaviour.IsStunned);
            _StunnedCooldownImg.fillAmount = 1.0f -_PlayerReference.Behaviour.StunnedDegenerateRatioProgress;
        }

        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _PlayerReference);
        }
    }
}