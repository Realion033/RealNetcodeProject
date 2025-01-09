using UnityEngine;

namespace NoNameGun
{
    [CreateAssetMenu(fileName = "AnimParamSO", menuName = "Scriptable Objects/AnimParamSO")]
    public class AnimParamSO : ScriptableObject
    {
        public string paramName;
        public int hashValue;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(paramName);
        }
    }
}
