using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
	[Header("Health Settings")]
	public int MaxHealth = 50;
	[System.NonSerialized]
	public int CurrentHealth;

	[Header("When dying")]
	public GameObject Explosion;
	public float TimeToDisappear = 1f;

	private Material material;
	private List<Renderer> renderers;
	private float timeSinceDead = 0f;
	private bool isDead = false;

	public void TakeDamage(int amount) {
		CurrentHealth -= amount;
		if (CurrentHealth < 0)
			Die();
	}

	void Die() {
		isDead = true;
		if (Explosion != null)
			Instantiate(Explosion, transform);
		List<Collider> colliders = new List<Collider>();
		GetComponentsInChildren(colliders);
		foreach(Collider c in colliders)
			c.enabled = false;
		Destroy(gameObject, TimeToDisappear);
	}

    // Start is called before the first frame update
    void Start() {
		CurrentHealth = MaxHealth;
		renderers = new List<Renderer>();
		GetComponentsInChildren(renderers);
		//material = GetComponent<Renderer>().material;
	}

    // Update is called once per frame
    void Update() {
        if(isDead) {
			timeSinceDead += Time.deltaTime;
			//material.SetFloat("_DissolveAmount", timeSinceDead / TimeToDisappear);
			if (renderers != null) {
				foreach (Renderer renderer in renderers) {
					foreach (Material mat in renderer.materials)
						mat.SetFloat("_DissolveAmount", timeSinceDead / TimeToDisappear);
				}
			}
		}
    }
}
