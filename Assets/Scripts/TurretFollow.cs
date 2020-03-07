using UnityEngine;

public class TurretFollow : MonoBehaviour {
	public Transform Target;
	public Transform RotatingPart;
	public float RotationDamping = 6.0f;
	public float FireRate = 1.5f;
	public GameObject Projectile;
	public Transform ProjectileStartPosition;

	private float lastFire = 0f;
	private bool isActive = false;

	void Awake() {

	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (isActive) { 
			lastFire += Time.deltaTime;
			if (lastFire > FireRate) {
				Fire();
			}
		}
	}

	void Fire() {
		lastFire = 0f;
		if(Projectile != null) {
			Instantiate(Projectile, ProjectileStartPosition.position, ProjectileStartPosition.rotation);
			AudioSource audioSource = GetComponent<AudioSource>();
			if (audioSource != null)
				audioSource.Play();
		}
	}

	void LateUpdate() {
		if (isActive && Target && RotatingPart) {
			Vector3 targetPos = Target.position;
			RotatingPart.transform.rotation = Quaternion.RotateTowards(RotatingPart.transform.rotation, Quaternion.LookRotation(targetPos - RotatingPart.transform.position), Time.deltaTime * RotationDamping);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
			isActive = true;

	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player")
			isActive = false;
	}
}