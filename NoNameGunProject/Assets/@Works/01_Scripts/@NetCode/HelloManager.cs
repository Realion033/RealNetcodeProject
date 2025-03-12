using NoNameGun.Managers;
using Unity.Netcode;
using UnityEngine;

// todo : Make LobbyManager

namespace HelloManager
{
    public class HelloManager : MonoBehaviour
    {
        private static NetworkManager m_NetworkManager;

        public static float fps;

        void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 600, 600));
            if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
            if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
            if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
        }

        static void StatusLabels()
        {
            var mode = m_NetworkManager.IsHost ?
                "Host" : m_NetworkManager.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " +
                m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
            GUILayout.Label("FPS : " + fps);
        }

        private float deltaTime = 0.0f;

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
        }
    }
}