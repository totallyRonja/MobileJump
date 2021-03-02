using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    public static PoolManager Instance;

    [SerializeField] private float CleanupOffset;

    private readonly Dictionary<int, Stack<GameObject>> pools = new Dictionary<int, Stack<GameObject>>();
    private readonly Queue<Transform> activeObjects = new Queue<Transform>();
    private Transform trans;

    private void Awake() {
        trans = transform;
    }

    private void OnEnable() {
        if(Instance != null)
            Debug.LogWarning("There are two pool managers in your scene, this shouldn't happen!");
        Instance = this;
    }

    private void OnDisable() {
        if (Instance == this)
            Instance = null;
    }

    private void LateUpdate() {
        //here we send items back into the pool
        //we only check the uppermost item in the queue
        //this assumes items are approximately inserted bottom to top and dont move too much
        //but it also makes the checks faster depending on the number of active items
        if(activeObjects.Count == 0) return;
        var lastObject = activeObjects.Peek();
        while (lastObject.position.y < trans.position.y - CleanupOffset) {
            var id = lastObject.GetComponent<PooledObject>().Id;
            var pool = GetPool(id);
            var itemToDisable = activeObjects.Dequeue().gameObject;
            itemToDisable.SetActive(false);
            pool.Push(itemToDisable);
            lastObject = activeObjects.Peek();
        }
    }

    public GameObject Spawn(GameObject prefab) {
        var id = prefab.GetInstanceID();
        var pool = GetPool(id);
        GameObject instance;
        if (pool.Count > 0)
            instance = pool.Pop();
        else {
            instance = Instantiate(prefab);
            instance.AddComponent<PooledObject>().Id = id;
        }

        instance.SetActive(true);
        instance.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
        activeObjects.Enqueue(instance.transform);
        return instance;
    }

    public void Clear() {
        //hide and pool all currently active objets
        var activeObject = activeObjects.Dequeue()?.gameObject;
        while (activeObject != null) {
            var pool = GetPool(activeObject.gameObject.GetInstanceID());
            activeObject.SetActive(false);
            pool.Push(activeObject);
            activeObject = activeObjects.Dequeue()?.gameObject;
        }
    }

    private Stack<GameObject> GetPool(int id) {
        if (!pools.TryGetValue(id, out var pool)) {
            pool = new Stack<GameObject>();
            pools.Add(id, pool);
        }
        return pool;
    }

    private void OnDrawGizmos() {
        var cam = Camera.main;
        var cameraWidth = cam!.orthographicSize * cam.aspect * 2;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * (CleanupOffset + 1), new Vector3(cameraWidth, 2, 1));
    }
}
