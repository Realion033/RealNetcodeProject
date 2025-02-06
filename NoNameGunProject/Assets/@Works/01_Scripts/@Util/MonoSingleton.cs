using System.Collections;
using System.Collections.Generic;
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
            Destroy(gameObject); // 기존 인스턴스가 있으면 제거
            return;
        }
    }

}