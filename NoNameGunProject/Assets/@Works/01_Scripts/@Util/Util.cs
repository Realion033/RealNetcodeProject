using UnityEngine;

public static class Util
{
    #region MAIN_FUNC
    public static T GetOrAddComponenet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }
        else
        {
            foreach (T item in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || item.name == name)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public static Color HexToColor(string v)
    {
        if (v.Contains("#") == false)
        {
            v = $"#{v}";
        }

        ColorUtility.TryParseHtmlString(v, out Color parsedColor);

        return parsedColor;
    }
    #endregion
}
