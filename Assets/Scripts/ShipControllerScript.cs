using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ShootType { Laser, Bullet, Nova };

[RequireComponent(typeof(Rigidbody))]
public class ShipControllerScript : MonoBehaviour {

	[Header("Movement Settings")]
	[SerializeField] float moveSpeed = 1f;
	[SerializeField] float turnSpeed = 1f;
	[SerializeField] float rollSpeed = 1f;
	[SerializeField] bool invertY = false;

	Rigidbody body;

	[Header("Shoot Settings")]
	public Transform ShootPositionLeft;
	public Transform ShootPositionRight;

	public GameObject LaserShoot;
	public GameObject BulletShoot;
	public GameObject NovaShoot;

	public float FireRateLaser = 0.3f;
	public float FireRateBullets = 0.05f;
	public float FireRateNova = 2f;

	private float timeBetweenWeaponSwitch = 0.5f;
	private float lastWeaponSwitch;
	private float currentFireRate;
	[System.NonSerialized]
	public ShootType CurrentShootType;

	[Header("Shoot Audio Sources & Clips")]
	public AudioSource ShootAudioSourceLeft;
	public AudioSource ShootAudioSourceRight;
	public AudioClip LaserShootClip;
	public AudioClip BulletShootClip;
	public AudioClip NovaShootClip;

	[Header("Lever Handle Settings")]
	public Transform LeverHandle;
	public float HandleRotationDamping = 10f;

	private float lastRotation = 0f;
	private float rotationResetTreshhold = 0.3f;
	private Vector3 handleNewRotation;
	private Quaternion handleOriginalRotation;

	private ShipSoundScript soundScript;
	private ShipStatus shipStatus;

	private float lastFire = 0f;

	void Awake() {
		body = GetComponent<Rigidbody>();
		soundScript = GetComponent<ShipSoundScript>();
		shipStatus = GetComponent<ShipStatus>();
	}

	void Start() {
		handleOriginalRotation = LeverHandle.localRotation;
		CurrentShootType = ShootType.Laser;
		currentFireRate = FireRateLaser;
		lastWeaponSwitch = 0f;
	}

	void OnEnable() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		// Change weapon
		lastWeaponSwitch += Time.deltaTime;
		if (Input.GetButton("Fire2") && lastWeaponSwitch > timeBetweenWeaponSwitch)
			SwitchWeapon();
		// Shooting process
		lastFire += Time.deltaTime;
		if (lastFire >= currentFireRate) {
			if (Input.GetButton("Fire1")) {
				lastFire = 0f;
				if(shipStatus.HasAmmo(CurrentShootType)) {
					AudioClip currentShootClip;
					GameObject shootToFire;
					switch (CurrentShootType) {
						case ShootType.Laser:
							shootToFire = LaserShoot;
							currentShootClip = LaserShootClip;
							break;
						case ShootType.Bullet:
							shootToFire = BulletShoot;
							currentShootClip = BulletShootClip;
							break;
						case ShootType.Nova:
							shootToFire = NovaShoot;
							currentShootClip = NovaShootClip;
							break;
						default:
							shootToFire = LaserShoot;
							currentShootClip = LaserShootClip;
							break;
					}
                    shootToFire.transform.localScale = transform.localScale;

                    GameObject shootLeft = GameObject.Instantiate(shootToFire, ShootPositionLeft.position, ShootPositionLeft.rotation);
                    //shootLeft.transform.localScale = transform.localScale;

					GameObject shootRight = GameObject.Instantiate(shootToFire, ShootPositionRight.position, ShootPositionRight.rotation);
                    //shootRight.transform.localScale = transform.localScale;

					if (ShootAudioSourceLeft != null)
						ShootAudioSourceLeft.PlayOneShot(currentShootClip, 0.3f);
					if (ShootAudioSourceRight != null)
						ShootAudioSourceRight.PlayOneShot(currentShootClip, 0.3f);
					shipStatus.DescreaseAmmo(CurrentShootType);
				}
				else {
					soundScript.PlayNoAmmo();
				}
			}
		}
	}

	void SwitchWeapon() {
		switch(CurrentShootType) {
			case ShootType.Laser:
				CurrentShootType = ShootType.Bullet;
				currentFireRate = FireRateBullets;
				break;
			case ShootType.Bullet:
				CurrentShootType = ShootType.Nova;
				currentFireRate = FireRateNova;
				break;
			case ShootType.Nova:
				CurrentShootType = ShootType.Laser;
				currentFireRate = FireRateLaser;
				break;
		}
		lastWeaponSwitch = 0f;
		soundScript.PlayWeaponSwitched();
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