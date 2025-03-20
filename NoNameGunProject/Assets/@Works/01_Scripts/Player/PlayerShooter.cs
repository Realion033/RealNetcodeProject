using System;
using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Players
{
    public class PlayerShooter : NetworkBehaviour
    {
        [SerializeField] private Transform _gunPivotPos;
        public GameObject currentGun;

        #region PRIVATE_VARIABLE
        private Transform _rightHandPos;
        private Player _player;
        private GameObject _spawnedGun;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        // public void Start()
        // {
        //     if (!IsOwner) return;

        //     // 총기 생성 및 부모 설정
        //     _spawnedGun = Instantiate(currentGun, _gunPivotPos.position, Quaternion.identity, _gunPivotPos);

        //     // 오른손 위치 설정
        //     _rightHandPos = _spawnedGun.transform.Find(Define.GunIKHandle.RightHandIK);
        //     if (_rightHandPos == null)
        //     {
        //         Debug.LogError("[PlayerShooter] RightHandIK 위치를 찾을 수 없습니다.");
        //         return;
        //     }

        //     _player.PlayerAnim.GunRightHandleTrm = _rightHandPos;

        //     // 모든 클라이언트에게 총 위치 동기화
        //     //UpdateGunPositionServerRpc(_gunPivotPos.position, _gunPivotPos.rotation);
        // }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            // 총의 위치 및 회전 동기화
            //UpdateGunPositionServerRpc(_gunPivotPos.position, _gunPivotPos.rotation);
        }
        #endregion

        #region RPC_FUNC
        // [ServerRpc]
        // private void UpdateGunPositionServerRpc(Vector3 position, Quaternion rotation)
        // {
        //     if (IsOwner) return; // 호스트가 자기 총을 조정하는 것은 불필요하므로 무시

        //     if (_spawnedGun == null)
        //     {
        //         // 클라이언트가 총을 가지고 있지 않다면 생성
        //         _spawnedGun = Instantiate(currentGun, position, rotation, _gunPivotPos);
        //     }
        //     else
        //     {
        //         // 위치 및 회전 동기화
        //         _spawnedGun.transform.position = position;
        //         _spawnedGun.transform.rotation = rotation;
        //     }
        // }
        #endregion
    }
}
