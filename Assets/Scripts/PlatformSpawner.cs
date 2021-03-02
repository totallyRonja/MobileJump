using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
    
    [SerializeField] private LevelChunk[] ChunkPrefabs;

    private int[] weightRedirection;
    private Camera cam;
    private Transform trans;
    private float currentHeight;
    
    private void Awake() {
        cam = Camera.main;
        trans = transform;
        currentHeight = trans.position.y;

        weightRedirection = ChunkPrefabs.Select(chunk => chunk.Weight)
            .SelectMany((weight, index) => Enumerable.Repeat(index, weight))
            .ToArray();
    }

    private void LateUpdate() {
        var cameraEdge = cam.transform.position.y + AspectHelper.Instance.WorldSize.y;
        while (currentHeight < cameraEdge) {
            var index = weightRedirection.RandomElement();
            var chunk = ChunkPrefabs[index];
            foreach (var newObject in chunk.ContainedObjects) {
                var instance = PoolManager.Instance.Spawn(newObject.Object);
                instance.transform.position = newObject.Position.Y(newObject.Position.y + currentHeight);
            }
            currentHeight += chunk.Height;
        }
    }
}
