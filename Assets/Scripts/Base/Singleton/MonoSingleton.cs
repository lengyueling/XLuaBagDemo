using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected MonoSingleton()
    {

    }
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }

}
