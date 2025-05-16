using UnityEngine;
using NoNameGun.Players;

namespace NoNameGun.Weaopon
{
    public abstract class GunBase : InitBase
    {
        protected Player _player;

        private void Awake()
        {
            Init();

            if (_init == false)
            {
                return;
            }

            _player = GetComponentInParent<Player>();
        }
        public virtual void Zoom() { }


        public abstract void Fire();
        public abstract void Reload();

    }
}
