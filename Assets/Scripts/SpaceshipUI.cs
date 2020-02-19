using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipUI : MonoBehaviour
{

	[Header("Health/Shield Bar")]
	public Image ShieldBar;
	public Image ShieldBarFill;
	public Image HealthBar;
	public Image HealthBarFill;

	public Color GreenHealthBarColor = new Color(45,118,13);
	public Color GreenHealthBarFillColor = new Color(132,243,103);
	public Color RedHealthBarColor = new Color(118,17,13);
	public Color RedHealthBarFillColor = new Color(243,106,104);

	[Header("Ammo & Weapons")]
	public Image LaserCircle;
	public Text LaserAmmoText;
	public Image BulletCircle;
	public Text BulletAmmoText;
	public Image NovaCircle;
	public Text NovaAmmoText;

	public Color SelectedWeaponColor = new Color(243, 243, 243);
	public Color NotSelectedWeaponColor = new Color(55, 55, 63);

	private ShipStatus status;
	private ShipControllerScript controller;
	void Start() {
		status = GetComponent<ShipStatus>();
		controller = GetComponent<ShipControllerScript>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnGUI() {
		// Health and Shield
		if (ShieldBarFill != null) {
			ShieldBarFill.fillAmount = (float)status.CurrentShield / status.MaxShield;
		}
		if (HealthBarFill != null) {
			HealthBarFill.fillAmount = (float)status.CurrentHealth / status.MaxHealth;
			HealthBarFill.color = (HealthBarFill.fillAmount < 0.25f) ? (RedHealthBarFillColor) : (GreenHealthBarFillColor);
		}
		if (HealthBar != null) {
			HealthBar.color = (HealthBarFill.fillAmount < 0.25f) ? (RedHealthBarColor) : (GreenHealthBarColor);
		}

		// Weapons and ammo
		switch(controller.CurrentShootType) {
			case ShootType.Laser:
				LaserCircle.color = SelectedWeaponColor;
				BulletCircle.color = NotSelectedWeaponColor;
				NovaCircle.color = NotSelectedWeaponColor;
				break;
			case ShootType.Bullet:
				LaserCircle.color = NotSelectedWeaponColor;
				BulletCircle.color = SelectedWeaponColor;
				NovaCircle.color = NotSelectedWeaponColor;
				break;
			case ShootType.Nova:
				LaserCircle.color = NotSelectedWeaponColor;
				BulletCircle.color = NotSelectedWeaponColor;
				NovaCircle.color = SelectedWeaponColor;
				break;
		}
		BulletAmmoText.text = status.CurrentAmmoBullets.ToString();
		NovaAmmoText.text = status.CurrentAmmoNova.ToString();
	}
}
