using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControllerScript : MonoBehaviour {
	[SerializeField] float moveSpeed = 1f;
	[SerializeField] float turnSpeed = 1f;
	[SerializeField] float rollSpeed = 1f;
	[SerializeField] bool invertY = false;

	Rigidbody body;

	public Transform LaserShootPositionLeft;
	public Transform LaserShootPositionRight;
	public GameObject LaserShoot;

	public float FireRate = 0.3f;
	private float lastFire = 0f;

	void Awake() {
		body = GetComponent<Rigidbody>();
	}

	void OnEnable() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		lastFire += Time.deltaTime;
		if (Input.GetButton("Fire1") && lastFire >= FireRate) 
		{
			lastFire = 0f;
			GameObject.Instantiate(LaserShoot, LaserShootPositionLeft.position, LaserShootPositionLeft.rotation);
			GameObject.Instantiate(LaserShoot, LaserShootPositionRight.position, LaserShootPositionRight.rotation);
		}
	}

	void FixedUpdate() {
		Vector3 direction = GetDirection();
		body.AddRelativeTorque(GetRotation() * turnSpeed * Time.deltaTime, ForceMode.VelocityChange);
		body.AddRelativeForce(direction * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

		Debug.Log(body.velocity);
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
		return new Vector3(pitch * turnSpeed, yaw * turnSpeed, roll * rollSpeed);
	}
}