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
        //private float _previousMouseDeltaX;
        private float _cameraPitch;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            // 각 클라이언트에서 카메라 활성/비활성 처리
            ManageCamera();
        }

        private void Update()
        {
            Vector2 mouseDelta = _player.PlayerInput.MouseDelta;
            HandleCameraRotation(mouseDelta.y);
        }

        public override void OnNetworkSpawn()
        {
            // 네트워크 오브젝트가 스폰될 때도 카메라 관리
            ManageCamera();
        }

        private void ManageCamera()
        {
            if (_playerCam == null)
            {
                Debug.LogError("Player camera is not assigned!");
                return;
            }

            if (IsOwner)
            {
                // 자신 소유의 카메라는 활성화
                _playerCam.SetActive(true);
                //Debug.Log("Player camera activated for owner.");
            }
            else
            {
                // 다른 플레이어의 카메라는 비활성화
                _playerCam.SetActive(false);
                //Debug.Log("Player camera deactivated for non-owner.");
            }
        }

        private void HandleCameraRotation(float mouseDeltaY)
        {
            if (_cameraTransform == null) return;

            // 카메라 피치 계산 (위아래 회전 제한)
            _cameraPitch -= mouseDeltaY * _player.MouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraPitchLimit, _cameraPitchLimit);

            // 카메라 회전 적용
            _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
        }
    }
}
