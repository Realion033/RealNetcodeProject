using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NoNameGun.Players
{
    // public enum PlayerStateEnum //fsm으로 하는게 비효울적일듯 플레이어는 짜피 하나 만드는데 ㅇㅇ
    // {
    //     Idle,
    //     Run,
    //     Fall,
    //     Attack,
    //     Reload,
    //     Dash
    // }
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
        public PlayerCameraController PlayerCamera { get; private set; }

        #region PRIVATE_VARIABLE
        private float _currentHealth;
        private NetworkObject _netplayer;
        #endregion

        #region UNITY_FUNC
        private void Awake()
        {
            Init();

            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerCamera = GetComponent<PlayerCameraController>();
            PlayerAnim = GetComponentInChildren<PlayerAnimation>();
            _netplayer = GetComponent<NetworkObject>();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {

        }
        #endregion

        #region VIRTUAL_FUNC
        public virtual void Init()
        {
            _currentHealth = PlayerHealth;
        }

        public void ApplyDamage(float damage)
        {
            //_currentHealth = -damage;
        }
        #endregion
    }
}
