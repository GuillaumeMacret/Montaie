using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionPortalTrigger : MonoBehaviour
{
	private List<MeshRenderer> meshRenderers;
	private List<OcclusionPortal> occlusionPortals;
    
	// Start is called before the first frame update
    void Start()
    {
		meshRenderers = new List<MeshRenderer>();
		GetComponentsInChildren(meshRenderers);
		foreach(MeshRenderer meshRenderer in meshRenderers)
			meshRenderer.enabled = false;
		occlusionPortals = new List<OcclusionPortal>();
		GetComponentsInChildren(occlusionPortals);
		foreach(OcclusionPortal occlusionPortal in occlusionPortals)
			occlusionPortal.open = true;
    }

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			foreach (OcclusionPortal occlusionPortal in occlusionPortals)
				occlusionPortal.open = false;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			foreach (OcclusionPortal occlusionPortal in occlusionPortals)
				occlusionPortal.open = true;
		}
	}
}
