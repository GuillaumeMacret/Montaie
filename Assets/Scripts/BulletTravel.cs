using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTravel : MonoBehaviour
{
	[SerializeField] float speed = 100f;

	void Start() {
		Destroy(this.gameObject, 2f);
	}

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
