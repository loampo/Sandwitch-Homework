using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            if (!TryGetComponent<T>(out instance))
            {
                instance = gameObject.AddComponent<T>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance);
    }
}
