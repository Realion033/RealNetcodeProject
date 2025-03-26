using System;
using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Players
{
    public class PlayerShooter : NetworkBehaviour
    {
        [SerializeField] private Transform _gunPivotPos;
        public GameObject CurrentGun;
        public event Action<Transform> IsGunSpawnEvt;

        #region PRIVATE_VARIABLE
        private Transform _rightHandPos;
        private Transform _spawnedGunTrm;
        private Player _player;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            GunSpawn();
        }


        void Update()
        {
            //SyncGunPos(_spawnedGun.transform);
        }
        #endregion

        #region MAIN_FUNC
        private void GunSpawn()
        {
            // 총 인스턴스 생성
            GameObject currentGun = Instantiate(CurrentGun, _gunPivotPos.position, _gunPivotPos.rotation);
            NetworkObject gunNetworkObject = currentGun.GetComponent<NetworkObject>();

            if (gunNetworkObject != null)
            {
                gunNetworkObject.Spawn();
            }

            // ✅ 네트워크에서 자신이 소유한 오브젝트일 경우만 부모 설정
            if (IsOwner)
            {
                currentGun.transform.SetParent(_gunPivotPos);
            }

            _spawnedGunTrm = currentGun.transform;
        }


        private void SyncGunPos(Transform gunPos)
        {
            //gunPos.position = _gunPivotPos.transform.position;
        }
        #endregion

        // #region RPC_FUNC
        // [ServerRpc]
        // private void GunSpawnServerRPC()
        // {
        //     GameObject currentGun = Instantiate(CurrentGun, _gunPivotPos.position, Quaternion.identity, _gunPivotPos);
        //     _spawnedGunTrm = currentGun.transform;
        //     IsGunSpawnEvt?.Invoke(_spawnedGunTrm);
        // }
        // #endregion
    }
}
