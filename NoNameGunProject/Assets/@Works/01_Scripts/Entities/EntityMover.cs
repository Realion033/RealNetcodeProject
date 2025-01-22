using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoNameGun.Entities
{
    public class EntityMover : NetworkBehaviour, IEntityComponent
    {
        //움직임 감지 액션 (추후 필요할수도?)
        public event Action<Vector2> OnMovement;

        [SerializeField] private PlayerInputSO _inputSO;

        [Header("Collision Detection")]
        [SerializeField] private Transform _groundCheckTrm;
        [SerializeField] private Vector3 _checkerSize;
        [SerializeField] private LayerMask _whatIsGround;

        [Header("Movement Stats")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _mouseSensitivity = 1f;

        [Header("Camera Settings")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraPitchLimit = 80f;

        private Entity _entity;
        private Rigidbody _rbCompo;

        private Vector2 _previousInputDir;
        private float _previousMouseDeltaX;
        private float _cameraPitch;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

        // 컴포넌트 초기화
        public void Init(Entity entity)
        {
            _entity = entity;
            _rbCompo = _entity.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            // 현재 입력값 가져오기
            Vector2 inputDir = _inputSO.InputDir;
            Vector2 mouseDelta = _inputSO.MouseDelta;

            // 주기적으로 서버로 입력 값 전송
            SubmitMoveInputServerRpc(inputDir, mouseDelta.x);

            // 클라이언트 로컬에서 카메라 회전 처리 (위아래)
            HandleCameraRotation(mouseDelta.y);
        }


        [ServerRpc]
        private void SubmitMoveInputServerRpc(Vector2 inputDir, float mouseDeltaX)
        {
            Move(inputDir, mouseDeltaX);

            if (Vector3.Distance(Position.Value, transform.position) > 0.01f ||
                Quaternion.Angle(Rotation.Value, transform.rotation) > 0.1f)
            {
                Position.Value = transform.position;
                Rotation.Value = transform.rotation;
            }
        }

        private void Move(Vector2 inputDir, float mouseDeltaX)
        {
            // 이동 처리
            Vector3 moveDirection = transform.forward * inputDir.y + transform.right * inputDir.x;
            _rbCompo.MovePosition(_rbCompo.position + moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime);

            // 캐릭터 회전 처리 (좌우)
            Quaternion deltaRotation = Quaternion.Euler(0f, mouseDeltaX * _mouseSensitivity, 0f);
            _rbCompo.MoveRotation(_rbCompo.rotation * deltaRotation);

            // 이동 이벤트 발생
            OnMovement?.Invoke(moveDirection.normalized * _moveSpeed);
        }

        private void HandleCameraRotation(float mouseDeltaY)
        {
            if (_cameraTransform == null) return;

            // 카메라 피치 계산 (위아래 회전 제한)
            _cameraPitch -= mouseDeltaY * _mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraPitchLimit, _cameraPitchLimit);

            // 카메라 회전 적용
            _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
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
                Vector3 boxCenter = _groundCheckTrm.position - Vector3.up * (_checkerSize.y * 0.5f);
                Gizmos.DrawWireCube(boxCenter, _checkerSize);
            }
        }
#endif
    }
}