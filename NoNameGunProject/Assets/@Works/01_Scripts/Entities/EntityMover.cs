using System;
using NoNameGun.Entities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoNameGun
{
    public class EntityMover : NetworkBehaviour, IEntityComponent
    {
        public event Action<Vector2> OnMovement;
        //Todo : FSM으로 분류
        [SerializeField] private PlayerInputSO _inputSO;

        [Header("Collision detect")]
        [SerializeField] private Transform _groundCheckTrm;
        [SerializeField] private Vector3 _checkerSize;
        [SerializeField] private LayerMask _whatIsGround;

        [Header("Stat")]
        //Todo : StatSystem
        [SerializeField] private float _moveSpeed = 5f;

        private Entity _entity;
        private Rigidbody _rbCompo;
        private Vector2 _moveVelo;

        //컴포넌트 Init
        public void Init(Entity entity)
        {
            _entity = entity;

            _rbCompo = _entity.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // if (IsOwner)
            // {
            //     Move();
            // }

            Move();

            DebugCode();
        }

        private void DebugCode()
        {
            Debug.Log(IsGroundDetected());
        }

        private void Move()
        {
            Vector3 moveDirection = transform.forward * _inputSO.InputDir.y + transform.right * _inputSO.InputDir.x;
            _rbCompo.MovePosition(_rbCompo.position + moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime);

            OnMovement?.Invoke(moveDirection.normalized * _moveSpeed);

            transform.Rotate(Vector3.up * _inputSO.MouseDelta.x);
        }


        public void SetMovement(Vector2 movement)
        {
            _moveVelo = movement;
        }

        public virtual bool IsGroundDetected()
            => Physics.BoxCast(_groundCheckTrm.position, _checkerSize * 0.5f, Vector3.down,
            Quaternion.identity, _checkerSize.y, _whatIsGround);

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            if (_groundCheckTrm != null)
            {
                // BoxCast에 맞춘 위치 및 크기
                Vector3 boxCenter = _groundCheckTrm.position - Vector3.up * (_checkerSize.y * 0.5f);
                Vector3 boxSize = new Vector3(_checkerSize.x, _checkerSize.y, _checkerSize.z);

                // 박스를 시각화
                Gizmos.DrawWireCube(boxCenter, boxSize);
            }
        }
#endif

    }
}
