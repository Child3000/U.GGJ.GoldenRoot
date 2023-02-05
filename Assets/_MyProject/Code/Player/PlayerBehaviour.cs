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
        [SerializeField] private Transform _ShovelTrans;
        [SerializeField] private float _ShovelRotateAmount;
        [SerializeField] private float _ShovelRevertSpeed;
        [SerializeField] private Transform _DigTarget;
        [SerializeField] private GridMap2D _GridMap2D;
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private AudioClip[] digSFXs;
        [SerializeField] private AudioClip[] swingSFXs;
        [SerializeField] private float sfxVolume = 0.7f;

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

        private Vector3 _ShovelOriginLocalPosition;
        
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
        private void Start()
        {
            this._ShovelOriginLocalPosition = this._ShovelTrans.localPosition;
        }

        private void Update()
        {
            if (IsStunned) return;

            // revert shovel to origin rotation
            this._ShovelTrans.localRotation = Quaternion.Slerp(
                this._ShovelTrans.localRotation, Quaternion.identity, Time.deltaTime * this._ShovelRevertSpeed
            );
            // revert shovel to origin position
            this._ShovelTrans.localPosition = Vector3.Lerp(
                this._ShovelTrans.localPosition, this._ShovelOriginLocalPosition, Time.deltaTime * this._ShovelRevertSpeed
            );

            if (PlayerInput.IsDig)
            {
                float3 position = this._DigTarget.position;
                int3 gridIdx = (int3)position;

                this._GridMap2D.DigTile(gridIdx.x, gridIdx.z, 1, this._PlayerReference.Type);

                // rotate shovel
                this._ShovelTrans.localRotation = Quaternion.Euler(this._ShovelRotateAmount, 0.0f, 0.0f);

                // play dig sound
                int clipToPlay = UnityEngine.Random.Range(0, digSFXs.Length);
                _AudioSource.PlayOneShot(digSFXs[clipToPlay], sfxVolume);
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

                // poke shovel
                Vector3 shovelTargetLocalPosition = this._ShovelOriginLocalPosition;
                shovelTargetLocalPosition.z += this._StunAttackRange;
                this._ShovelTrans.localPosition = shovelTargetLocalPosition;
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
