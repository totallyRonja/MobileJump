using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScrollingBackground : MonoBehaviour {
    [Range(0, 2)] [SerializeField] private float Parallax = 1;
    [SerializeField] private GameObject BackgroundElement;

    [SerializeField] [HideInInspector] private int LastInstance = -1;
    [SerializeField] [HideInInspector] private float LastCamHeight = -1;
    [SerializeField] [HideInInspector] private float ElementHeight;

    private Camera cam;
    private float startHeight = 0;
    private float startCameraHeight = 0;
    private Transform trans;

    private void Awake() {
        cam = Camera.main;
        trans = transform;
        startHeight = trans.position.y;
        startCameraHeight = cam!.transform.position.y;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        //OnValidate calculates the needed amount of background objects and assembles them automatically in the editor
        
        //only redo when relevant data exists and its new data to avoid editor lag adding up
        if(BackgroundElement == null)
            return;
        var instance = BackgroundElement.GetInstanceID();
        var camHeight = Camera.main!.orthographicSize;
        if (instance == LastInstance && camHeight == LastCamHeight) {
            //last settings as last time presumably
            return;
        }
        LastInstance = instance;
        LastCamHeight = camHeight;
        //cleanup
        foreach (Transform child in transform)
            Util.DestroyEditorSafe(child.gameObject);
        //reconstruct
        Bounds? boundingBox = null;
        foreach (Renderer renderer in BackgroundElement.GetComponentsInChildren<Renderer>()) {
            if (boundingBox == null)
                boundingBox = renderer.bounds;
            else
                boundingBox.Value.Encapsulate(renderer.bounds);
        }
        if (boundingBox == null) {
            Debug.LogWarning("Element doesnt have any renderers and is not suited as a background element");
            return;
        }
        ElementHeight = boundingBox.Value.size.y;
        var screenHeight = Camera.main!.orthographicSize * 2;
        var elementCount = Mathf.CeilToInt(screenHeight / ElementHeight) + 1;
        for (int i = 0; i < elementCount; i++) {
            var newObject = (GameObject) PrefabUtility.InstantiatePrefab(BackgroundElement, transform);
            newObject.transform.localPosition = new Vector3(0, -i * ElementHeight, 0);
        }
    }
    
    //in case we manually changed the prefab and want to force a rebuild
    [ContextMenu("Force Rebuild")]
    private void ForceRebuild() {
        LastInstance = -1;
        LastCamHeight = -1;
        OnValidate();
    }
#endif

    
    private void LateUpdate() {
        //get change in camera position
        var camHeight = cam.transform.position.y;
        var diff = camHeight - startCameraHeight;
        //apply parallax to that
        diff *= Parallax;
        var pos = startHeight + diff;
        //and then choose the multiple thats just above the camera top edge
        pos = Mathf.Ceil((camHeight + cam.orthographicSize - pos) / ElementHeight) * ElementHeight + pos;
        trans.position = trans.position.Y(pos);
    }
}
