using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBloodBar : MonoBehaviour {
    public GameObject bloodbar, bloodbar2;
    [HideInInspector]
    public bool startdamage = false;
    private float x = 0.05f; //体力条缩减速度
    private SpriteRenderer healthBar, healthBar2;
    private Vector3  healthScale, healthScale2;
    [HideInInspector]
    public bool canshow = false;

    // Use this for initialization
    void Start () {
        healthBar = bloodbar.GetComponent<SpriteRenderer>();
        healthBar2 = bloodbar2.GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
        healthScale2 = healthBar2.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {


        if (startdamage)
        {
            if (healthBar2.transform.localScale.x > healthBar.transform.localScale.x)
            {
                healthBar2.transform.localScale = new Vector3(healthBar.transform.localScale.x - x, 1, 1);

            }
            else
                startdamage = false;
        }

    }


    public IEnumerator UpdateHealthBar(float HpPersent)
    {
        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        //healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HPthreata * 0.01f);
        // Set the scale of the health bar to be proportional to the player's health.
        bloodbar.transform.localScale = new Vector3(Mathf.Max(healthScale.x * HpPersent,0), 1, 1);
        yield return new WaitForSeconds(1f);
        //healthBar2.transform.localScale = new Vector3(healthScale.x * (HPthreata /HP), 2, 1);
        startdamage = true;
    }

    public void showself()
    {
       var allObjct = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in allObjct )
        {

            s.enabled = true;


        }


    }


}
