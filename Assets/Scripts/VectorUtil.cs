using UnityEngine;

public static class VectorUtil {
	public static Vector3 X(this Vector3 vector, float newX) {
		vector.x = newX;
		return vector;
	}
	
	public static Vector3 Y(this Vector3 vector, float newY) {
		vector.y = newY;
		return vector;
	}
	
	public static Vector3 Z(this Vector3 vector, float newZ) {
		vector.z = newZ;
		return vector;
	}
	
	public static Vector2 X(this Vector2 vector, float newX) {
		vector.x = newX;
		return vector;
	}
	
	public static Vector2 Y(this Vector2 vector, float newY) {
		vector.y = newY;
		return vector;
	}
}
