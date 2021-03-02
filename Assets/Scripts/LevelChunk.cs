using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//ideologically this should be a scriptableobject, but monobehaviours are just way easier to quickly get authoring to work.
public class LevelChunk : MonoBehaviour {
    [SerializeField] public float Height;
    [SerializeField] public int Weight;

    [SerializeField] public List<PositionedObject> ContainedObjects;

#if UNITY_EDITOR
    private void OnValidate() {
        ContainedObjects ??= new List<PositionedObject>();
        ContainedObjects.Clear();

        foreach (Transform child in transform) {
            var prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(child.gameObject);
            //only prefab children are allowed here
            if(prefab == null)
                continue;
            ContainedObjects.Add(new PositionedObject{ Position = child.localPosition, Object = prefab });
        }
    }
#endif

    private void OnDrawGizmos() {
        var cam = Camera.main;
        var cameraWidth = cam!.orthographicSize * cam.aspect * 2;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (Height/2), new Vector3(cameraWidth, Height, 1));
    }

    [Serializable]
    public struct PositionedObject {
        public Vector3 Position;
        public GameObject Object;
    }
}
