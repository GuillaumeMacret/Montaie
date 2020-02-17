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

	[Header("Ammunition Settings")]
	public int StartingAmmoBullets = 100;
	public int StartingAmmoNova = 2;
	[System.NonSerialized]
	public int CurrentAmmoBullets;
	[System.NonSerialized]
	public int CurrentAmmoNova;

	private float lastShieldRegenTick = 0f;
	private float lastHitReceived;

	private ShipSoundScript shipSound;

	// Start is called before the first frame update
	void Start() {
		CurrentHealth = MaxHealth;
		CurrentShield = MaxShield;
		lastHitReceived = 0f;
		CurrentAmmoBullets = StartingAmmoBullets;
		CurrentAmmoNova = StartingAmmoNova;
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

	public bool HasAmmo(ShootType st) {
		switch(st) {
			case ShootType.Bullet:
				return CurrentAmmoBullets > 0;
			case ShootType.Nova:
				return CurrentAmmoNova > 0;
			default:
				return true;
		}
	}

	public void DescreaseAmmo(ShootType st) {
		switch (st) {
			case ShootType.Bullet:
				CurrentAmmoBullets--;
				break;
			case ShootType.Nova:
				CurrentAmmoNova--;
				break;
			default:
				break;
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
