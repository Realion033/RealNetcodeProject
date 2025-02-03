using UnityEngine;

namespace NoNameGun.Players
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    }
}
