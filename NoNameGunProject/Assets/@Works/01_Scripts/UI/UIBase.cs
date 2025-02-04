using UnityEngine;

namespace NoNameGun.UI
{
    public class UIBase : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        public virtual void Init() { }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ToggleVisibility()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
