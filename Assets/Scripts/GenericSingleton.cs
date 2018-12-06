using UnityEngine;

public abstract class GenericSingleton<T> : MonoBehaviour where T: Component
{

    private static T m_instance;
    protected bool m_persistent = false;

    public static T Instance()
    {
        if (m_instance == null)
        {
            if ((m_instance = FindObjectOfType<T>()) == null)
            {
                GameObject obj = new GameObject {name = typeof(T).Name};
                m_instance = obj.AddComponent<T>();
            }

        }
        return m_instance;
    }

    private void Awake()
    {
        if (m_instance != null && m_instance != this as T)
        {
            Destroy(gameObject);
            return;
        }

        OnAwake();

        m_instance = this as T;
        if (m_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnAwake()
    {
        //derived classes provide implementation, such as setting the persistent bool
    }
}
