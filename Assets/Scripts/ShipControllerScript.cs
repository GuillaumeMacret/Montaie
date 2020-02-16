using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class ShipControllerScript : MonoBehaviour {

	[Header("Movement Settings")]
	[SerializeField] float moveSpeed = 1f;
	[SerializeField] float turnSpeed = 1f;
	[SerializeField] float rollSpeed = 1f;
	[SerializeField] bool invertY = false;

	Rigidbody body;

	[Header("Laser Settings")]
	public Transform LaserShootPositionLeft;
	public Transform LaserShootPositionRight;
	public GameObject LaserShoot;

	public float FireRate = 0.3f;

	[Header("Laser Audio Sources")]
	public AudioSource LaserAudioSourceLeft;
	public AudioSource LaserAudioSourceRight;

	[Header("Lever Handle Settings")]
	public Transform LeverHandle;
	public float HandleRotationDamping = 10f;

	private float lastRotation = 0f;
	private float rotationResetTreshhold = 0.3f;
	private Vector3 handleNewRotation;
	private Quaternion handleOriginalRotation;

	private ShipSoundScript soundScript;

	private float lastFire = 0f;

	void Awake() {
		body = GetComponent<Rigidbody>();
		soundScript = GetComponent<ShipSoundScript>();
	}

	void Start() {
		handleOriginalRotation = LeverHandle.localRotation;	
	}

	void OnEnable() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		lastFire += Time.deltaTime;
		if (Input.GetButton("Fire1") && lastFire >= FireRate) {
			lastFire = 0f;
			GameObject.Instantiate(LaserShoot, LaserShootPositionLeft.position, LaserShootPositionLeft.rotation);
			GameObject.Instantiate(LaserShoot, LaserShootPositionRight.position, LaserShootPositionRight.rotation);
			if (LaserAudioSourceLeft != null)
				LaserAudioSourceLeft.Play();
			if (LaserAudioSourceRight != null)
				LaserAudioSourceRight.Play();
		}
	}

	void FixedUpdate() {
		Vector3 direction = GetDirection();
		Vector3 rotation = GetRotation();
		lastRotation += Time.deltaTime;
		if(LeverHandle != null) {
			Quaternion target;
			if (rotation == Vector3.zero && lastRotation >= rotationResetTreshhold) {
				target = Quaternion.FromToRotation(LeverHandle.localRotation.eulerAngles, handleOriginalRotation.eulerAngles);
			}
			else {
				lastRotation = 0f;
				target = Quaternion.Euler(handleNewRotation);
			}
			target.x = Mathf.Clamp(target.x, -5f, 5f);
			target.y = Mathf.Clamp(target.y, -5f, 5f);
			target.z = Mathf.Clamp(target.z, -5f, 5f);
			LeverHandle.localRotation = Quaternion.Lerp(LeverHandle.localRotation, target, Time.deltaTime * HandleRotationDamping);
		}
		if (soundScript.IsEnginePlaying() && direction == Vector3.zero && rotation == Vector3.zero) {
			soundScript.StopEngine();
		}
		else if (!soundScript.IsEnginePlaying() && (direction != Vector3.zero || rotation != Vector3.zero)) {
			soundScript.StartEngine();
		}
		body.AddRelativeTorque(GetRotation() * turnSpeed * Time.deltaTime, ForceMode.VelocityChange);
		body.AddRelativeForce(direction * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}

	Vector3 GetDirection() {
		Vector3 dir = new Vector3();
		dir += Vector3.forward * Input.GetAxis("Power");
		dir += Vector3.right * Input.GetAxis("Horizontal");
		dir += Vector3.up * Input.GetAxis("Vertical");
		return dir;
	}

	Vector3 GetRotation() {
		float yaw = Input.GetAxis("Yaw");
		float pitch = Input.GetAxis("Pitch") * (invertY ? 1 : -1);
		float roll = 0;
		if (Input.GetAxis("Roll Left") > 0) roll += 1;
		if (Input.GetAxis("Roll Right") > 0) roll -= 1;
		handleNewRotation = new Vector3(pitch * 50, yaw * 70, roll * 15);
		return new Vector3(pitch * turnSpeed, yaw * turnSpeed, roll * rollSpeed);
	}
}