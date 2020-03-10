using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionPortalTrigger : MonoBehaviour
{
	private MeshRenderer meshRenderer;
	private OcclusionPortal occlusionPortal;
    
	// Start is called before the first frame update
    void Start()
    {
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.enabled = false;
		occlusionPortal = GetComponent<OcclusionPortal>();
		occlusionPortal.open = true;
    }

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {
			Debug.Log("Closed");
			occlusionPortal.open = false;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log("Opened");
			occlusionPortal.open = true;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
