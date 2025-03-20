using UnityEngine;
using Unity.Netcode;
using UnityEngine.Animations.Rigging;
using System;
using Unity.VisualScripting;

namespace NoNameGun.Players
{
    public class PlayerAnimation : NetworkBehaviour
    {
        [Header("IK Setting")]
        public Rig RightHandRig;
        public Rig LeftHandRig;
        //public Transform RightHandTarget;
        public Transform LeftHandTarget;

        [Header("Gun Hands")]
        public Transform GunRightHandleTrm;
        public Transform GunLeftHandleTrm;

        #region PRIVATE_VARIABLE
        private Animator _animator;
        private Player _player;

        private float angle;
        private bool isNull = true;
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
            //HandUpdate();
            NeckAngleUpdate();
        }
        #endregion

        #region  MAIN_FUNC
        private void NeckAngleUpdate()
        {
            _animator.SetFloat("Angle", _player.PlayerCamera.CameraAngleCalcValue);
        }

        // private void HandUpdate()
        // {
        //     if (GunRightHandleTrm == null)
        //     {
        //         // 💡 계속해서 갱신 시도 (한 번 null이어도 계속 확인)
        //         GunRightHandleTrm = _player.PlayerShoot?.currentGun?.transform.Find(Define.GunIKHandle.RightHandIK);

        //         if (GunRightHandleTrm == null)
        //         {
        //             Debug.LogWarning("[PlayerAnimation] GunRightHandleTrm이 아직 설정되지 않았습니다.");
        //             return;
        //         }
        //         else
        //         {
        //             Debug.Log("[PlayerAnimation] GunRightHandleTrm이 정상적으로 할당되었습니다.");
        //         }
        //     }

        //     //RightHandTarget.position = GunRightHandleTrm.position;
        //     //RightHandTarget.rotation = GunRightHandleTrm.rotation;

        //     //SetPlayerHandTargetTrmServerRpc();
        // }

        private void UpdateMoveAnimation(Vector2 inputDir)
        {
            _animator.SetFloat("MoveX", inputDir.x);
            _animator.SetFloat("MoveY", inputDir.y);
        }
        #endregion

        #region RPC_FUNC
        // [ServerRpc]
        // private void SetPlayerHandTargetTrmServerRpc()
        // {
        //     RightHandTarget.position = GunRightHandleTrm.position;
        //     RightHandTarget.rotation = GunRightHandleTrm.rotation;
        // }

        // private void UpdateHandIKWeight(float weight)
        // {
        //     RightHandRig.weight = weight;
        //     LeftHandRig.weight = weight;
        // }
        #endregion
    }
}
