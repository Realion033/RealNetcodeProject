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
        public float MouseSensitivity = 2f;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
