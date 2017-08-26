using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newPlayer : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.
    

    //public float moveForce = 5f;          // Amount of force added to move the player left and right.
    public float SpeedForce = 10;
    public float walkSpeed = 10f;             // The fastest the player can travel in the x axis.
    public float runSpeed = 30f;
    //public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
    public float jumpForce = 1000;         // Amount of force added when the player jumps.
    public float dodgeForce = 500;
    public float chargeForce = 0;
    public float chargeMax = 100;//蓄力最大数
    public float chargeMin = 10;  //没用
    public float chargeSpd = 10;  //蓄力速度
    
                                           //public AudioClip[] taunts;              // Array of clips for when the player taunts.
                                           //public float tauntProbability = 50f;    // Chance of a taunt happening.
                                           //public float tauntDelay = 1f;           // Delay for when the taunt should happen.

    public float keyTime = 0.5f;//两次连续按键的输入间隔
    public GameObject blast;
    public GameObject blast2;
    public GameObject blast3;
    public Rigidbody2D bulletobject;
    public float bulletspeed = 5;
    public int firecount = 5;
    public float mirrorvalue = 0.6f;//转身时的位移偏移大小
    [Tooltip("数组： 0定帧时间 1伤害值 2作用力 3范围（1，圆形，2，直线 3， 矩形范围） 4半径（圆）或距离（直线，矩形）   5水平方向夹角（直线，矩形）   6 X偏移  7 Y偏移  8 X缩放   9 Y缩放  10矩形发射方向角度  11韧性 12削韧    15被击特效类型")]
    public float[] comblist1 = new float[20];//连招1 数组： 0定帧时间 1伤害值 2作用力 3范围（1，圆形，2，直线） 4半径或距离   5水平方向夹角  6被击特效类型              轻攻击1 形态1
    [Tooltip("轻攻击2")]
    public float[] comblist2 = new float[20];//轻攻击2 形态1
    [Tooltip("轻攻击3")]
    public float[] comblist3 = new float[20];//轻攻击3 形态1
    [Tooltip("蓄力攻击1")]
    public float[] comblist4 = new float[20];//蓄力攻击1级 形态1
    [Tooltip("蓄力攻击2")]
    public float[] comblist5 = new float[20];//蓄力攻击2级 形态1
    [Tooltip("蓄力攻击13")]
    public float[] comblist6 = new float[20];//蓄力攻击3级 形态1
    [Tooltip("变形攻击1")]
    public float[] comblist7 = new float[20];//变形攻击 形态1
    [Tooltip("轻攻击4")]
    public float[] comblist8 = new float[20];//轻攻击4 形态1
    [Tooltip("形态2 轻攻击1")]
    public float[] comblist20 = new float[20];//轻攻击1 形态2
    [Tooltip("形态2 轻攻击2")]
    public float[] comblist21 = new float[20];//轻攻击2 形态2
    [Tooltip("形态2 轻攻击3")]
    public float[] comblist22 = new float[20];//轻攻击3 形态2
    [Tooltip("形态2 重攻击1")]
    public float[] comblist23 = new float[20];//重攻击1 形态2
    [Tooltip("形态2 重攻击2")]
    public float[] comblist24 = new float[20];//重攻击2 形态2
    [Tooltip("形态2 重攻击3")]
    public float[] comblist25 = new float[20];//重攻击3 形态2
    [Tooltip("形态2 变形攻击")]
    public float[] comblist26 = new float[20];//变形攻击 形态2



    public float[,] combshuju = new float[15, 20];
    public Material canyingMaterial;
    public Material defaultMaterial;

    // private int tauntIndex;                 // The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
    private Animator anim;                  // Reference to the player's animator component.
    private AnimatorStateInfo animState;
    private Rigidbody2D rigi;
    private Vector2 playervector;
    private GameObject weapon;
    private bool canmove = true;
    private int HitCount = 0;
    private int heavyHitCount = 0;
    //private int attackCount = 0;
    private const string IdelState = "yu_idel";
    private const string attack1State = "yu_attack1";
    private const string attack2State = "yu_attack2";
    private const string attack3State = "yu_attack3";
    private const string heavyattack1State = "yu_heavyattack1";
    private const string heavyattack2State = "yu_heavyattack2";
    private const string heavyattack3State = "yu_heavyattack3";
    private const string WalkState = "yu_walk";
    private const string dodgeState = "yu_rush";
    private const string dodgebackState = "yu_dodge";
    private const string chargeState1 = "yu_charge1" ;
    private const string chargeState2 = "yu_charge2";
    private const string chargeattack1State = "yu_chargeattack1";
    private const string chargeattack2State = "yu_chargeattack2";
    private const string chargeattack3State = "yu_chargeattack3";
    private const string transformState = "yu_transform";
    private const string transformAttackState = "yu_transformattack";

    private Vector3 blastpiont;
   // private bool fall = false;
   // private bool land = false;
    private GameObject self;
    private float timelost = 0f;
    private bool attack = false;
    private float hspeed = 0;
    private int keycount = 0;
    private float gs = 3; //储存重力系数
    private int dodgeCount = 0;
    private float t1 = 0;
    private float t2 = 0;
    private float t_detal = 0;
    private bool isDown = false;
    private float chargeTotal = 0;//当前蓄力数
    private bool dodgeban = false;
    public bool isTransform = false;
    private float sign = 1;
    private GameObject startpiont;
    private float defaultRenxing = 10;//默认韧性
    private  float Renxing = 10;//韧性变量   技能改变的就是这个韧性
    public float Renxingshiyong = 10;//实际韧性
    private bool attackpanding = false;
    //public ArrayList comblist[] = null;






    void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        self = this.gameObject ;
        gs = GetComponent<Rigidbody2D>().gravityScale;
        startpiont = GameObject.Find("startpiont");



    }





    void Update()
    {
        //将攻击标记重置
        dodgeCount = 0;


        animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName(IdelState) && HitCount != 0 && animState.normalizedTime > 0f)
        {
            HitCount = 0;
            anim.SetInteger("attackcount", HitCount);

        }
        if (HitCount != 0 && animState.normalizedTime > 1.0f)
        {
            HitCount = 0;
            anim.SetInteger("attackcount", HitCount);

        }


        if (animState.IsName(IdelState) && heavyHitCount != 0 && animState.normalizedTime > 0f)
        {
            heavyHitCount = 0;
            anim.SetInteger("heavyattackcount", heavyHitCount);

        }

        if (heavyHitCount != 0 && animState.normalizedTime > 1.0f)
        {
            heavyHitCount = 0;
            anim.SetInteger("heavyattackcount", heavyHitCount);

        }

        if (animState.IsName(IdelState) && chargeTotal != 0 && animState.normalizedTime > 0f)
        {
            chargeTotal = 0;
            isDown = false;
            anim.SetFloat("charge", chargeTotal);
            anim.ResetTrigger("chargeattack");

        }
        //如果变形动作结束，则转换形态





        //The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, (1 << LayerMask.NameToLayer("Ground")) |(1<< LayerMask.NameToLayer("Enemy")));

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded && !attack)
            jump = true;




        if (Input.GetButtonDown("lightattack") && grounded && GetComponent<getHurt>().SPthreata>0)
        {
            // ... set the animator Shoot trigger parameter and play the audioclip.

            //anim.SetTrigger("attackcount");
            attackcommand();

        }

        //蓄力攻击和重攻击 根据变形状态决定是否有重攻击或蓄力攻击 配置时注意
          if ( Input.GetButtonDown("heavyattack") && grounded && !isTransform && GetComponent<getHurt>().SPthreata > 0)
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


          if (Input.GetButtonUp("heavyattack")&&  chargeTotal!=0 && !isTransform && GetComponent<getHurt>().SPthreata > 0)
          {
              isDown = false;
              t2 = Time.time;
              chargeTotal = (t2 - t1) * chargeSpd;
              anim.SetFloat("charge", chargeTotal);
              anim.SetTrigger("chargeattack");

            //isDown = false;
            chargeTotal = 0;
          }

        if (Input.GetButtonDown("heavyattack") && grounded && isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
           

            heavyattackcommand();
        }



        //蓄力攻击和重攻击

        //变形和变形攻击
        if (Input.GetButtonDown("transform") && grounded && GetComponent<getHurt>().SPthreata > 0)
        {
            if (HitCount == 0 && heavyHitCount == 0 && !animState.IsName(transformState))
            {
                anim.SetTrigger("istransform");
            }
            else

                transformAttack();




        }

        //变形和变形攻击

        //闪避

        if (Input.GetButtonDown("dodge") && GetComponent<getHurt>().SPthreata > 0)
        {
            float inputspeed = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputspeed )> 0)
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

        //测试用重置按钮
        if (Input.GetButtonDown("Submit"))
        {

            StartCoroutine(GameObject.Find("god").GetComponent<godOfGame>().playerDead());
        }

    }





    //连击
    public void attackcommand()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName(IdelState)|| animState.IsName(WalkState) && HitCount == 0 && GetComponent<getHurt>().SPthreata > 0)
        {
            HitCount = 1;
            anim.SetTrigger("isattack");
            //canmove = false;


        } else
        if (animState.IsName(attack1State) && HitCount == 1 && animState.normalizedTime > 0 && GetComponent<getHurt>().SPthreata>0)
        {
            HitCount = 2;
            anim.SetInteger("attackcount", HitCount);
            //canmove = false;



        } else
        if (animState.IsName(attack2State) && HitCount == 2 && animState.normalizedTime > 0 && (GetComponent<getHurt>().SPthreata -10)>0)
        {
            HitCount = 3;
            anim.SetInteger("attackcount", HitCount);
           // canmove = false;


        }



    }

    public void heavyattackcommand()
    {

        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName(IdelState) || animState.IsName(WalkState) && heavyHitCount == 0 && isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
            heavyHitCount = 1;
            anim.SetTrigger("isheavyattack");



        }
        else
        if (animState.IsName(heavyattack1State) && heavyHitCount == 1 && animState.normalizedTime > 0 && isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
            heavyHitCount = 2;
            anim.SetInteger("heavyattackcount", heavyHitCount);




        }
        else
        if (animState.IsName(heavyattack2State) && heavyHitCount == 2 && animState.normalizedTime > 0 && isTransform && GetComponent<getHurt>().SPthreata > 0)
        {
            heavyHitCount = 3;
            anim.SetInteger("heavyattackcount", heavyHitCount);



        }


    }


    private void transformAttack()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        if (HitCount>0 || heavyHitCount > 0|| chargeTotal>0 && animState.normalizedTime > 0 && !animState.IsName(transformAttackState) && GetComponent<getHurt>().SPthreata > 0)
        {
            anim.SetTrigger("istransformattack");
            HitCount = 0;
            heavyHitCount = 0;
            chargeTotal = 0;
            anim.SetInteger("heavyattackcount", heavyHitCount);
            anim.SetInteger("attackcount", HitCount);
            anim.SetFloat("charge", chargeTotal);

        }

    }


    void FixedUpdate()
    {

        // Cache the horizontal input.
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        

        if (canmove)
            
        {
               hspeed = Input.GetAxis("Horizontal");

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(SpeedForce * hspeed));

            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached walkSpeed yet...
            //if (hspeed * GetComponent<Rigidbody2D>().velocity.x < walkSpeed)
                // ... add a force to the player.
                rigi.velocity = new Vector2(SpeedForce * hspeed, GetComponent<Rigidbody2D>().velocity.y);




            // If the player's horizontal velocity is greater than the walkSpeed...
            //if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > walkSpeed)
            // ... set the player's velocity to the walkSpeed in the x axis.
            //rigi.velocity = new Vector2(walkSpeed, GetComponent<Rigidbody2D>().velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (hspeed > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (hspeed < 0 && facingRight)
                // ... flip the player.

 

                

                Flip();

        }

        // If the player should jump...
        if (jump)
        {
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump");

            // Play a random jump audio clip.
            //   int i = Random.Range(0, jumpClips.Length);
            //   AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            // Add a vertical force to the player.
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
            //land = false;
        }

        //判断下落状态
        Vector3 sudu = this.GetComponent<Rigidbody2D>().velocity;
        if (sudu.y <= 0&& !grounded)
        {
            
            isFall();
        }
        else
        if (sudu.y == 0 && grounded)
            {
            isLand();
        }
        //判断下落状态

        //判断闪避动作
        
        


        if (animState.IsName(dodgeState))
        {

                rigi.velocity = new Vector2(sign*dodgeForce, GetComponent<Rigidbody2D>().velocity.y);

        }
        //闪避时位移
        if (animState.IsName(dodgebackState))

        {

                rigi.velocity = new Vector2(-sign*dodgeForce, GetComponent<Rigidbody2D>().velocity.y);

        }
        //判断闪避



    }
    
    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        sign *= -1;


        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        mirrorpos();

    }

    //翻转坐标，因为图片中心点，直接翻转会产生图像位移
    void mirrorpos()
    {
        //Vector3 spritepos = transform.position;
        Vector3 spriteNewPos;

           spriteNewPos = new Vector3(transform.position.x - (sign*2 * mirrorvalue), transform.position.y, transform.position.z);
            transform.position = spriteNewPos;


        




    }




        //创建刀光，刀光是个物体，自带动作，根据朝向需要翻转刀光
    public void createBlast()

    {

        if (facingRight)
        {
            Vector3 blastpos = new Vector3(transform.position.x + 2.2f, transform.position.y + 0.4f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast, blastpos, Quaternion.Euler(new Vector3(0, 0, 0)));
            prefabInstance.transform.parent = this.transform;
        }

        else
        {
            Vector3 blastpos = new Vector3(transform.position.x - 2.2f, transform.position.y + 0.4f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast, blastpos, Quaternion.Euler(new Vector3(0, 180, 0)));
            //prefabInstance.gameObject.GetComponent<SpriteRenderer>().material.mainTextureScale = new Vector2(-1, 1);
            prefabInstance.transform.parent = this.transform;
        }
        



    }


    public void createBlast2()

    {

        if (facingRight)
        {
            Vector3 blastpos = new Vector3(transform.position.x + 4.954f, transform.position.y - 0.237f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast2, blastpos, Quaternion.Euler(new Vector3(0, 0, 0)));
            prefabInstance.transform.parent = this.transform;
        }

        else
        {
            Vector3 blastpos = new Vector3(transform.position.x - 4.954f, transform.position.y - 0.237f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast2, blastpos, Quaternion.Euler(new Vector3(0, 180, 0)));
            //prefabInstance.gameObject.GetComponent<SpriteRenderer>().material.mainTextureScale = new Vector2(-1, 1);
            prefabInstance.transform.parent = this.transform;
        }




    }

    public void createBlast3()

    {

        if (facingRight)
        {
            Vector3 blastpos = new Vector3(transform.position.x + 0.88f, transform.position.y + 0.71f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast3, blastpos, Quaternion.Euler(new Vector3(0, 0, 0)));
            prefabInstance.transform.parent = this.transform;
        }

        else
        {
            Vector3 blastpos = new Vector3(transform.position.x - 0.88f, transform.position.y +0.71f, transform.position.z);
            GameObject prefabInstance = Instantiate(blast3, blastpos, Quaternion.Euler(new Vector3(0, 180, 0)));
            //prefabInstance.gameObject.GetComponent<SpriteRenderer>().material.mainTextureScale = new Vector2(-1, 1);
            prefabInstance.transform.parent = this.transform;
        }




    }


    public IEnumerator fireblast(string fire)
    {
        if (fire == "on")
        {
            for (int i = 0; i < firecount; i++)
            {
                
              
                Vector3 blastpiont = new Vector3(transform.position.x + i * 1, transform.position.y + 2, 0);
                yield return new WaitForSeconds(0.05f);
                createLava(blastpiont);

            


                    
            }
            
        }
    }


    public void createLava(Vector3 blastpiont )
    {
        Rigidbody2D bulletInstance = Instantiate(bulletobject, blastpiont, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
        bulletInstance.velocity = new Vector2(0, -bulletspeed);

    }


    public void isFall()
    {
        //animplay();
        anim.SetTrigger("isFall");
            
        //fall = true;
       
            
        

    }

    public void isLand()
    {
        //anim.speed = 1;
        anim.SetTrigger("isground");
        //anim.Play("yu_idel", 0, 0.0f);
            
        //land = true;
        //fall = false;

        

    }

    public void transformready()
    {
        anim.SetTrigger("transformready");
        isTransform = !isTransform;

    }


    public void canying()
    {
        GetComponent<SpriteRenderer>().material = canyingMaterial;

    }

    public void canyingOff()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;

    }








    public void DoDamage()
    {
        if (attackpanding)
        {
            // Find all the colliders on the Enemies layer within the bombRadius.
            Collider2D[] enemies = null;
            float[] comb = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//默认值
            if (animState.IsName(attack1State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist1[i];
                }
            if (animState.IsName(attack2State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist2[i];
                }
            if (animState.IsName(attack3State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist3[i];
                }

            if (animState.IsName(chargeattack1State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist4[i];
                }



            if (animState.IsName(chargeattack2State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist5[i];
                }

            if (animState.IsName(chargeattack3State) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist6[i];
                }

            if (animState.IsName(transformAttackState) && !isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist7[i];
                }

            if (animState.IsName(attack1State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist20[i];
                }
            if (animState.IsName(attack2State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist21[i];
                }
            if (animState.IsName(attack3State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist22[i];
                }
            if (animState.IsName(heavyattack1State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist23[i];
                }
            if (animState.IsName(heavyattack2State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist24[i];
                }
            if (animState.IsName(heavyattack3State) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist25[i];
                }
            if (animState.IsName(transformAttackState) && isTransform)
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist26[i];
                }

            //设置韧性参数
            Renxing = comb[11];
            if (comb[3] == 1)//如果是圆形范围
            {
                float Radis = comb[4];
                enemies = Physics2D.OverlapCircleAll(startpiont.transform.position, Radis, 1 << LayerMask.NameToLayer("Enemy"));
                foreach (Collider2D en in enemies)
                // foreach (RaycastHit2D en in enimies2)
                {
                    // Check if it has a rigidbody (since there is only one per enemy, on the parent).
                    Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
                    if (rb != null && rb.tag == "Enemys")
                    {
                        // Find the Enemy script and set the enemy's health to zero.
                        StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(self, comb[0], comb[1], comb[2], comb[12]));
                        //chaneLighting(rb);连锁闪电

                    }
                }
            }
            else
                if (comb[3] == 2)//如果是直线范围 
            {
                Vector2 direction = new Vector2(sign * Mathf.Cos(Mathf.Deg2Rad * comb[5]), Mathf.Sin(Mathf.Deg2Rad * comb[5]));
                float distance = comb[4];
                RaycastHit2D[] enimiesRay = Physics2D.RaycastAll(startpiont.transform.position, direction, distance, 1 << LayerMask.NameToLayer("Enemy"));
                foreach (RaycastHit2D en in enimiesRay)
                {
                    Rigidbody2D rb = en.collider.GetComponent<Rigidbody2D>();
                    if (rb != null && rb.tag == "Enemys")
                    {
                        // Find the Enemy script and set the enemy's health to zero.
                        StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(self, comb[0], comb[1], comb[2], comb[12]));
                        //chaneLighting(rb);
                    }

                }

            }
            else
                if (comb[3] == 3)//如果是矩形发射范围
            {
                Vector2 origin = new Vector2(startpiont.transform.position.x + (sign * comb[6]), startpiont.transform.position.y + comb[7]);
                Vector2 size = new Vector2(comb[8], comb[9]);
                float angel = comb[5];
                Vector2 direction = new Vector2(sign * Mathf.Cos(comb[10] * Mathf.Deg2Rad), Mathf.Sin(comb[10] * Mathf.Deg2Rad));
                float distance = comb[4];

                RaycastHit2D[] enimiesRay = Physics2D.BoxCastAll(origin, size, angel, direction, distance, 1 << LayerMask.NameToLayer("Enemy"));
                foreach (RaycastHit2D en in enimiesRay)
                {
                    Rigidbody2D rb = en.collider.GetComponent<Rigidbody2D>();
                    if (rb != null && rb.tag == "Enemys")
                    {
                        // Find the Enemy script and set the enemy's health to zero.
                        StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(self, comb[0], comb[1], comb[2], comb[12]));
                        //chaneLighting(rb);
                    }

                }
            }


            attackpanding = false;

        }

         
    }

    public void PlayerCanMove()
    {
        canmove = true;
        attack = false;
        chargeTotal = 0;
        isDown = false;


    }

    public void PlayeCantMove()
    {
        canmove = false;
        attack = true;
        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

    }



    public void dodgeTime()
    {

       // GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        this.gameObject.layer = 12;
        this.gameObject.tag = "Dodge";
        dodgeban = true;
    }

    public void dodgeEnd()
    {

       // GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = gs;
        this.gameObject.layer = 10;
        this.gameObject.tag = "Player";
        dodgeban = false;
    }


    public IEnumerator attackMove()
    {
        




                rigi.velocity = new Vector2(sign*50, GetComponent<Rigidbody2D>().velocity.y);
                



        yield return new WaitForSeconds(0.1f);

        rigi.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

    }



    public void attackMoveMotionOff()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;


    }

    public void attackMoveMotionOn()
    {
        GetComponent<Rigidbody2D>().gravityScale = gs;


    }


    private void attackoff()
    {
        attackpanding = false;


    }




    public void shakeCamera()
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().shakeCamera();
    }



   
    //判断攻击区域函数 暂时不用
    /* private Collider2D[] circleArea(float Radis)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(startpiont.transform.position, Radis, 1 << LayerMask.NameToLayer("Enemy"));

        return enemies;
    }


    private Collider2D[] lineArea(float jiaodu, float distance)
    {
        Vector2 direction = new Vector2(sign * Mathf.Sin(jiaodu), Mathf.Cos(jiaodu));
        Collider2D[] enemies = null;
        RaycastHit2D[] enimiesRay = Physics2D.RaycastAll(startpiont.transform.position, direction, distance, 1 << LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i <= enimiesRay.Length; i++)
        {
            enemies[i] = enimiesRay[i].collider;



        }

        return enemies;

    }
    */






    private void chaneLighting(Rigidbody2D enemy)
    {
        Vector3 posA = startpiont.transform.position;
        Vector3 posB = enemy.gameObject.transform.position;
        Vector2 pos1 = new Vector2(posA.x, posA.y);
        Vector2 pos2 = new Vector2(posB.x, posB.y);

        GameObject.Find("DemoScript").GetComponent<DemoScript>().CreatePooledBolt(pos1, pos2, Color.white, 3f);
    }



    private void setRenxing(int i)
    {
        if (i == 1)
        {
            Renxing = comblist1[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 2)
        {
            Renxing = comblist2[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 3)
        {
            Renxing = comblist3[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 4)
        {
            Renxing = comblist4[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 5)
        {
            Renxing = comblist5[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 6)
        {
            Renxing = comblist6[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 7)
        {
            Renxing = comblist7[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 8)
        {
            Renxing = comblist8[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 20)
        {
            Renxing = comblist20[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 21)
        {
            Renxing = comblist21[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 22)
        {
            Renxing = comblist22[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 23)
        {
            Renxing = comblist23[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 24)
        {
            Renxing = comblist24[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 25)
        {
            Renxing = comblist25[11];
            Renxingshiyong = Renxing;
        }
        else
            if (i == 26)
        {
            Renxing = comblist26[11];
            Renxingshiyong = Renxing;
        }


    }


    private void recoverRenxing()
    {

        Renxingshiyong = defaultRenxing;

    }

    private void attackon()
    {

        attackpanding = true;

    }



    // public IEnumerator Taunt()
    //{
    // Check the random chance of taunting.
    //   float tauntChance = Random.Range(0f, 100f);
    //  if (tauntChance > tauntProbability)
    //  {
    // Wait for tauntDelay number of seconds.
    //     yield return new WaitForSeconds(tauntDelay);

    // If there is no clip currently playing.
    //      if (!GetComponent<AudioSource>().isPlaying)
    //    {
    //         // Choose a random, but different taunt.
    //       tauntIndex = TauntRandom();

    // Play the new taunt.
    //         GetComponent<AudioSource>().clip = taunts[tauntIndex];
    //        GetComponent<AudioSource>().Play();
    //    }
    //}
    //  }


    // int TauntRandom()
    // {
    // Choose a random index of the taunts array.
    //    int i = Random.Range(0, taunts.Length);

    // If it's the same as the previous taunt...
    //   if (i == tauntIndex)
    // ... try another random taunt.
    //       return TauntRandom();
    //    else
    // Otherwise return this index.
    //        return i;
    // }
}
