using UnityEngine;

namespace NoNameGun.Players
{
    public enum PlayerStateEnum
    {
        Idle,
        Run,
        Fall,
        Attack,
        Reload,
        Dash
    }
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [Header("Player Stats")]
        public float MoveSpeed = 5f;
        public float JumpPower = 3f;
        public float MouseSensitivity = 2f;

        private PlayerAnimation _playerAnim;

        private void Awake()
        {
            _playerAnim = GetComponentInChildren<PlayerAnimation>();
        }

        private void Update()
        {
            Debug.Log($"x : {PlayerInput.InputDir.x}, y : {PlayerInput.InputDir.y}");
        }
    }
}
