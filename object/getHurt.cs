using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class getHurt : MonoBehaviour
{



    public float physicalResistance = 0;//打击抗性，决定定帧时间和击退的削减
    public float HP = 100;                //HP
    public float SP = 50;               //耐力 攻击和回避会消耗耐力
    public float SPrecover = 0.5f;
    public Color hitMaterial;
    public Color defaultMaterial;
    public Shader hitShader;
    public Shader defaultShader;
    public GameObject hiteffect;
    public GameObject enemyBloodbar;
    public Animator animSelf;
    public GameObject damageShow;
    private Transform[] allObjct;
    private Transform[] allObjctAnim;
    private GameObject senderSave;
    private float forceSave;
    private Transform hitpiont;
    private float damageamount;
    [HideInInspector]
    public float HPthreata;
    [HideInInspector]
    public float SPthreata;
    private SpriteRenderer healthBar;
    private SpriteRenderer healthBar2;
    private SpriteRenderer staBar;
    private Vector3 healthScale;
    private Vector3 healthScale2;
    private Vector3 staScale;
    private bool startdamage = false;
    private float starttime = 0;
    private float x = 0.03f; //体力条缩减速度
    private bool stopstarecover = false;
    [HideInInspector]
    public bool isdead = false;
    private MeshRenderer meshrender;
    public bool befucked = false;

    private float defaultRenxing = 10;//默认韧性
    [HideInInspector]
    public float Renxingshiyong = 10;
    private float Renxing = 10;





    private void Awake()
    {
        HPthreata = HP;
        SPthreata = SP;
        healthBar = GameObject.Find("bloodbargreen").GetComponent<SpriteRenderer>();
        healthBar2 = GameObject.Find("bloodbarred").GetComponent<SpriteRenderer>();
        staBar = GameObject.Find("staminabargreen").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
        healthScale2 = healthBar2.transform.localScale;
        staScale = staBar.transform.localScale;
        meshrender = GetComponent<MeshRenderer>();

    }

    // Use this for initialization
    void Start()
    {
        animSelf = GetComponent<Animator>();
        hitpiont = transform.Find("hitpiont");
        starttime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (startdamage)
        {
            if (healthBar2.transform.localScale.x > healthBar.transform.localScale.x)
            {
                healthBar2.transform.localScale = new Vector3(healthBar2.transform.localScale.x - x, 2, 1);

            }
            else
                startdamage = false;
        }




    }

    private void FixedUpdate()
    {
        if (HPthreata <= 0 && !isdead)
        {
            //gameObject.GetComponent<Death>().beDeath();
            isdead = true;
            animSelf.SetTrigger("dead");
        }

        if (SPthreata < SP && !stopstarecover)
        {
            SPthreata = SPthreata + Time.deltaTime * SPrecover;
            float X = staBar.transform.localScale.x + staScale.x * (Time.deltaTime * SPrecover / SP);
            if (SPthreata < 0)
            {
                staBar.transform.localScale = new Vector3(0, 2, 1);

            }
            else
            {
                staBar.transform.localScale = new Vector3(X, 2, 1);
            }

        }


    }


    //受伤函数执行被击动作，减少HP，改变自身材质
    public IEnumerator Hurt(GameObject sender, float fzTime, float damageAmount, float force, float xiaoren)
    {
        hitanim(xiaoren);
        damageamount = damageAmount;
        HPthreata = HPthreata - damageamount;


        if (this.tag=="Player")
        {
            StartCoroutine(UpdateHealthBar());
        }


        if (this.tag == "Enemy")
        {
            if(!enemyBloodbar.GetComponent<enemyBloodBar>().canshow)
            {
                enemyBloodbar.GetComponent<enemyBloodBar>().showself();
                enemyBloodbar.GetComponent<enemyBloodBar>().canshow = true;
            }
            StartCoroutine(enemyBloodbar.GetComponent<enemyBloodBar>().UpdateHealthBar(HPthreata/HP));
        }


        hitEffect();
        DamageShow();

        if (sender != null)
        {
            senderSave = sender;
            forceSave = force;
            StartCoroutine(freezeTime(sender, fzTime));
        }


        var materiallist =   gameObject.GetComponent<MeshRenderer>().materials;
        foreach (Material M in materiallist)
        {
            M.shader = hitShader;
            M.color = hitMaterial;
        }

        yield return new WaitForSeconds(Mathf.Max((fzTime - physicalResistance), 0));


        foreach (Material M in materiallist)
        {
            M.shader = defaultShader;
            M.color = defaultMaterial;
        }


           // gameObject.GetComponent<MeshRenderer>().material.shader = defaultShader;
           // gameObject.GetComponent<MeshRenderer>().material.color = defaultMaterial;


    }


    public IEnumerator freezeTime(GameObject sender_1, float fzTime)
    {
        Animator anim = sender_1.GetComponent<Animator>();
        if (anim != null)
        anim.speed = 0;
        allObjctAnim = sender_1.GetComponentsInChildren<Transform>();
        animSelf.speed = 0;
        setChildSpeed(allObjctAnim, 0);


        yield return new WaitForSeconds(Mathf.Max((fzTime - physicalResistance), 0));
        if (anim != null)
            anim.speed = 1;
        animSelf.speed = 1;
        setChildSpeed(allObjctAnim, 1);
        hurtForce();
    }




    public void setChildSpeed(Transform[] childAnim, float animspeed)
    {

        foreach (Transform child in childAnim)
        {

            if (child.gameObject.GetComponent<Transform>() != null && child.gameObject.GetComponent<Animator>() != null)
            {

                Animator animChild = this.GetComponent<Animator>();
                animChild = child.gameObject.GetComponent<Animator>();
                animChild.speed = animspeed;
            }
        }

    }

    //施加力
    public void hurtForce()
    {
        if (HP > 0 && senderSave != null)
        {
            Vector3 deltaPos = transform.position - senderSave.transform.position;

            // Apply a force in this direction with a magnitude of bombForce.
            Vector3 addforce = deltaPos.normalized * forceSave;
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(addforce);
        }


    }
    //创建被击特效
    public void hitEffect()
    {
        Vector3 hitpos = new Vector3(hitpiont.transform.position.x, hitpiont.transform.position.y, hitpiont.transform.position.z);
        GameObject prefabInstance = Instantiate(hiteffect, hitpos, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        //prefabInstance.transform.parent = this.transform;

    }

    protected void hitanim(float xiaoren)
    {
        if (HPthreata > 0)
        {
            if (this.tag == "Player")
            {
                float renxing = Renxingshiyong;
                if (xiaoren >= renxing)
                {

                    bool isTransform = this.GetComponent<hero>().isTransform;
                    if (!isTransform)
                    {
                        animSelf.SetTrigger("hit");
                    }
                    else
                        if (!isTransform)
                    {

                        animSelf.SetTrigger("transformhit");

                    }
                }
            }
            else
            {
                float renxing = Renxingshiyong;
                if (xiaoren >= renxing)
                {
                    animSelf.SetTrigger("hit");
                }

            }
        }
    }

    public void DamageShow()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        GameObject mObject = Instantiate(damageShow, pos, Quaternion.identity);
        float a = Mathf.Floor(damageamount);
        int b = (int)a;
        mObject.GetComponent<damageShow>().Value = b;

    }

    public IEnumerator UpdateHealthBar()
    {
        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        //healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HPthreata * 0.01f);
        // Set the scale of the health bar to be proportional to the player's health.
        healthBar.transform.localScale = new Vector3(healthScale.x * (HPthreata / HP), 2, 1);
        yield return new WaitForSeconds(1f);
        //healthBar2.transform.localScale = new Vector3(healthScale.x * (HPthreata /HP), 2, 1);
        startdamage = true;
    }

    public void UpdateStaBar()
    {
        staBar.transform.localScale = new Vector3(staScale.x * (SPthreata / SP), 2, 1);
        if (staBar.transform.localScale.x < 0)
        {
            staBar.transform.localScale = new Vector3(0, 2, 1);

        }
    }









    public void staminaCost(float cost)
    {

        SPthreata = SPthreata - cost;
        UpdateStaBar();


    }

    public void staminaCostEnd()
    {

        stopstarecover = false;
    }


    public void stopStaminaRecover()
    {
        stopstarecover = true;

    }



    private void setRenxing(float ren)
    {
        //设置韧性 在攻击动作开始前设定韧性 韧性高就不会被打断

        Renxing = ren;
        Renxingshiyong = Renxing;



    }


    private void recoverRenxing()
    {

        Renxingshiyong = defaultRenxing;

    }


}
