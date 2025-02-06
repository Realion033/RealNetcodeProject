using Unity.Netcode;
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
    public class Player : MonoBehaviour, IDamagable
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [Header("Player Stats")]
        public float PlayerHealth = 100f;
        public float MoveSpeed = 5f;
        public float JumpPower = 3f;
        public float MouseSensitivity = 2f;

        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerAnimation PlayerAnim { get; private set; }

        private float _currentHealth;
        private NetworkObject _netplayer;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerAnim = GetComponentInChildren<PlayerAnimation>();
            _netplayer = GetComponent<NetworkObject>();
        }

        private void Start()
        {
            _currentHealth = PlayerHealth;
        }

        private void Update()
        {
            Debug.Log($"x : {PlayerInput.InputDir.x}, y : {PlayerInput.InputDir.y}");

            if (_currentHealth <= 0)
            {
                Debug.Log("Die");
                gameObject.SetActive(false);
            }
        }

        public void ApplyDamage(float damage)
        {
            _currentHealth = -damage;
        }
    }
}
