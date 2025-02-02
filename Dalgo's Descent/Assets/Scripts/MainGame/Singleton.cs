using UnityEngine;
public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    #region  Fields
    private static T _instance;
    private static readonly object Lock = new object();

    [SerializeField] protected bool _persistent = true;
    #endregion

    #region  Properties
    public static T Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance might not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return _instance;
            }
            lock (Lock)
            {
                if (_instance != null) return _instance;
                var instances = FindObjectsOfType<T>(true);
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return _instance = instances[0];
                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}")
                           .AddComponent<T>();
            }
        }
    }

    public static bool IsCreated => _instance != null;
    #endregion

    #region  Methods
    private void Awake()
    {
        OnAwake();
        if (_persistent)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnAwake() { }
    #endregion
}

public abstract class Singleton : MonoBehaviour
{
    #region  Properties
    public static bool Quitting { get; private set; }
    #endregion

    #region  Methods
    private void OnApplicationQuit() => Quitting = true;
    #endregion
}