using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    public static T instance; 

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(instance == null)
        {
            if(!TryGetComponent<T>(out instance))
            {
                instance = gameObject.AddComponent<T>();
            }
        }else
        {
            Destroy(gameObject);
        }
    }
}