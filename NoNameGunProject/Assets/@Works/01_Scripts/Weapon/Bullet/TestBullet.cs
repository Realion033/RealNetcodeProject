using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace NoNameGun
{
    public class TestBullet : NetworkBehaviour
    {
        [SerializeField] private float _power = 30f;
        [SerializeField] private float _lifeTime = 2f; // 총알의 생명 시간 (초)
        private Rigidbody _rbCompo;

        private void Awake()
        {
            _rbCompo = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (!IsOwner)
            {
                return;
            }

            SetFireServerRpc();

            StartCoroutine(DestroyBullet());
        }

        [ServerRpc]
        private void SetFireServerRpc()
        {
            Vector3 fireDirection = transform.forward; // 총구의 전방 방향
            _rbCompo.AddForce(fireDirection * _power, ForceMode.Impulse);
        }

        // 2초가 지나면 총알 삭제
        IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(2.3f);

            if (IsOwner)
            {
                NetworkObject.Despawn();  // 네트워크 상에서 총알 삭제
                Destroy(gameObject); // 로컬에서 총알 삭제
            }
        }

        // 충돌 시 총알 삭제
        private void OnCollisionEnter(Collision collision)
        {
            if (IsOwner)
            {
                NetworkObject.Despawn();  // 네트워크 상에서 총알 삭제
                Destroy(gameObject); // 로컬에서 총알 삭제
            }

            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            
            if (damagable != null)
            {
                damagable.ApplyDamage(10); // 피해를 적용
            }
        }
    }
}
