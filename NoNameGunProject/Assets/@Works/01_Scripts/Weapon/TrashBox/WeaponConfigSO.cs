using UnityEngine;

namespace NoNameGun.Weaopon
{
    public enum WeaponType
    {
        PISTOL,
        KNIFE,
        OTHER
    }

    public enum WeaponRating
    {
        APOLLON,
        SENTINEL,
        GLITCH
    }

    [CreateAssetMenu(fileName = "WeaponConfigSO", menuName = "Scriptable Objects/WeaponConfigSO")]
    public class WeaponSO : ScriptableObject
    {
        public bool IsRaycast;
        public GameObject Bullet;

        [Header("IK")]
        public Transform LeftHandTrm;
        public Transform RightHandTrm;
    }
}
