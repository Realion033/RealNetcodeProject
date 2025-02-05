using UnityEngine;
using Unity.Netcode;

namespace NoNameGun.Players
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private Animator _animator;
        private Player _player;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponentInParent<Player>();

            // 입력 값 변경 이벤트 구독
            _player.PlayerMovement.OnMovement += UpdateMoveAnimation;
        }

        private void OnDisable()
        {
            _player.PlayerMovement.OnMovement -= UpdateMoveAnimation;
        }

        private void UpdateMoveAnimation(Vector2 inputDir)
        {
            // 클라이언트에서 애니메이션 갱신 후, 서버에도 동기화 요청
            _animator.SetFloat("MoveX", inputDir.x);
            _animator.SetFloat("MoveY", inputDir.y);

            //UpdateMoveAnimationServerRpc(inputDir);
        }

        // [ServerRpc]
        // private void UpdateMoveAnimationServerRpc(Vector2 inputDir)
        // {
        //     UpdateMoveAnimationClientRpc(inputDir);
        // }

        // [ClientRpc]
        // private void UpdateMoveAnimationClientRpc(Vector2 inputDir)
        // {
        //     if (!IsOwner) // 본인 클라이언트에서 중복 실행 방지
        //     {
        //         _animator.SetFloat("MoveX", inputDir.x);
        //         _animator.SetFloat("MoveY", inputDir.y);
        //     }
        // }
    }
}
