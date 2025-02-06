using System;
using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Weaopon
{
    public class TestGun : NetworkBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;
        [SerializeField] private float fireRate = 0.5f; // 발사 간격 (초)
        private float lastFireTime = 0f; // 마지막 발사 시간

        public GameObject Bullet;
        public Transform FirePos;

        private void Awake()
        {
            _playerInput.AttackEvt += Fire;
        }

        private void OnDisable()
        {
            _playerInput.AttackEvt -= Fire;
        }

        private void Fire()
        {
            // 현재 시간과 마지막 발사 시간의 차이가 발사 간격보다 클 때만 발사
            if (IsOwner && Time.time - lastFireTime >= fireRate)
            {
                FireBulletServerRpc();  // 서버에서 총알 발사 처리
                lastFireTime = Time.time; // 마지막 발사 시간 갱신
            }
        }

        // ServerRpc: 서버에서 총알을 생성하고 스폰하도록 하는 메서드
        [ServerRpc]
        private void FireBulletServerRpc()
        {
            GameObject bullet = Instantiate(Bullet, FirePos.position, FirePos.rotation);
            bullet.GetComponent<NetworkObject>().Spawn();  // 네트워크 오브젝트로 스폰
        }

        private void Update()
        {
            Debug.Log($"FirePos 위치: {FirePos.position}, 회전: {FirePos.rotation}");
        }
    }
}
