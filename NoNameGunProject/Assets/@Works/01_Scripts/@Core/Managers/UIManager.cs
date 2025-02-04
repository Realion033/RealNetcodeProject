using UnityEngine;
using System.Collections.Generic;
using NoNameGun.UI;

namespace NoNameGun.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, UIBase> uiPanels = new Dictionary<string, UIBase>();     

        private void Start()
        {
            foreach (Transform child in transform)
            {
                UIBase uiBase = child.GetComponent<UIBase>();

                if (uiBase != null)
                {
                    uiPanels[child.gameObject.name] = uiBase;
                    child.gameObject.SetActive(false);
                }
            }
        }

        public void ShowPanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out UIBase panel))
            {
                panel.Show();
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }

        public void HidePanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out UIBase panel))
            {
                panel.Hide();
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }

        public void TogglePanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out UIBase panel))
            {
                panel.ToggleVisibility();
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }
    }
}
