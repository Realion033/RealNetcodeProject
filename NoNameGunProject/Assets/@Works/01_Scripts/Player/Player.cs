using NoNameGun.Entities;
using UnityEngine;

namespace NoNameGun.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    }
}
