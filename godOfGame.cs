using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class godOfGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator playerDead()
    {

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        // ... reload the level.
        StartCoroutine("ReloadGame");


    }

    IEnumerator ReloadGame()
    {
        // ... pause briefly
        yield return new WaitForSeconds(2);
        // ... and then reload the level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }


}
