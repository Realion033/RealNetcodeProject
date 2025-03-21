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
        private bool isFind = false;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponentInParent<Player>();

            _player.PlayerMovement.OnMovement += UpdateMoveAnimation;
            _player.PlayerShoot.IsGunSpawnEvt += HandleGunFind;
        }

        private void OnDisable()
        {
            _player.PlayerMovement.OnMovement -= UpdateMoveAnimation;
            _player.PlayerShoot.IsGunSpawnEvt -= HandleGunFind;
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
            _animator.SetFloat("Angle", _player.PlayerCamera.CameraAngleCalcValue);
        }

        private void HandUpdate()
        {
            if (GunRightHandleTrm == null)
            {
                return;
            }

            RightHandTarget.position = GunRightHandleTrm.position;
            RightHandTarget.rotation = GunRightHandleTrm.rotation;

            //SetPlayerHandTargetTrmServerRpc();
        }

        #endregion

        #region EVT_FUNC
        private void UpdateMoveAnimation(Vector2 inputDir)
        {
            _animator.SetFloat("MoveX", inputDir.x);
            _animator.SetFloat("MoveY", inputDir.y);
        }

        private void HandleGunFind(Transform transform)
        {
            GunRightHandleTrm = transform.Find(Define.GunIKHandle.RightHandIK);
        }
        #endregion
    }
}
