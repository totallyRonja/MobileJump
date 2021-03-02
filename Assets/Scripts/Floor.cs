using UnityEngine;

public class Floor : MonoBehaviour {
    [SerializeField] private Transform FollowObject;
    [SerializeField] private float MaxHeightDiff;

    private Transform trans;
    
    private void Awake() {
        trans = transform;
    }

    private void Update()
    {
        if (FollowObject.position.y - trans.position.y > MaxHeightDiff)
            trans.position = trans.position.Y(FollowObject.position.y - MaxHeightDiff);
    }
}
