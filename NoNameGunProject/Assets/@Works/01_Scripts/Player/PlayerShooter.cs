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
            GameObject currentGun = Instantiate(CurrentGun, _gunPivotPos.position, Quaternion.identity, _gunPivotPos);
            _spawnedGunTrm = currentGun.transform;
            IsGunSpawnEvt?.Invoke(_spawnedGunTrm);

            GunSpawnServerRPC();
        }

        void Update()
        {
            //SyncGunPos(_spawnedGun.transform);
        }
        #endregion

        #region MAIN_FUNC
        private void SyncGunPos(Transform gunPos)
        {
            //gunPos.position = _gunPivotPos.transform.position;
        }
        #endregion

        #region RPC_FUNC
        [ServerRpc]
        private void GunSpawnServerRPC()
        {
            GameObject currentGun = Instantiate(CurrentGun, _gunPivotPos.position, Quaternion.identity, _gunPivotPos);
            _spawnedGunTrm = currentGun.transform;
            IsGunSpawnEvt?.Invoke(_spawnedGunTrm);
        }
        #endregion
    }
}
