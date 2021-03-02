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

    // Update is called once per frame
    private void LateUpdate() {
        var cameraEdge = cam.transform.position.y + cam.orthographicSize;
        while (currentHeight < cameraEdge) {
            var index = weightRedirection.RandomElement();
            var chunk = ChunkPrefabs[index];
            foreach (var newObject in chunk.ContainedObjects) {
                Instantiate(newObject.Object, newObject.Position.Y(newObject.Position.y + currentHeight), Quaternion.identity);
            }
            currentHeight += chunk.Height;
        }
    }
}
