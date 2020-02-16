using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnProjectilesScript : MonoBehaviour {
	public Text effectName;
	public List<GameObject> VFXs = new List<GameObject> ();

	private int count = 0;
	private GameObject effectToSpawn;

	void Start () {
		if(VFXs.Count>0)
			effectToSpawn = VFXs[0];
		else
			Debug.Log ("Please assign one or more VFXs in inspector");
		
		if (effectName != null)
			effectName.text = effectToSpawn.name;
		else
			Debug.Log ("Please assign one or more Cameras in inspector");
		SpawnVFX();
	}

	public void SpawnVFX () {
		GameObject vfx;
		vfx = Instantiate (effectToSpawn);

		var ps = vfx.GetComponent<ParticleSystem> ();

		if (vfx.transform.childCount > 0) {
			ps = vfx.transform.GetChild (0).GetComponent<ParticleSystem> ();
		}
	}

	public void Next () {
		count++;

		if (count > VFXs.Count)
			count = 0;

		for(int i = 0; i < VFXs.Count; i++){
			if (count == i)	effectToSpawn = VFXs [i];
			if (effectName != null)	effectName.text = effectToSpawn.name;
		}
	}

	public void Previous () {
		count--;

		if (count < 0)
			count = VFXs.Count;

		for (int i = 0; i < VFXs.Count; i++) {
			if (count == i) effectToSpawn = VFXs [i];
			if (effectName != null)	effectName.text = effectToSpawn.name;
		}
	}
}
