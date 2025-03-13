// DONT NEED MORE

using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = Object.FindFirstObjectByType<T>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}