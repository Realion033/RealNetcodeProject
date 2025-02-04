using UnityEngine;
using System.Collections.Generic;

namespace NoNameGun.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, GameObject> uiPanels = new Dictionary<string, GameObject>();     

        private void Start()
        {
            foreach (Transform child in transform)
            {
                uiPanels[child.gameObject.name] = child.gameObject;
                child.gameObject.SetActive(false);
            }
        }

        public void ShowPanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }

        public void HidePanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }

        public void TogglePanel(string panelName)
        {
            if (uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                panel.SetActive(!panel.activeSelf);
            }
            else
            {
                Debug.LogWarning($"UIManager: {panelName} 패널을 찾을 수 없습니다.");
            }
        }
    }
}
