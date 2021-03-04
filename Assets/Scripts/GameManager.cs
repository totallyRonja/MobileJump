using System;
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
	[SerializeField] private FloatProperty CountDown;

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

		if (MaxHeight > RecordHeight)
			RecordHeight.Value = MaxHeight;
		if (UniqueJumps > RecordJumps)
			RecordJumps.Value = UniqueJumps;
		
		//todo: consider saving the highscore over multiple sessions
	}

	private void Restart() {
		MaxHeight.ResetValues();
		UniqueJumps.ResetValues();
		
		State.Value = (int) GameState.Starting;
		Time.timeScale = 0;
		CountDown.Value = 3f;
	}

	private void Update() {
		if(State != (int)GameState.Starting) return;
		//the first frame in the editor is usually kind of a mess that takes multiple seconds
		//so lets just ignore it 
		if (Time.frameCount <= 1) {
			return;
		}
		
		CountDown.Value -= Time.unscaledDeltaTime;
		if (CountDown <= 0) {
			Time.timeScale = 1;
			State.Value = (int) GameState.Playing;
		}
	}

	private void LateUpdate() {
		if(State != (int)GameState.Playing) return;
		//check how far the measurement object (usually player) moved upwards and update when its higher than so far
		var height = HeightMeasurement.position.y - heightZero;
		if (height > MaxHeight)
			MaxHeight.Value = height;
	}
}