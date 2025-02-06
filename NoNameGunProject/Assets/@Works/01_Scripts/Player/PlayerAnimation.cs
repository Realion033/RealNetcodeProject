using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations.Rigging;
using System;

namespace NoNameGun.Players
{
    public class PlayerAnimation : NetworkBehaviour
    {
        [Header("IK Setting")]
        public Rig RightHandRig;
        public Rig LeftHandRig;
        public Transform RightHandTarget;
        public Transform LeftHandTarget;

        [Header("Gun Hands")]
        public Transform GunRightHandleTrm;
        public Transform GunLeftHandleTrm;

        private Animator _animator;
        private Player _player;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponentInParent<Player>();

            // 입력 값 변경 이벤트 구독
            _player.PlayerMovement.OnMovement += UpdateMoveAnimation;
        }

        private void OnDisable()
        {
            _player.PlayerMovement.OnMovement -= UpdateMoveAnimation;
        }

        private void Update()
        {
            SetPlayerHandTargetTrm();
        }

        private void SetPlayerHandTargetTrm()
        {
            RightHandTarget.position = GunRightHandleTrm.position;
            RightHandTarget.rotation = GunRightHandleTrm.rotation;

            LeftHandTarget.position = GunLeftHandleTrm.position;
            LeftHandTarget.rotation = GunLeftHandleTrm.rotation;
        }

        public void UpdateHandIKAnimation(float weight, bool isRight)
        {
            if (isRight)
            {
                UpdateRightHandIKAnimationServerRpc(weight);
            }
            else
            {
                UpdateLeftHandIKAnimationServerRpc(weight);
            }
        }

        private void UpdateMoveAnimation(Vector2 inputDir)
        {
            if (!IsOwner)
            {
                return;
            }

            UpdateMoveAnimationServerRpc(inputDir);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateMoveAnimationServerRpc(Vector2 inputDir)
        {
            _animator.SetFloat("MoveX", inputDir.x);
            _animator.SetFloat("MoveY", inputDir.y);
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateRightHandIKAnimationServerRpc(float _weight)
        {
            RightHandRig.weight = _weight;
        }

        [ServerRpc(RequireOwnership = false)]
        private void UpdateLeftHandIKAnimationServerRpc(float _weight)
        {
            LeftHandRig.weight = _weight;
        }
    }
}
