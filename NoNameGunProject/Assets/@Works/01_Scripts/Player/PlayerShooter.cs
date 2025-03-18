using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NoNameGun.Players
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private Transform _gunPivotPos;
        public GameObject currentGun;

        #region PRIVATE_VARIABLE
        private Transform _rightHandPos;
        private Transform _leftHandPos;
        private Player _player;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            _player = GetComponent<Player>();
            _rightHandPos = currentGun.transform.Find(Define.GunIKHandle.RightHandIK);
        }

        private void Start()
        {
            GameObject newGun = Instantiate(currentGun, _gunPivotPos.position, Quaternion.identity, _gunPivotPos);

            _rightHandPos = newGun.transform.Find(Define.GunIKHandle.RightHandIK);
            if (_rightHandPos == null)
            {
                Debug.LogError("[PlayerShooter] RightHandIK 위치를 찾을 수 없습니다.");
            }

            _player.PlayerAnim.GunRightHandleTrm = _rightHandPos;
        }
        #endregion
    }
}
