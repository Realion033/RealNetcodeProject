using System;
using Unity.Netcode;
using UnityEngine;

namespace NoNameGun.Players
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Header("Camera Settings")]
        public Transform CameraTransform;
        [SerializeField] private float _cameraPitchLimit = 80f;
        [SerializeField] private GameObject _playerCam;

        public float CameraAngleCalcValue;

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

            CalcCameraAngle();
            HandleCameraRotation(mouseDelta.y);
            CameraRotationServerRpc(_cameraPitch);
        }

        private void CalcCameraAngle()
        {
            float rotationX = _player.PlayerCamera.transform.eulerAngles.x;

            if (rotationX > 180f)
                rotationX -= 360f;

            float normalizedValue = 1 - ((rotationX + 80f) / 160f);

            normalizedValue = Mathf.Clamp01(normalizedValue);
            CameraAngleCalcValue = normalizedValue;

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
            if (CameraTransform == null) return;

            _cameraPitch -= mouseDeltaY * _player.MouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraPitchLimit, _cameraPitchLimit);
            CameraTransform.rotation = Quaternion.Euler(_cameraPitch, transform.eulerAngles.y, 0f);
        }

        [ServerRpc]
        private void CameraRotationServerRpc(float pitch)
        {
            CameraRotationClientRpc(pitch);
        }

        [ClientRpc]
        private void CameraRotationClientRpc(float pitch)
        {
            if (IsOwner) return; 
            _cameraPitch = pitch;
            CameraTransform.rotation = Quaternion.Euler(_cameraPitch, transform.eulerAngles.y, 0f);
        }
    }
}
