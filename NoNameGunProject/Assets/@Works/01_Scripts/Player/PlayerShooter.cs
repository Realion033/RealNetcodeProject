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
        private NetworkObject _gunNetworkObject;
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

            GameObject currentGun = Instantiate(CurrentGun, _gunPivotPos.position, _gunPivotPos.rotation);
            _gunNetworkObject = currentGun.GetComponent<NetworkObject>();

            _spawnedGunTrm = currentGun.transform;
            IsGunSpawnEvt?.Invoke(_spawnedGunTrm);
        }
        
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            GunPosSync();
        }
        #endregion

        #region MAIN_FUNC
        private void GunPosSync()
        {
            _gunNetworkObject.transform.localPosition = _gunPivotPos.position;
            _gunNetworkObject.transform.localRotation = _gunPivotPos.rotation;
        }
        #endregion

        #region RPC_FUNC
        [ServerRpc]
        private void GunSetParentServerRPC()
        {
            GameObject currentGun = Instantiate(CurrentGun);
            NetworkObject gunNetworkObject = currentGun.GetComponent<NetworkObject>();
            gunNetworkObject.TrySetParent(_gunPivotPos);

            _spawnedGunTrm = currentGun.transform;
            IsGunSpawnEvt?.Invoke(_spawnedGunTrm);
        }
        #endregion
    }
}