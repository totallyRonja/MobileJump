using System;
using UnityEngine;

public class Floor : MonoBehaviour {
	[SerializeField] private Transform FollowObject;
	[SerializeField] private float MaxHeightDiff;

	private Transform trans;
	private float baseY;
    
	private void Awake() {
		trans = transform;
		baseY = trans.position.y;
	}

	private void Update()
	{
		if (FollowObject.position.y - trans.position.y > MaxHeightDiff)
			trans.position = trans.position.Y(FollowObject.position.y - MaxHeightDiff);
	}

	public void Reset() {
		trans.position = trans.position.Y(baseY);
	}
}