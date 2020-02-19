using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSoundScript : MonoBehaviour {
	[Header("Audio Sources")]
	public AudioSource ShipAudio;
	public AudioSource EngineAudio;

	[Header("Status Sounds")]
	public AudioClip ShieldBroken;
	public AudioClip CriticalHealth;
	public AudioClip NoAmmo;
	public AudioClip WeaponSwitched;

	[Header("Pickup Sounds")]
	public AudioClip WeaponPickup;
	public AudioClip HealthPickup;
	
	private bool engineOn = false;	

	public void SetEngineVolume(float vol) {
		EngineAudio.volume = vol;
	}

	public bool IsEnginePlaying() {
		return engineOn;
	}

	public void StartEngine() {
		engineOn = true;
	}

	public void StopEngine() {
		engineOn = false;
	}

	public void PlayShieldBroken() {
		ShipAudio.PlayOneShot(ShieldBroken);
	}
	public void PlayCriticalHealth() {
		ShipAudio.PlayOneShot(CriticalHealth, 0.3f);
	}
	public void PlayWeaponPickup() {
		ShipAudio.PlayOneShot(WeaponPickup);
	}
	public void PlayHealthPickup() {
		ShipAudio.PlayOneShot(HealthPickup);
	}
	public void PlayNoAmmo() {
		ShipAudio.PlayOneShot(NoAmmo);
	}
	public void PlayWeaponSwitched() {
		ShipAudio.PlayOneShot(WeaponSwitched);
	}

	private void Start() {
		EngineAudio.volume = 0f; 
		EngineAudio.Play();
	}

	private void Update() {
		if (engineOn)
			EngineAudio.volume = Mathf.Clamp(EngineAudio.volume + Time.deltaTime, 0f, 1f);
		else
			EngineAudio.volume = Mathf.Clamp(EngineAudio.volume - Time.deltaTime, 0f, 1f);
	}
}
