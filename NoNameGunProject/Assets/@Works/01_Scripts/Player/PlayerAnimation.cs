using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations.Rigging;
using System;
using System.Collections.Generic;

namespace NoNameGun.Players
{
    public enum AnimType
    {
        Int,
        Float,
        Bool,
        Trigger
    }

    // 파라미터 구조체
    [System.Serializable]
    public struct AnimParam
    {
        public string name;
        public AnimType type;

        public int Hash => Animator.StringToHash(name);
    }

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

        [Header("Param Setting")]
        public List<AnimParam> AnimParams;
        private Dictionary<string, int> _animParamDic;

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
            _player.PlayerInput.JumpEvt += HandleJumpEvt;
            _player.PlayerShoot.IsGunSpawnEvt += HandleGunFind;

            _animParamDic = new Dictionary<string, int>();  // 딕셔 선언
            foreach (var param in AnimParams)
            {
                _animParamDic[param.name] = param.Hash; // 알잘딲 포이치
            }
        }

        private void OnDisable()
        {
            _player.PlayerMovement.OnMovement -= UpdateMoveAnimation;
            _player.PlayerInput.JumpEvt += HandleJumpEvt;
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
        public int GetParamHash(string paramName)
        {
            return _animParamDic[paramName];    // 원래 리스트에서 직접뽑았는데 최적화 차원에서 딕셔너리사용
        }

        private void NeckAngleUpdate()
        {
            _animator.SetFloat(GetParamHash("Angle"), _player.PlayerCamera.CameraAngleCalcValue);
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
            _animator.SetFloat(GetParamHash("MoveX"), inputDir.x);
            _animator.SetFloat(GetParamHash("MoveY"), inputDir.y);
        }

        private void HandleGunFind(Transform transform)
        {
            GunRightHandleTrm = transform.Find(Define.GunIKHandle.RightHandIK);
        }

        private void HandleJumpEvt()
        {
            if (_player.PlayerMovement.IsGrounded == true)
            {
                _animator.SetTrigger(GetParamHash("OnJump"));
            }
        }
        #endregion
    }
}
