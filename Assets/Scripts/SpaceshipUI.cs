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

	private ShipStatus status;

	void Start() {
		status = GetComponent<ShipStatus>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnGUI() {
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
	}
}
