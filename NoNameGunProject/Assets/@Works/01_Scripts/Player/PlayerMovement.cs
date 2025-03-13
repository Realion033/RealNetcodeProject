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
                // Rpc로 동기화
                MoveServerRpc(inputDir, mouseDelta.x);
                //Move(inputDir, mouseDelta.x);
            }
        }
        #endregion

        #region MAIN_FUNC
        private void Move(Vector2 inputDir, float mouseDeltaX)
        {
            float multiplier = _isSprinting ? _sprintMultiplier : 1;

            _afterMoveInput = Vector2.Lerp(_afterMoveInput, inputDir, _smoothTime);
            Vector3 dir = new Vector3(_afterMoveInput.x, 0, _afterMoveInput.y);
            dir = transform.TransformDirection(dir);
            _rbCompo.linearVelocity = new Vector3(dir.x * _player.MoveSpeed * multiplier,
            _rbCompo.linearVelocity.y, dir.z * _player.MoveSpeed * multiplier);

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
                JumpServerRpc(_player.JumpPower);
            }
        }

        private void HandleSprintEvt(bool isSprint)
        {
            _isSprinting = isSprint;
        }
        #endregion

        #region RPC_FUNC

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