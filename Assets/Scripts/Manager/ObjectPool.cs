using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ObjectPool : Singleton<ObjectPool>
{
    //private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new();
    private GameObject pool;



    // public static ObjectPool Instance
    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             instance = new ObjectPool();
    //         }
    //         return instance;
    //     }
    // }
    private void Start()
    {
        SceneManager.sceneLoaded += OnsceneLoad;
    }

    private void OnsceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        objectPool = new();
    }

    public GameObject GetObject(GameObject prefab, Transform _position, Quaternion _rotation)
    {
        GameObject _object;

        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = Instantiate(prefab);

            PushObject(_object);
            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        _object = objectPool[prefab.name].Dequeue();

        _object.transform.SetPositionAndRotation(_position.position, _rotation);
        _object.SetActive(true);
        return _object;
    }
    public GameObject GetObject(GameObject prefab, Vector2 _position, Quaternion _rotation)
    {
        GameObject _object;

        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = Instantiate(prefab);

            PushObject(_object);
            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        _object = objectPool[prefab.name].Dequeue();

        _object.transform.SetPositionAndRotation(_position, _rotation);
        _object.SetActive(true);
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);

        if (!objectPool.ContainsKey(_name))
            objectPool.Add(_name, new Queue<GameObject>());
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }

}
