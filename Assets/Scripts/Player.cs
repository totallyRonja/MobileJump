using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum PlayerState {
	Default,
	Dead,
	RocketBoosting
}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
	[SerializeField] private float Gravity = -10;
	[SerializeField] private float MoveSpeed = 1;
	[SerializeField] private float JumpVelocity = 1;
	[SerializeField] private float SpringVelocity = 1;
	[SerializeField] private float Acceleration = 1;
	[SerializeField] private float DeathBopVelocity = 1;
	[SerializeField] private float RocketDuration = 2;
	[SerializeField] private float RocketSpeed = 7;
	[SerializeField] private float TouchBreakingZone = 0.5f;
	[SerializeField] private InputActionReference MoveKeys = null;
	[SerializeField] private InputActionReference MoveTouch = null;
	[SerializeField] private float Width = 1;
	[SerializeField] private EventProperty GameOver;
	[SerializeField] private EventProperty Retry;
	[SerializeField] private GameObject RocketDisplay;

	private float fallSpeed = 0;
	private float verticalSpeed = 0;
	private Rigidbody2D rigid;
	private Transform trans;
	private int hurtLayer;
	private float screenWidth;
	private Camera cam;
	private Vector3 startPosition;
	private float rocketTimer;
    
	private PlayerState state = PlayerState.RocketBoosting;

	private void Start() {
		rigid = GetComponent<Rigidbody2D>();
		trans = transform;
		startPosition = trans.position;
        
		MoveKeys.action.Enable();
		MoveTouch.action.Enable();

		hurtLayer = LayerMask.NameToLayer("Hurt");

		cam = Camera.main;
		screenWidth = 2 * AspectHelper.Instance.WorldSize.x + Width;

		Retry?.Event.AddListener(ResetValues);
		RocketDisplay.SetActive(false);
	}

	private void ResetValues() {
		trans.position = startPosition;
		fallSpeed = 0;
		verticalSpeed = 0;
		trans.localScale = trans.localScale.Y(1);
		state = PlayerState.Default;
		StopRocket();
	}

	private void StartRocket() {
		rocketTimer = RocketDuration;
		state = PlayerState.RocketBoosting;
		RocketDisplay?.SetActive(true);
	}
	
	private void StopRocket() {
		state = PlayerState.Default;
		RocketDisplay?.SetActive(false);
	}

	private void FixedUpdate() {
		//do the screen wrap
		var position = rigid.position;
		if (position.x < -screenWidth / 2) {
			position.x += screenWidth;
			trans.position = position;
		}
		if (position.x > screenWidth / 2) {
			position.x -= screenWidth;
			trans.position = position;
		}
		
		//burn out rocket
		if (state == PlayerState.RocketBoosting) {
			rocketTimer -= Time.fixedDeltaTime;
			if (rocketTimer <= 0)
				StopRocket();
		}

		var movement = Vector2.zero;
		//gravity
		if (state == PlayerState.RocketBoosting)
			fallSpeed = RocketSpeed;
		else
			fallSpeed += Gravity * Time.fixedDeltaTime;
		movement.y += fallSpeed;

		//input
		DoInput();
		movement.x += verticalSpeed;
		movement *= Time.fixedDeltaTime;

		//move
		if(state != PlayerState.Dead)
			Move(movement);
		else
			SimpleMove(movement); //no more collision when were dead
	}

	private void DoInput() {
		float input;
		if (state != PlayerState.Dead) {
			input = MoveKeys.action.ReadValue<float>();
			if (input == 0) { //if no key was pressed, check for touchscreen
				var touch = MoveTouch.action.ReadValue<TouchState>();
				if (touch.isPrimaryTouch) {
					var worldTarget = cam.ScreenToWorldPoint(touch.position);
					input = worldTarget.x - trans.position.x;
					//making the speed linearly slower within some world distance isnt super correct but works nicely
					input = Mathf.Clamp(input, -TouchBreakingZone, TouchBreakingZone) / TouchBreakingZone;
				}
			}
		} else
			input = 0;
		float targetVelocity = input * MoveSpeed;
		verticalSpeed = Mathf.MoveTowards(verticalSpeed, targetVelocity, Acceleration * Time.fixedDeltaTime);
	}

	private readonly List<RaycastHit2D> hits = new List<RaycastHit2D>();

	private void Move(Vector2 movement) {
		if (movement.x > 0)
			trans.localScale = trans.localScale.X(-1);
		if (movement.x < 0)
			trans.localScale = trans.localScale.X(1);
		if (state == PlayerState.RocketBoosting)
			RocketDisplay.transform.rotation = Quaternion.Euler(0, 0, -movement.x / Time.fixedDeltaTime * 4);

		var hitCount = rigid.Cast(movement, hits, movement.magnitude);
		var collision = hitCount > 0;
		var distance = 0f;
        
		//if we hit something, check if any of the hits makes us jump or kills us
		if (collision) {
			collision = false; //clear collision at first, and add it again when we find a valid collision
			for (int i = 0; i < hitCount; i++) {
				var hit = hits[i];
				var coll = hit.collider;
				//if we hit a hurt object, die
				if (coll.CompareTag("Hurt")) {
					Death();
					coll.SendMessage("JumpedOn", SendMessageOptions.DontRequireReceiver);
					return;
				}
				//start rocket phase when we touch a pickup
				if (coll.CompareTag("Pickup")) {
					StartRocket();
					coll.SendMessage("PickedUp", SendMessageOptions.DontRequireReceiver);
					continue; //dont collide with pickup
				}
				//if we hit the top side and we actually moved into the direction of the collision normal, its a real collision
				//this also triggers when inside a block which feels friendly to the player but we could also consider not doing that
				if (hit.normal.y > 0.5f && Vector2.Dot(movement, hit.normal) < 0) {
					collision = true;
					distance = hit.distance;
					var jumpSpeed = coll.CompareTag("Spring") ? SpringVelocity : JumpVelocity;
					fallSpeed = Mathf.Max(jumpSpeed, fallSpeed); //make player jump up again
					coll.SendMessage("JumpedOn", SendMessageOptions.DontRequireReceiver);
					break;
				}
			}
		}
		//no collision means regular movement
		if (!collision)
			SimpleMove(movement);
		//otherwise restrict movement to distance
		else
			SimpleMove(movement.normalized * distance);

	}

	private void SimpleMove(Vector2 movement) {
		//just do a relative MovePosition
		var position = rigid.position;
		position += movement; 
		rigid.MovePosition(position);
	}

	private void Death() {
		state = PlayerState.Dead;
		trans.localScale = trans.localScale.Y(-1); //flip player (colliders dont matter anymore)
		fallSpeed = DeathBopVelocity; //do the "mario death bop"
		GameOver?.Invoke();
	}
}