using System;
using System.Collections;
using UnityEngine;

namespace GoldenRoot
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        /************************************************************************************************************************/
        [SerializeField] private PlayerReference _PlayerReference;
        private PlayerInput PlayerInput => _PlayerReference.Input;
        private PlayerBehaviour PlayerBehaviour => _PlayerReference.Behaviour;
        /************************************************************************************************************************/
        [SerializeField] private CharacterController _CharController;
        /************************************************************************************************************************/
        [SerializeField] private float _MoveSpeed = 5f;
        [SerializeField] private float _RotateSpeed = 130f;
        /************************************************************************************************************************/

        /************************************************************************************************************************/
        // Dig, Swing Hit Sound Efffects
        /************************************************************************************************************************/
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private AudioClip[] walkSFXs;
        [SerializeField] private float sfxVolume = 0.5f;
        [SerializeField] private float walkInterval = 1f;
        /************************************************************************************************************************/

        private float accumWalkTime = 0f;


        public Vector3 FaceDirection
        {
            get => transform.forward;
            set => transform.forward = value;
        }

        private Vector3 TargetFaceDirection { get; set; }

        private void Update()
        {
            if (PlayerBehaviour.IsStunned) return;
            if (GameManager.Singleton.GameState != GameState.Playing) return;
            
            // translation
            if (PlayerInput.MoveAxis != Vector2.zero)
            {
                Vector3 moveDirection = new Vector3(PlayerInput.MoveAxis.x, 0, PlayerInput.MoveAxis.y);
                Vector3 motion =  moveDirection * (_MoveSpeed * Time.deltaTime);
                _CharController.Move(motion);

                TargetFaceDirection = moveDirection.normalized;

                accumWalkTime += Time.fixedDeltaTime;
                if (accumWalkTime > walkInterval)
                {
                    accumWalkTime = 0;
                    int clipToPlay = UnityEngine.Random.Range(0, walkSFXs.Length);
                    _AudioSource.PlayOneShot(walkSFXs[clipToPlay], sfxVolume);
                }
            }

            Vector3 position = transform.position;
            position.y = 0.0f;
            transform.position = position;

            // rotation
            if (TargetFaceDirection != Vector3.zero)
            {
                FaceDirection = Vector3.RotateTowards(FaceDirection, TargetFaceDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            }
        }


        /************************************************************************************************************************/
#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _CharController);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + FaceDirection * 2f);
        }
        #endif
        /************************************************************************************************************************/
    }
}