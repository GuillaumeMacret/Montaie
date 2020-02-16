using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStatus : MonoBehaviour
{
	[Header("Status Settings")]
	public int MaxShield = 50;
	[System.NonSerialized]
	public int CurrentShield;

	public int MaxHealth = 100;
	[System.NonSerialized]
	public int CurrentHealth;

	[Header("Shield Settings")]
	public int ShieldRegenPerTick = 3;
	public float SecondsBeforeShieldRegen = 3f;
	public float ShieldRegenTickTime = 0.5f;

	[Header("Pickup Settings")]
	public int HealthRestoredOnPickup = 25;

	private float lastShieldRegenTick = 0f;
	private float lastHitReceived;

	private ShipSoundScript shipSound;

	// Start is called before the first frame update
	void Start() {
		CurrentHealth = MaxHealth;
		CurrentShield = 25;
		lastHitReceived = 0f;
		shipSound = GetComponent<ShipSoundScript>();
	}

    // Update is called once per frame
    void Update() {
		lastHitReceived += Time.deltaTime;
        if(lastHitReceived >= SecondsBeforeShieldRegen) {
			lastShieldRegenTick += Time.deltaTime;
			if(lastShieldRegenTick >= ShieldRegenTickTime) {
				CurrentShield = Mathf.Clamp(CurrentShield + ShieldRegenPerTick, 0, MaxShield);
				lastShieldRegenTick = 0f;
			}
		}
    }

	public void TakeDamage(int amount) {
		lastHitReceived = 0f;
		if (CurrentShield > 0) {
			CurrentShield = Mathf.Clamp(CurrentShield - amount, 0, MaxShield);
			if (CurrentShield == 0)
				shipSound.PlayShieldBroken();
		}
		else {
			CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
			if (CurrentHealth <= MaxHealth * 0.25)
				shipSound.PlayCriticalHealth();
		}
		if(CurrentHealth == 0) {
			//GAMEOVER
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Health Pickup") {
			Destroy(other.gameObject);
			CurrentHealth = Mathf.Clamp(CurrentHealth + HealthRestoredOnPickup, 0, MaxHealth);
			shipSound.PlayHealthPickup();
		}
	}
}
