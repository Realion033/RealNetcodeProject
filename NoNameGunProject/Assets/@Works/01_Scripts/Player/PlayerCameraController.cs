using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Players
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraPitchLimit = 80f;
        [SerializeField] private GameObject _playerCam;

        private Player _player;
        private float _cameraPitch;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            ManageCamera();
        }

        private void Update()
        {
            if (!IsOwner) return;

            Vector2 mouseDelta = _player.PlayerInput.MouseDelta;

            // 클라이언트에서 직접 회전 적용 (즉각 반응)
            HandleCameraRotation(mouseDelta.y);

            // 서버에 회전 값 전송하여 동기화
            CameraRotationServerRpc(_cameraPitch);
        }

        public override void OnNetworkSpawn()
        {
            ManageCamera();
        }

        private void ManageCamera()
        {
            if (_playerCam == null)
            {
                Debug.LogError("Player camera is not assigned!");
                return;
            }

            _playerCam.GetComponent<Camera>().enabled = IsOwner;
        }

        private void HandleCameraRotation(float mouseDeltaY)
        {
            if (_cameraTransform == null) return;

            // 클라이언트에서 즉시 회전 적용
            _cameraPitch -= mouseDeltaY * _player.MouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraPitchLimit, _cameraPitchLimit);
            _cameraTransform.rotation = Quaternion.Euler(_cameraPitch, transform.eulerAngles.y, 0f);
        }

        [ServerRpc]
        private void CameraRotationServerRpc(float pitch)
        {
            CameraRotationClientRpc(pitch);
        }

        [ClientRpc]
        private void CameraRotationClientRpc(float pitch)
        {
            if (IsOwner) return; // 이미 적용한 클라이언트는 제외
            _cameraPitch = pitch;
            _cameraTransform.rotation = Quaternion.Euler(_cameraPitch, transform.eulerAngles.y, 0f);
        }
    }
}
