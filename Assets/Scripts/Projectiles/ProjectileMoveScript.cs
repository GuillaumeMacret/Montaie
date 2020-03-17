using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour {

	public float speed;
	public int damageDealt;
	[Tooltip("From 0% to 100%")]
	public float accuracy;
	public float TimeBeforeVanish = 1.5f;
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;
	public GameObject explosionPrefab;
	public List<GameObject> trails;

	private float speedRandomness;
	private Vector3 offset;
	private bool collided;
	private Rigidbody rb;

	void Start () {	
		rb = GetComponent <Rigidbody> ();

		//used to create a radius for the accuracy and have a very unique randomness
		if (accuracy != 100) {
			accuracy = 1 - (accuracy / 100);

			for (int i = 0; i < 2; i++) {
				float val = 1 * Random.Range (-accuracy, accuracy);
				int index = Random.Range (0, 2);
				if (i == 0) {
					if (index == 0)
						offset = new Vector3 (0, -val, 0);
					else
						offset = new Vector3 (0, val, 0);
				} else {
					if (index == 0)
						offset = new Vector3 (0, offset.y, -val);
					else
						offset = new Vector3 (0, offset.y, val);
				}
			}
			Destroy(gameObject, TimeBeforeVanish);
		}
			
		if (muzzlePrefab != null) {
			GameObject muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward + offset;
			ParticleSystem ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				ParticleSystem psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}
	}

	void FixedUpdate () {	
		if (speed != 0 && rb != null)
			rb.position += (transform.forward + offset)  * (speed * Time.deltaTime);
	}

	void OnCollisionEnter (Collision co) {
		if (co.gameObject.tag != "Bullet" && !collided) {
			collided = true;
			if (co.gameObject.tag == "Enemy") {
				EnemyStatus enemyStatus = co.gameObject.GetComponent<EnemyStatus>();
				if (enemyStatus != null)
					enemyStatus.TakeDamage(damageDealt);
			}
			if (co.gameObject.tag == "Player") {
				ShipStatus shipStatus = co.gameObject.GetComponent<ShipStatus>();
				if (shipStatus != null)
					shipStatus.TakeDamage(damageDealt);
			}
			if (trails.Count > 0) {
				for (int i = 0; i < trails.Count; i++) {
					trails [i].transform.parent = null;
					ParticleSystem ps = trails [i].GetComponent<ParticleSystem> ();
					if (ps != null) {
						ps.Stop ();
						Destroy (ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
					}
				}
			}
			
			speed = 0;
			GetComponent<Rigidbody> ().isKinematic = true;

			ContactPoint contact = co.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;

			if(explosionPrefab != null && (co.gameObject.tag == "Enemy" || co.gameObject.tag == "Player") && explosionPrefab != null) {
				GameObject explosionVFX = Instantiate(explosionPrefab, pos, rot);
				Destroy(explosionVFX, 1.5f);
			}
			else if (hitPrefab != null) {
				GameObject hitVFX = Instantiate (hitPrefab, pos, rot) as GameObject;

				ParticleSystem ps = hitVFX.GetComponent<ParticleSystem> ();
				if (ps == null) {
					ParticleSystem psChild = hitVFX.transform.GetChild (0).GetComponent<ParticleSystem> ();
					Destroy (hitVFX, psChild.main.duration);
				} else
					Destroy (hitVFX, ps.main.duration);
			}

			StartCoroutine (DestroyParticle (0f));
		}
	}

	public IEnumerator DestroyParticle (float waitTime) {
		/*
		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}*/
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
