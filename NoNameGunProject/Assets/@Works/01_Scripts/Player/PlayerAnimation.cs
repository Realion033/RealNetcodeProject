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

        #region PRIVATE_VARIABLE
        private Animator _animator;
        private Player _player;

        private float angle;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponentInParent<Player>();

            _player.PlayerMovement.OnMovement += UpdateMoveAnimation;
        }

        private void OnDisable()
        {
            _player.PlayerMovement.OnMovement -= UpdateMoveAnimation;
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandUpdate();
            NeckAngleUpdate();
        }
        #endregion

        #region  MAIN_FUNC
        private void NeckAngleUpdate()
        {
            Debug.Log(_player.PlayerCamera.CameraAngleCalcValue);
            _animator.SetFloat("Angle", _player.PlayerCamera.CameraAngleCalcValue);
        }

        private void HandUpdate()
        {
            RightHandTarget.position = GunRightHandleTrm.position;
            RightHandTarget.rotation = GunRightHandleTrm.rotation;

            SetPlayerHandTargetTrmServerRpc();
        }

        private void UpdateMoveAnimation(Vector2 inputDir)
        {
            _animator.SetFloat("MoveX", inputDir.x);
            _animator.SetFloat("MoveY", inputDir.y);
        }
        #endregion

        #region RPC_FUNC
        [ServerRpc]
        private void SetPlayerHandTargetTrmServerRpc()
        {
            RightHandTarget.position = GunRightHandleTrm.position;
            RightHandTarget.rotation = GunRightHandleTrm.rotation;
        }

        private void UpdateHandIKWeight(float weight)
        {
            RightHandRig.weight = weight;
            LeftHandRig.weight = weight;
        }
        #endregion
    }
}
