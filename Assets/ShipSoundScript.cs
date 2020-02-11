using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSoundScript : MonoBehaviour
{
	public AudioSource EngineStart;

	private bool engineOn = false;	

	public void SetEngineVolume(float vol) {
		EngineStart.volume = vol;
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

	private void Start() {
		EngineStart.volume = 0f; 
		EngineStart.Play();
	}

	private void Update() {
		Debug.Log(EngineStart.volume);
		if (engineOn)
			EngineStart.volume = Mathf.Clamp(EngineStart.volume + Time.deltaTime, 0f, 1f);
		else
			EngineStart.volume = Mathf.Clamp(EngineStart.volume - Time.deltaTime, 0f, 1f);
	}
}
