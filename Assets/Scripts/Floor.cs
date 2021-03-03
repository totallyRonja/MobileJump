using UnityEngine;

public class Floor : MonoBehaviour {
	[SerializeField] private Transform FollowObject;
	[SerializeField] private float MaxHeightDiff;
	[SerializeField] private EventProperty Reset;

	private Transform trans;
	private float baseY;
    
	private void Awake() {
		trans = transform;
		baseY = trans.position.y;
		Reset?.Event.AddListener(ResetValues);
	}

	private void Update()
	{
		if (FollowObject.position.y - trans.position.y > MaxHeightDiff)
			trans.position = trans.position.Y(FollowObject.position.y - MaxHeightDiff);
	}

	public void ResetValues() {
		trans.position = trans.position.Y(baseY);
	}
}