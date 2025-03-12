using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace NoNameGun.Players
{
    public class PlayerMovement : NetworkBehaviour
    {
        //움직임 감지 액션 (추후 필요할수도?)
        public event Action<Vector2> OnMovement;
        [SerializeField] private float smoothTime = 0.1f;

        [Header("Collision Detection")]
        [SerializeField] private Transform _groundCheckTrm;
        [SerializeField] private Vector3 _checkerSize;
        [SerializeField] private LayerMask _whatIsGround;

        public bool CanMove = true;

        private Player _player;
        private Rigidbody _rbCompo;

        private Vector2 afterMoveInput;

        // 컴포넌트 초기화
        private void Awake()
        {
            _player = GetComponent<Player>();
            _rbCompo = GetComponent<Rigidbody>();

            _player.PlayerInput.JumpEvt += HandleJumpEvt;
        }

        private void OnDisable()
        {
            _player.PlayerInput.JumpEvt -= HandleJumpEvt;
        }
        
        private void Update()
        {
            if (!IsOwner) return;

            // 현재 입력값 가져오기
            if (CanMove)
            {
                Vector2 inputDir = _player.PlayerInput.InputDir;
                Vector2 mouseDelta = _player.PlayerInput.MouseDelta;
                // Rpc로 동기화
                MoveServerRpc(inputDir, mouseDelta.x);
                //Move(inputDir, mouseDelta.x);
            }
        }

        private void Move(Vector2 inputDir, float mouseDeltaX)
        {
            // 이동 처리
            afterMoveInput = Vector2.Lerp(afterMoveInput, inputDir, smoothTime);
            Vector3 dir = new Vector3(afterMoveInput.x, 0, afterMoveInput.y);
            dir = transform.TransformDirection(dir);
            _rbCompo.linearVelocity = new Vector3(dir.x * _player.MoveSpeed, _rbCompo.linearVelocity.y, dir.z * _player.MoveSpeed);

            // 캐릭터 회전 처리 (좌우)
            Quaternion deltaRotation = Quaternion.Euler(0f, mouseDeltaX * _player.MouseSensitivity, 0f);
            _rbCompo.MoveRotation(_rbCompo.rotation * deltaRotation);

            // 이동 이벤트 발생
            OnMovement?.Invoke(afterMoveInput);
        }

        public bool IsGroundDetected()
            => Physics.BoxCast(_groundCheckTrm.position, _checkerSize * 0.5f, Vector3.down,
                Quaternion.identity, _checkerSize.y, _whatIsGround);

        private void HandleJumpEvt()
        {
            if (IsOwner && IsGroundDetected())
            {
                JumpServerRpc(_player.JumpPower);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            if (_groundCheckTrm != null)
            {
                Vector3 boxCenter = _groundCheckTrm.position - Vector3.up * (_checkerSize.y * 0.5f);
                Gizmos.DrawWireCube(boxCenter, _checkerSize);
            }
        }
#endif

        #region RPCs

        [ServerRpc]
        private void MoveServerRpc(Vector2 inputDir, float mouseDeltaX)
        {
            Move(inputDir, mouseDeltaX);
        }

        [ServerRpc]
        private void JumpServerRpc(float JumpPower)
        {
            _rbCompo.AddForce(Vector3.up * JumpPower);
        }

        #endregion
    }
}