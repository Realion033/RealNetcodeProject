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

        #region PRIVATE_VARIABLE
        private Player _player;
        private float _cameraPitch;
        #endregion

        #region UNITY_FUNC
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
        #endregion

        #region MAIN_FUNC
        private void CalcCameraAngle()
        {
            float angle = _playerCam.transform.eulerAngles.x;

            if (angle > 100)
            {
                angle -= 360f;
            }

            angle = (-angle + 80) / 160f - 0.05f;

            if (angle < 0)
            {
                angle = 0;
            }
            else if (angle > 1)
            {
                angle = 1;
            }

            CameraAngleCalcValue = angle;
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
        #endregion

        #region VIRTUAL_FUNC
        public override void OnNetworkSpawn()
        {
            ManageCamera();
        }
        #endregion

        #region RPC_FUNC
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
        #endregion
    }
}
