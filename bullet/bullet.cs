using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Destroy(gameObject,2f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            StartCoroutine(collision.GetComponent<getHurt>().Hurt(gameObject, 0.3f, 10, 10, 12));

            Destroy(gameObject, 2);
            transform.position = new Vector3(-1000, -1000, 0);
        }


    }


}
