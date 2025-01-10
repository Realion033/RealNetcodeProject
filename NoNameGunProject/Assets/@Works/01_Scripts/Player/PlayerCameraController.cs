using Unity.Netcode;
using UnityEngine;

namespace NoNameGun
{
    public class PlayerCameraManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerCam;

        private void Start()
        {
            // 각 클라이언트에서 카메라 활성/비활성 처리
            ManageCamera();
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
                Debug.Log("Player camera activated for owner.");
            }
            else
            {
                // 다른 플레이어의 카메라는 비활성화
                _playerCam.SetActive(false);
                Debug.Log("Player camera deactivated for non-owner.");
            }
        }
    }
}
