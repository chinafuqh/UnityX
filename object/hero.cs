using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Spine.Unity.Modules;
public class hero : MonoBehaviour
{
    private float hspeed;
    private Animator anim;
    public float maxSpeed = 5f;
    public float SpeedForce = 10;
    public float chargeSpd = 10f;
    public float chargeMax = 100;
    private Rigidbody2D rigi;
    public Spine.Skeleton skeleton;
    SkeletonAnimator skeletonAnimation;
    private bool facingRight = true;
    private float sign = 1;
    public bool isTransform = false;
    public GameObject paoputexiao;
    public GameObject luodiyanchen;
    public GameObject gongjiyanchen;
    public GameObject transformtexiao;
    private GameObject weapon;
    [HideInInspector]
    public float HitCount = 0;
    private float movedaduan = 0;
    [HideInInspector]
    public AnimatorStateInfo animState;
    private static string IdelState = "idel";
    private static string WalkState = "walk";
    private static string attack1State = "attack1";
    private static string attack2State = "attack2";
    private static string attack3State = "attack3";
    private static string attack4State = "attack4";
    private static string jumpState = "jump";

    private bool canmove = true;
    private bool attack = false;
    private float chargeTotal = 0;
    private  bool isDown = false;
    private float t1 = 0;
    private float t2 = 0;
    private float t_detal = 0;
    private Transform groundCheck, root,footL,footR,tranpiont;         
    private bool grounded = true;
    private int dodgeCount = 0;
    private bool jump = false;
    private bool isfall = false;
    private bool canmovedaduan = false;
    void OnValidate()
    {
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimator>();
    }
    private void Awake()
    {
        groundCheck = transform.Find("groundcheck");
        root = transform.Find("root");
        footL  = transform.Find("footL");
        footR = transform.Find("footR");
        tranpiont = transform.Find("tranpiont");
    }
    // Use this for initialization
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimator>();
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        skeleton = skeletonAnimation.Skeleton;
        //weapon = GameObject.Find("weapon1");




    }



    // Update is called once per frame
    void Update()
    {

        dodgeCount = 0;

        animState = anim.GetCurrentAnimatorStateInfo(0);

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, (1 << LayerMask.NameToLayer("ground")) | (1 << LayerMask.NameToLayer("Enemy")));

        /*
                if (animState.IsName(IdelState) && HitCount != 0 && animState.normalizedTime > 0f)
                {
                    HitCount = 0;
                    anim.SetInteger("attackcount", HitCount);

                }
                */
                
                if (HitCount != 0 && animState.normalizedTime > 1f)
                {
                    HitCount = 0;
                    //anim.SetFloat("attackcount", HitCount);
                }

                /*
                if (anim.IsInTransition(0))
                {
                HitCount = 0;
                anim.SetFloat("attackcount", HitCount);

                }
                */




        if (animState.IsName(IdelState) && chargeTotal != 0 && animState.normalizedTime > 0f)
                {
                    chargeTotal = 0;
                    isDown = false;
                    anim.SetFloat("charge", chargeTotal);
                    anim.ResetTrigger("chargeattack");
                    Debug.Log("ccc");
                }





        if (Input.GetButtonDown("lightattack"))
        {

                attackcommand();
        }



        if (Input.GetButtonDown("heavyattack") && grounded && !isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
            if (animState.IsName(WalkState) || animState.IsName(IdelState))
            {
                canmove = false;
                attack = true;
                t1 = Time.time;
                isDown = true;
                anim.SetTrigger("ischarge");
            }

            //heavyattackcommand();
        }

        if (isDown)
        {
            t_detal = Time.time;
            chargeTotal = (t_detal - t1) * chargeSpd;
            anim.SetFloat("charge", chargeTotal);
            if (chargeTotal >= chargeMax)
            {
                anim.SetTrigger("chargeattack");
                isDown = false;
                chargeTotal = 0;
            }

        }


        if (Input.GetButtonUp("heavyattack") && chargeTotal != 0 && !isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
            isDown = false;
            t2 = Time.time;
            chargeTotal = (t2 - t1) * chargeSpd;
            anim.SetFloat("charge", chargeTotal);
            anim.SetTrigger("chargeattack");

            //isDown = false;
            chargeTotal = 0;
        }




        //闪避
        if (Input.GetButtonDown("dodge") && GetComponent<getHurt>().SPthreata > 0&& grounded)
        {
            float inputspeed = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputspeed) > 0)
            {
                anim.SetTrigger("isDodge");
                dodgeCount += 1;

            }
            else

            {
                anim.SetTrigger("isDodgeback");
                dodgeCount += 1;

            }



        }


        if (Input.GetButtonDown("Jump")&& !isfall &&grounded&&canmove)
        {
            jump = true;

        }

        if(Input.GetButtonDown("transform"))
        {
            anim.SetTrigger("istransform");

        }


    }
    private void FixedUpdate()
    {



        if (canmove)
        {
            animState = anim.GetCurrentAnimatorStateInfo(0);
            hspeed = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", Mathf.Abs(hspeed * SpeedForce));

                movedaduan = animState.normalizedTime;

                anim.SetFloat("movedaduan", Mathf.Min(movedaduan,1));


            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached walkSpeed yet...
            //if (hspeed * GetComponent<Rigidbody2D>().velocity.x < walkSpeed)
            // ... add a force to the player.
            //rigi.velocity = new Vector2(SpeedForce * hspeed, GetComponent<Rigidbody2D>().velocity.y);
            if (hspeed * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
                // ... add a force to the player.
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * hspeed * 365);
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
                // ... set the player's velocity to the maxSpeed in the x axis.
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            if (hspeed == 0)
                rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (hspeed > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (hspeed < 0 && facingRight)
            // ... flip the player.





            Flip();

        if(jump)
        {
            animState = anim.GetCurrentAnimatorStateInfo(0);
            float jumptime = animState.normalizedTime;
            anim.SetTrigger("jump");
            anim.SetFloat("jumptime", jumptime);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1200f));


            jump = false;

        }

        if(GetComponent<Rigidbody2D>().velocity.y<=0&&!grounded&&!isfall)
        {
            anim.SetTrigger("fall");
            isfall = true;

        }

        if (animState.IsName("fall")&&grounded&&isfall)
        {
            anim.SetTrigger("grounded");
            isfall = false;
            anim.ResetTrigger("fall");
        }
//清除攻击标志 看是在update里合适还是fixed里合适
/*
        animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName(IdelState) && HitCount != 0 && animState.normalizedTime > 0f)
        {
            HitCount = 0;
            anim.SetInteger("attackcount", HitCount);

        }


        if (HitCount != 0 && animState.normalizedTime > 1f)
        {
            HitCount = 0;
            anim.SetInteger("attackcount", HitCount);
        }



        if (animState.IsName(IdelState) && chargeTotal != 0 && animState.normalizedTime > 0f)
        {
            chargeTotal = 0;
            isDown = false;
            anim.SetFloat("charge", chargeTotal);
            anim.ResetTrigger("chargeattack");
            Debug.Log("ccc");
        }

*/
////

    }
    public void Flip()
    {
        facingRight = !facingRight;
        skeleton.FlipX = !skeleton.FlipX;
        sign *= -1;


    }



    public void attackcommand()
    {
        animState = anim.GetCurrentAnimatorStateInfo(0);

        if (HitCount == 0 &&( animState.IsName(IdelState)||animState.IsName(WalkState)))
        {
            HitCount = 1.01f;
            anim.SetFloat("attackcount", HitCount);
            //canmove = false;


        }
        else
        if (animState.normalizedTime < 1f&&animState.IsName(attack1State))
        {
           
                HitCount = 2 + animState.normalizedTime;
                anim.SetFloat("attackcount", HitCount);




        }
        else
        if (animState.normalizedTime < 1f && animState.IsName(attack2State))
        {
            HitCount = 3+ animState.normalizedTime;
            anim.SetFloat("attackcount", HitCount);
            //canmove = false;



        }
        else
        if (animState.normalizedTime < 1f && animState.IsName(attack3State))
        {
            HitCount = 4+animState.normalizedTime;
            anim.SetFloat("attackcount", HitCount);
            //canmove = false;



        }
        else
        if (animState.normalizedTime < 1f && animState.IsName(attack4State))
        {
            HitCount = 5 + animState.normalizedTime;
            anim.SetFloat("attackcount", HitCount);
            //canmove = false;



        }




    }




    private void attackon(int num)
    {
        //如果是分段伤害 num代表此次攻击是第几段
        
        
            GameObject.Find("weapon1").GetComponent<PolygonCollider2D>().enabled = true;
            GameObject.Find("weapon1").GetComponent<AttackEnemy>().num = num;


    }

    private void attackoff()
    {

        GameObject.Find("weapon1").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("weapon1").GetComponent<AttackEnemy>().clearTarget();
        GameObject.Find("weapon1").GetComponent<AttackEnemy>().num = 1;

    }


    private void callweaponClear()
    {

        GameObject.Find("weapon1").GetComponent<AttackEnemy>().clearTarget();

    }





    public void shakeCamera()
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().shakeCamera();
    }


    public void PlayerCanMove()
    {
        canmove = true;
        attack = false;
        chargeTotal = 0;
        isDown = false;
        anim.SetFloat("charge", chargeTotal);


    }

    public void PlayeCantMove()
    {
        canmove = false;
        attack = true;
        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        anim.SetFloat("Speed", 0);

    }


    public void dodgemove1(float speed)
    {
        rigi.velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        Vector2 forcr = new Vector2(0, 100);
        rigi.AddForce(forcr);




    }


    public void rush()
    {
        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * sign * 1200f);
        GetComponent<SkeletonGhost>().ghostingEnabled = true;


    }

    public void dodge()
    {
        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * -sign * 1200f);
        GetComponent<SkeletonGhost>().ghostingEnabled = true;


    }

    public void stopMove()
    {

        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<SkeletonGhost>().ghostingEnabled = false;

    }


    public void stopJump()
    {

        rigi.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y,0);


    }


    public void dodgeTime_wudi()
    {

        this.gameObject.tag = "Dodge";
    }

    public void dodgeTime_chuanyue()
    {

        this.gameObject.layer = 13;

    }

    public void dodgeEnd_wudi()
    {

        this.gameObject.tag = "Player";

    }


    public void dodgeEnd_chuangyue()
    {
        this.gameObject.layer = 8;


    }



    public  void  attackmove(float force)
    {


        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * sign * force);


    }


    public void stopattackmove()
    {
        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

    }


    public void stepFX()
    {

       GameObject  paobu = Instantiate(paoputexiao, root.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        paobu.transform.localScale = new Vector3(sign, 1, 1);


    }

    public  void landFX()
    {
        Instantiate(luodiyanchen, root.position, Quaternion.Euler(new Vector3(0, 0, 0)));



    }

    public void attackStepFXL()
    {
        GameObject texiao = Instantiate(gongjiyanchen, footL.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        texiao.transform.localScale = new Vector3(sign, 1, 1);

    }


    public void attackStepFXR()
    {
        GameObject texiao = Instantiate(gongjiyanchen, footR.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        texiao.transform.localScale = new Vector3(sign, 1, 1);

    }

    public void transformFX()
    {
        Instantiate(transformtexiao, tranpiont.position, Quaternion.Euler(new Vector3(0, 0, 0)));

    }


    private void attackon2(int num)
    {
        //如果是分段伤害 num代表此次攻击是第几段


        GameObject.Find("weapon2").GetComponent<PolygonCollider2D>().enabled = true;
        GameObject.Find("weapon2").GetComponent<AttackEnemy>().num = num;


    }

    private void attackoff2()
    {

        GameObject.Find("weapon2").GetComponent<PolygonCollider2D>().enabled = false;
        GameObject.Find("weapon2").GetComponent<AttackEnemy>().clearTarget();
        GameObject.Find("weapon2").GetComponent<AttackEnemy>().num = 1;

    }


    private void callweaponClear2()
    {

        GameObject.Find("weapon2").GetComponent<AttackEnemy>().clearTarget();

    }



}
