using NoNameGun.Entities;
using UnityEngine;

namespace NoNameGun.Players
{
    public enum PlayerStateEnum
    {
        Idle,
        Run,
        Fall,
        Attack,
        Dash
    }
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
        }
    }
}
