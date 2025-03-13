using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoNameGun
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        //입력 액션이벤트
        public event Action AttackEvt;
        public event Action JumpEvt;
        public event Action<bool> SprintEvt;

        public Vector2 InputDir { get; private set; }
        public Vector2 MouseDelta { get; private set; }

        private InputSystem_Actions _inputActions;

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputSystem_Actions();
                _inputActions.Player.SetCallbacks(this);
            }

            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
        }

        #region IPlayerActions

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackEvt?.Invoke();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpEvt?.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            MouseDelta = context.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            InputDir = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintEvt?.Invoke(true);
            }
            else if (context.canceled)
            {
                SprintEvt?.Invoke(false);
            }
        }

        #endregion
    }
}
