using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public Transform LaserShoot,LaserShootPosition;
    public AudioSource LaserAudioSource;

    public float fireCooldown = .5f;
	private float lastFireCooldownCpt = 0f;

    void Update() {
		if(lastFireCooldownCpt > 0)lastFireCooldownCpt -= Time.deltaTime;
		if (Input.GetButton("Fire1") && lastFireCooldownCpt <= 0)
		{
			lastFireCooldownCpt = fireCooldown;
			GameObject.Instantiate(LaserShoot, LaserShootPosition.position, LaserShootPosition.rotation);
			if (LaserAudioSource != null)
				LaserAudioSource.Play();
		}
	}
}
