using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : MonoBehaviour
{
	[SerializeField] float speed = 100f;
	public GameObject Explosion;
	private AudioSource audioSource;
	private bool isDestroyed = false;

	void Start() {
		Destroy(this.gameObject, 1.5f);
	}

    // Update is called once per frame
    void Update() {
		if(!isDestroyed)
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PlayerProjectile" || other.gameObject.tag == "Player")
			return;
		isDestroyed = true;
		GetComponent<BoxCollider>().enabled = false;
		GetComponentInChildren<TrailRenderer>().enabled = false;
		GetComponentInChildren<ParticleSystem>().Clear(true);

		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
		}
		GameObject explosion = Instantiate(Explosion, this.transform);
		Destroy(explosion, 1.5f);
	}
}
