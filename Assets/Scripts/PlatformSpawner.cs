using System.Linq;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
    
	[SerializeField] private LevelChunk StartChunk;
	[SerializeField] private LevelChunk[] ChunkPrefabs;
	[SerializeField] private EventProperty Reset;

	private int[] weightRedirection;
	private Camera cam;
	private Transform trans;
	private float currentHeight;
    
	private void Start() {
		cam = Camera.main;
		trans = transform;
		currentHeight = trans.position.y;

		weightRedirection = ChunkPrefabs.Select(chunk => chunk.Weight)
			.SelectMany((weight, index) => Enumerable.Repeat(index, weight))
			.ToArray();

		Reset?.Event.AddListener(ResetValues);
		if (StartChunk != null)
			AddChunk(StartChunk);
	}

	private void ResetValues() {
		PoolManager.Instance.Clear();
		currentHeight = trans.position.y;
		
		if (StartChunk != null)
			AddChunk(StartChunk);
	}

	private void LateUpdate() {
		var cameraEdge = cam.transform.position.y + AspectHelper.Instance.WorldSize.y;
		while (currentHeight < cameraEdge) {
			var index = weightRedirection.RandomElement();
			var chunk = ChunkPrefabs[index];
			AddChunk(chunk);
		}
	}

	private void AddChunk(LevelChunk chunk) {
		foreach (var newObject in chunk.ContainedObjects) {
			var instance = PoolManager.Instance.Spawn(newObject.Object);
			instance.transform.position = newObject.Position.Y(newObject.Position.y + currentHeight);
		}
		currentHeight += chunk.Height;
	}
}