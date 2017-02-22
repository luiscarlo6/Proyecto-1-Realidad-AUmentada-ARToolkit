using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        //Physics.gravity = new Vector3(-5*transform.rotation.y/90, 0.0F, transform.rotation.x / 90);
        Vector3 ang = transform.localRotation.eulerAngles;
        
        Physics.gravity = new Vector3(-(ang.y - 180) / 2000, -0.1F, (ang.x - 180)/2000);
    }
}
