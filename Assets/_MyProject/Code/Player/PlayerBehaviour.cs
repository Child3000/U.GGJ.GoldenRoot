using System;
using UnityEngine;
using Unity.Mathematics;

namespace GoldenRoot
{
    public class PlayerBehaviour : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private PlayerReference _PlayerReference;
        private PlayerInput PlayerInput => _PlayerReference.Input;
        /************************************************************************************************************************/
        [SerializeField] private GridMap2D _GridMap2D;
        
        /************************************************************************************************************************/
        // Stun Players
        /************************************************************************************************************************/
        
        [SerializeField] private float _StunAttackRange = 0.5f;

        [SerializeField] private float _StunOriginForwardOffset = 1f;

        [SerializeField] private float _StunOriginHeightOffset = 1f;

        [SerializeField] private LayerMask _StunLayerMask;

        [SerializeField] private QueryTriggerInteraction _StunQueryTriggerInteraction = QueryTriggerInteraction.Collide;

        private Collider[] _TempStunResults = new Collider[MaxStunOverlapQueryAmount];

        private const int MaxStunOverlapQueryAmount = 100;
        
        public Vector3 StunOverlapOrigin => transform.position + Vector3.up * _StunOriginHeightOffset + _PlayerReference.Movement.FaceDirection * _StunOriginForwardOffset;
        public float StunAttackRange => _StunAttackRange;

        /************************************************************************************************************************/
        // Stun Cooldown
        /************************************************************************************************************************/

        public float StunActionRegenerateRatioProgress => AppTimeWhenUsedStunActionOnValidTarget.HasValue ? 
                        Mathf.Clamp01((float) (Time.timeAsDouble - AppTimeWhenUsedStunActionOnValidTarget) / (float) (StunActionCooldownInSeconds)) : 
                        0f;
        
        private float StunActionCooldownInSeconds => 10f;
        private double? AppTimeWhenUsedStunActionOnValidTarget { get; set; }

        public bool IsStunActionInCooldown => AppTimeWhenUsedStunActionOnValidTarget.HasValue && Time.timeAsDouble < AppTimeWhenUsedStunActionOnValidTarget + StunActionCooldownInSeconds;
        
        /************************************************************************************************************************/
        // Stunnable
        /************************************************************************************************************************/

        public float StunnedDegenerateRatioProgress => AppTimeWhenStunned.HasValue ? 
                    Mathf.Clamp01((float) (Time.timeAsDouble - AppTimeWhenStunned) / (float) (StunPeriodInSeconds)) : 
                    0f;
        
        public bool IsStunned => AppTimeWhenStunned.HasValue && Time.timeAsDouble < AppTimeWhenStunned + StunPeriodInSeconds;
        
        private double? AppTimeWhenStunned { get; set; }
        private double StunPeriodInSeconds => 2f;

        public void Stunned()
        {
            GRDebug.Log($"{gameObject.name} is stunned!");
            AppTimeWhenStunned = Time.timeAsDouble;
        }
        
        /************************************************************************************************************************/
        
        private void Update()
        {
            if (IsStunned) return;
            
            if (PlayerInput.IsDig)
            {
                float3 position = this.transform.position;
                int3 gridIdx = (int3)position;

                this._GridMap2D.DigTile(gridIdx.x, gridIdx.z, 1, this._PlayerReference.Type);
            }

            if (PlayerInput.IsAttack && !IsStunActionInCooldown)
            {
                // Try to detect nearby player and stun them.
                var size = Physics.OverlapSphereNonAlloc(StunOverlapOrigin, StunAttackRange, _TempStunResults, _StunLayerMask, _StunQueryTriggerInteraction);
                for (int i = 0; i < size; i++)
                {
                    if (_TempStunResults[i].gameObject != gameObject &&
                        _TempStunResults[i].TryGetComponent<PlayerBehaviour>(out var component))
                    {
                        component.Stunned();

                        AppTimeWhenUsedStunActionOnValidTarget = Time.timeAsDouble;
                    }
                }
                
                Debug.Log("IsAttack");
            }
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            _StunAttackRange = GRUtility.AtLeast(_StunAttackRange, 0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(StunOverlapOrigin, _StunAttackRange);
        }
        #endif
    }
}
