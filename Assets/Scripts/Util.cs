using System.Collections;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;

public static class Util {
	public static void DestroyEditorSafe(Object victim) {
#if UNITY_EDITOR
		//delay destruction outside the current call context (for example for OnValidate where we cant destroy)
		EditorCoroutineUtility.StartCoroutineOwnerless(DestroyRoutine(victim));
#else
		//this should never be called in the game, but if we do, this should work.
		Destroy(victim);
#endif
	}

	private static IEnumerator DestroyRoutine(Object victim) {
		yield return new WaitForEndOfFrame();
		Object.DestroyImmediate(victim);
	}
}