using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find(typeof(T).Name);

                if (obj == null)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogFormat("Create New Singleton Instance : {0}", typeof(T).Name);
                    }

                    obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
                else
                {
                    _instance = obj.GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    public static bool HasInstance()
    {
        return _instance != null ? true : false;
    }

    public static void DestroyInstance()
    {
        if (Instance == null)
        {
            return;
        }

        // 즉시 제거가 안되어서 제거될 오브젝트가 재참조 되는 현상 존대. Immediate로 변경합니다.
        // 이전 IL2CPP 빌드시 재확인이 필요합니다.
        DestroyImmediate(Instance.gameObject);
    }

    public virtual void OnDestroy()
    {
        _instance = null;
    }
}
