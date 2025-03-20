using Unity.Netcode;
using UnityEngine;
using System;

namespace NoNameGun.Players
{
    public class PlayerMovement : NetworkBehaviour
    {
        public event Action<Vector2> OnMovement;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _sprintMultiplier = 1.5f;

        [Header("Collision Detection")]
        [SerializeField] private Transform _groundCheckTrm;
        [SerializeField] private Vector3 _checkerSize;
        [SerializeField] private LayerMask _whatIsGround;

        public bool CanMove = true;

        #region PRIVATE_VARIABLE
        private Vector3 _cachedHorizontalVelocity = Vector3.zero; // 점프 시 저장할 수평 속도
        private Player _player;
        private Rigidbody _rbCompo;

        private Vector2 _afterMoveInput;
        private bool _isSprinting = false;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _player = GetComponent<Player>();
            _rbCompo = GetComponent<Rigidbody>();

            _player.PlayerInput.JumpEvt += HandleJumpEvt;
            _player.PlayerInput.SprintEvt += HandleSprintEvt;
        }

        private void OnDisable()
        {
            _player.PlayerInput.JumpEvt -= HandleJumpEvt;
            _player.PlayerInput.SprintEvt -= HandleSprintEvt;
        }

        private void Update()
        {
            if (!IsOwner) return;

            // 현재 입력값 가져오기
            if (CanMove)
            {
                Vector2 inputDir = _player.PlayerInput.InputDir;
                Vector2 mouseDelta = _player.PlayerInput.MouseDelta;

                // 클라이언트에서 즉시 이동 처리
                Move(inputDir, mouseDelta.x);
            }
        }

        private void Move(Vector2 inputDir, float mouseDeltaX)
        {
            float multiplier = _isSprinting ? _sprintMultiplier : 1;
            bool isGrounded = IsGroundDetected(); // 현재 착지 여부 확인

            _afterMoveInput = Vector2.Lerp(_afterMoveInput, inputDir, _smoothTime * Time.deltaTime);
            Vector3 dir = new Vector3(_afterMoveInput.x, 0, _afterMoveInput.y);
            dir = transform.TransformDirection(dir);

            Vector3 targetVelocity;

            if (!isGrounded)
            {
                // 공중에서 입력 방향에 따라 이동 가능하게 설정
                targetVelocity = new Vector3(dir.x * _player.MoveSpeed * multiplier,
                                            _rbCompo.linearVelocity.y,  // Y축 속도는 계속 중력에 의해 조정
                                            dir.z * _player.MoveSpeed * multiplier);

                // 공중에서의 이동 방향을 부드럽게 조정
                targetVelocity = Vector3.Lerp(_rbCompo.linearVelocity, targetVelocity, _smoothTime * Time.deltaTime);
            }
            else
            {
                // 착지 후 이동 속도를 부드럽게 변화시킴
                targetVelocity = new Vector3(dir.x * _player.MoveSpeed * multiplier,
                                            _rbCompo.linearVelocity.y,
                                            dir.z * _player.MoveSpeed * multiplier);

                // 착지 시 현재 속도를 저장
                _cachedHorizontalVelocity = new Vector3(targetVelocity.x, 0, targetVelocity.z);
            }

            // 최종 속도 적용
            _rbCompo.linearVelocity = targetVelocity;

            // 회전 처리
            Quaternion deltaRotation = Quaternion.Euler(0f, mouseDeltaX * _player.MouseSensitivity, 0f);
            _rbCompo.MoveRotation(_rbCompo.rotation * deltaRotation);

            OnMovement?.Invoke(_afterMoveInput);
        }


        public bool IsGroundDetected()
            => Physics.BoxCast(_groundCheckTrm.position, _checkerSize * 0.5f, Vector3.down,
                Quaternion.identity, _checkerSize.y, _whatIsGround);
        #endregion

        #region EVT_FUNC
        private void HandleJumpEvt()
        {
            if (IsOwner && IsGroundDetected())
            {
                // 점프하기 전에 현재 수평 속도 저장
                _cachedHorizontalVelocity = new Vector3(_rbCompo.linearVelocity.x, 0, _rbCompo.linearVelocity.z);
                _rbCompo.AddForce(Vector3.up * _player.JumpPower);
            }
        }

        private void HandleSprintEvt(bool isSprint)
        {
            _isSprinting = isSprint;
        }
        #endregion

        #region RPC_FUNC

        // [ServerRpc]
        // private void MoveServerRpc(Vector2 inputDir, float mouseDeltaX)
        // {
        //     Move(inputDir, mouseDeltaX);
        // }

        // [ServerRpc]
        // private void JumpServerRpc(float JumpPower)
        // {
        //     _rbCompo.AddForce(Vector3.up * JumpPower);
        // }
        #endregion

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
    }
}