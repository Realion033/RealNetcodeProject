using UnityEngine;

namespace NoNameGun.Managers
{
    public class Managers : MonoBehaviour
    {
        private static Managers s_instance;
        private static Managers Instance { get { Init(); return s_instance; } }

        #region UI_VARIABLE
        private UIManager _ui = new UIManager();
        private MultiNetcodeManager _multi = new MultiNetcodeManager();

        public static UIManager UI { get { return Instance?._ui; } }
        public static MultiNetcodeManager Multi { get { return Instance?._multi; } }
        #endregion
        private static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);

                s_instance = go.GetComponent<Managers>();
            }
        }
    }
}
