using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] private Transform HeightMeasurement;
	[SerializeField] private FloatProperty MaxHeight;
	[SerializeField] private IntProperty UniqueJumps;
	[SerializeField] private FloatProperty RecordHeight;
	[SerializeField] private IntProperty RecordJumps;
	[SerializeField] private EventProperty Death;
	[SerializeField] private EventProperty Retry;
	[SerializeField] private IntProperty State;

	private float heightZero;

	private void Awake() {
		heightZero = HeightMeasurement.position.y;
		Death?.Event.AddListener(GameOver);
		Retry?.Event.AddListener(Restart);
	}

	private void Start() {
		Restart();
	}

	private void GameOver() {
		State.Value = (int) GameState.GameOver;

		if (MaxHeight.Value > RecordHeight.Value)
			RecordHeight.Value = MaxHeight.Value;
		if (UniqueJumps.Value > RecordJumps.Value)
			RecordJumps.Value = UniqueJumps.Value;
		
		//todo: consider saving the highscore over multiple sessions
	}

	private void Restart() {
		MaxHeight.ResetValues();
		UniqueJumps.ResetValues();
		
		State.Value = (int) GameState.Playing;
	}

	private void LateUpdate() {
		//check how far the measurement object (usually player) moved upwards and update when its higher than so far
		var height = HeightMeasurement.position.y - heightZero;
		if (height > MaxHeight.Value)
			MaxHeight.Value = height;
	}
}