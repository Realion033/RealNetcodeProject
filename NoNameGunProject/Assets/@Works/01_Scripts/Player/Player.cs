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

        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerAnim = GetComponentInChildren<PlayerAnimation>();
        }

        private void Update()
        {
            Debug.Log($"x : {PlayerInput.InputDir.x}, y : {PlayerInput.InputDir.y}");
        }
    }
}
