using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    public float waitTime = 0.2f;

	// Use this for initialization
	void Start () {
        StartCoroutine(stopLight());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator stopLight()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Light>().enabled = false;

    }
}
