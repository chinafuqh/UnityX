using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {


    public newPlayer newplayer;
    private Animator anim;
    private bool isdead = false;

    // Use this for initialization
    void Start () {
        newplayer = GetComponent<newPlayer>();
        anim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void beDeath()
    {

        if (GetComponent<newPlayer>() != null)
        {
            newplayer.enabled = false;
            if (GetComponent<newPlayer>().isTransform)
            {

                anim.SetTrigger("transformDie");
               

            }
            else
            {
                anim.SetTrigger("Die");
            }
            GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(GameObject.Find("god").GetComponent<godOfGame>().playerDead());
        }
        else
        {
            anim.SetTrigger("Die");

        }

    }




    public void DestroySelf()
    {

        Destroy(gameObject,1f);

    }


    public void untouchble()
    {

        this.gameObject.layer = 13;

    }
}
