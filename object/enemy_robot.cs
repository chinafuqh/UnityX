using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine;
using Spine.Unity;

public class enemy_robot : MonoBehaviour
{

    public float moveSpeed = 10f;        // 走路速度
    public float runSpeed = 15f;         //跑步速度
    public int HP = 2;                  // How many times the enemy can be hit before it dies.
    public Sprite deadEnemy;            // A sprite of the enemy when it's dead.
    public bool isTransform = false;    //支持多形态
    public float searchradis = 8;
    public float runRange = 8;
    public float attackRange = 4;
    public float alarmRange = 20;
    public int skillcount = 1;
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public Spine.Skeleton skeleton;
    SkeletonAnimator skeletonAnimation;
    public Rigidbody2D bullet;
    Bone bone;


    private const string attack1State = "attack1";
    private const string attack2State = "attack2";
    private const string attack3State = "attack3";
    private SpriteRenderer ren;         // Reference to the sprite renderer.
    private Transform frontCheck;       // Reference to the position of the gameobject used for checking if something is in front.
    private Transform groundcheck;
    private bool dead = false;          // Whether or not the enemy is dead.
    private bool land = false;
    private bool jump = false;
    // private Score score;                // Reference to the Score script.
    private bool haveEnemy = false;      //是否有敌人
    private int baseState = 0;     //基本状态  0 休闲  1 战备  2其他
    private int subBattleState = 0;     //次级战斗状态  0 阶段0  1阶段1 ……
    private bool faceright = true;    //面朝方向 
    private float sign = 1;          //速度符号，决定速度的方向是X轴的哪一边
    private GameObject target;
    [HideInInspector]
    public AnimatorStateInfo animState;
    private Animator anim;
    private int actionStep = 0;
    private bool actionready = true;
    private bool isback = false;
    private bool canmove = false;
    private bool canface = false;
    private bool canattack = false;
    private bool candefend = false;
    private bool canthink = true;
    private float waitingtime = 2f;
    private bool attacked = false;
    private float defaultRenxing = 10;
    private float Renxing = 10;
    public float Renxingshiyong = 10;
    public bool befucked = false;
    private bool locktarget = false;
    Transform GunposBone, bulletBone, tarHitpoint;
    public GameObject lockLiser, line, weapon1;
    public bool[] colddown = new bool[20];
    private bool fade = false;

    void Awake()
    {
        // Setting up the references.
        skeletonAnimation = GetComponent<SkeletonAnimator>();
        frontCheck = transform.Find("frontCheck").transform;
        groundcheck = transform.Find("groundcheck").transform;
        //score = GameObject.Find("Score").GetComponent<Score>();
        faceright = !faceright;
        sign *= -1;
        skeleton = skeletonAnimation.Skeleton;
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
        anim = GetComponent<Animator>();
        GunposBone = this.gameObject.transform.GetChild(3);
        bulletBone = this.gameObject.transform.GetChild(4);
        weapon1 = this.gameObject.transform.GetChild(1).gameObject;


        line = Instantiate(lockLiser);

        //父物体设为闪电对象
        line.transform.parent = transform;

        //设为非激活
        line.SetActive(false);

    }

    private void Start()
    {


    }



    void FixedUpdate()
    {
        // Create an array of all the colliders in front of the enemy.
        //在fixedupadte中实现基本的行为元素，如行走，跑步，待机，思考等等


        animState = anim.GetCurrentAnimatorStateInfo(0);

        // Check each of the colliders.
        if (!dead)
        {

            //休闲状态 索敌并巡逻
            if (baseState == 0)
            {
                //进入休闲状态
                // anim.SetTrigger("normal");
                searchPlayer();
                //startmove(sign, moveSpeed);
                //normal_move();



            }

            //战斗状态
            if (baseState == 1)
            {
                //进入战斗状态
                
                //战斗中的移动
                float fangxiang = target.transform.position.x - transform.position.x;
                float fangxiangSign = fangxiang / Mathf.Abs(fangxiang);
                float tarDistance = Mathf.Abs(fangxiang);

                //先执行思考模块
                if (canthink)
                {
                    canthink = false;//得出结果后，停止思考 执行次级战斗命令

                    subBattleState = think(tarDistance);

                    StartCoroutine(TestThink(subBattleState, tarDistance));

                }

                if (canface)
                {
                    facingPlayer(fangxiangSign);
                }



                if (!animState.IsTag("attack")&& actionready)
                {
                    canthink = true;


                }
                //无事可做的时候执行移动模块
             
                if (canmove)
                {
                    battle_move(tarDistance);

                }
               

                //超过一定距离 脱离战斗

                //攻击结束后看是否需要转向


                //看看能不能攻击





            }













        }
        if (GetComponent<getHurt>().HPthreata <= 0 && !dead)
        // ... call the death function.
        {

            dead = true;
        }





    }



    private void Update()
    {
        if (locktarget)
        {
            float fangxiang = target.transform.position.x - transform.position.x;
            float fangxiangSign = fangxiang / Mathf.Abs(fangxiang);
            tarHitpoint = target.transform.Find("hitpiont");
            var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(tarHitpoint.position);
            Vector3 defaultPoint = new Vector3(transform.position.x+(sign*2), transform.position.y+2, transform.position.z);

            Vector2 gun = new Vector2(GunposBone.position.x, GunposBone.position.y);
            RaycastHit2D hit = Physics2D.Linecast(gun, tarHitpoint.position, (1 << LayerMask.NameToLayer("ground")) | (1 << LayerMask.NameToLayer("Player"))| (1 << LayerMask.NameToLayer("Doge")));

            if (fangxiang * sign < 0)
            {
                skeletonSpacePoint = defaultPoint;

            }

            if (skeletonAnimation.Skeleton.FlipX)
                skeletonSpacePoint.x *= -1;

            bone.SetPosition(skeletonSpacePoint);

            line.SetActive(true);
            Vector2 A = new Vector2(GunposBone.position.x, GunposBone.position.y);
            Vector2 B = hit.point;

            //获取Line组件
            Line lineComponent = line.GetComponent<Line>();
            lineComponent.SetColor(Color.red);
            lineComponent.A = A;
            lineComponent.B = B;
            lineComponent.Thickness = 0.75f;
            lineComponent.Draw();

        }



    }



    //思考模块 返回值为行为类型
    public int think(float distance)
    {
        if (target != null && target.GetComponent<getHurt>().isdead)
        //获取玩家血量和自身血量的百分比
        {

            return 1000;

        }
        else
        {
            float playerhp = GameObject.Find("hero").GetComponent<getHurt>().HP;
            float playerhpt = GameObject.Find("hero").GetComponent<getHurt>().HPthreata;
            float selfhp = GetComponent<getHurt>().HP;
            float selfhpt = GetComponent<getHurt>().HPthreata;
            float playerHP = playerhpt / playerhp;
            float selfHP = selfhpt / selfhp;
            float deltaHigh = Mathf.Abs(target.transform.position.y - transform.position.y);
            if (deltaHigh > 2 && distance>10 && Random.Range(0, 100) < 30&& colddown[3])
            {
                return 3;

            }
            else

                if (distance<20&& distance>10&&Random.Range(0, 100) < 10 && colddown[3])
            {
                return 3;


            }
            else
            if(distance<5 && colddown[2])
            {
                return 2;

            }
            else


                if (distance<3 && colddown[1])
            {

                return 1;


            }
            else
                if (distance>alarmRange)
            {


                return 100;

            }
            else
                if(canmove)
            {

                return 101;

            }
            else 
                if(playerHP <=0)
            {

                return 1000;
            }
        }

        //1 执行攻击 
        //2 随机缓慢移动
        //3 追踪玩家
        //4其他
        //0 什么都不做
        return 0;
    }






    public void cow()
    {



    }




    //搜索玩家 没有就持续缓慢移动
    public void searchPlayer()
    {
        Collider2D[] frontHits_player = Physics2D.OverlapCircleAll(transform.position, searchradis, 1 << LayerMask.NameToLayer("Player"));
        foreach (Collider2D c in frontHits_player)
        {

            if (c.tag == "Player" && !c.GetComponent<getHurt>().isdead)
            {
                // ... Flip the enemy and stop checking the other colliders.
                baseState = 1;
                target = c.gameObject;
                anim.SetTrigger("active");
            }


        }

    }



    //休闲时移动 碰到障碍物转弯
    public void normal_move()
    {

        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1 << LayerMask.NameToLayer("Default"));
        foreach (Collider2D c in frontHits)
        {

            if (c.tag == "Obstacle")
            {
                // ... Flip the enemy and stop checking the other colliders.
                Flip();
                break;
            }
        }




    }



    //战斗移动规则，距离过远就跑步追，进入攻击范围停止移动
    public void battle_move(float tarDistance)
    {

        if (tarDistance > runRange)
        {
            //startmove();
            anim.SetFloat("speed", runSpeed);

        }

        if (tarDistance <= runRange && tarDistance > attackRange)
        {

            //startmove();
            anim.SetFloat("speed", moveSpeed);

        }

        if (tarDistance <= attackRange)
        {
            //endmove();
            
            anim.SetFloat("speed", 0);


        }


    }




    public void facingPlayer(float fangxiangSign)
    {
        if (fangxiangSign > 0 && !faceright)
        {
            Flip();

        }
        else
                if (fangxiangSign < 0 && faceright)
        {
            Flip();


        }

    }






  







    void Death()
    {
        // Find all of the sprite renderers on this object and it's children.
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Disable all of them sprite renderers.
        foreach (SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }

        // Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
        ren.enabled = true;
        ren.sprite = deadEnemy;


        // Increase the score by 100 points
        //score.score += 100;

        // Set dead to true.
        dead = true;

        // Allow the enemy to rotate and spin it by adding a torque.

        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }


        Destroy(gameObject, 2);
    }


    public void Flip()
    {

        faceright = !faceright;
        sign *= -1;
        // Multiply the x component of localScale by -1.
        /*
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
        */
        skeleton.FlipX = !skeleton.FlipX;
    }

    /*
    public void DoDamage()
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
                comb[i] = comblist2[i];
            }



        if (comb[3] == 1)//如果是圆形范围
        {
            Novalightning();
            float Radis = comb[4];
            enemies = Physics2D.OverlapCircleAll(transform.position, Radis, 1 << LayerMask.NameToLayer("Player"));
            foreach (Collider2D en in enemies)
            // foreach (RaycastHit2D en in enimies2)
            {
                // Check if it has a rigidbody (since there is only one per enemy, on the parent).
                Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
                if (rb != null && rb.tag == "Player")
                {
                    // Find the Enemy script and set the enemy's health to zero.
                    StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(gameObject, comb[0], comb[1], comb[2], comb[12]));
                    chaneLighting(rb);//连锁闪电


                }
            }
        }
        else
            if (comb[3] == 2)//如果是直线范围 
        {
            Vector2 direction = new Vector2(sign * Mathf.Cos(Mathf.Deg2Rad * comb[5]), Mathf.Sin(Mathf.Deg2Rad * comb[5]));
            float distance = comb[4];
            RaycastHit2D[] enimiesRay = Physics2D.RaycastAll(transform.position, direction, distance, 1 << LayerMask.NameToLayer("Player"));
            foreach (RaycastHit2D en in enimiesRay)
            {
                Rigidbody2D rb = en.collider.GetComponent<Rigidbody2D>();
                if (rb != null && rb.tag == "Player")
                {
                    // Find the Enemy script and set the enemy's health to zero.
                    StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(gameObject, comb[0], comb[1], comb[2], comb[12]));
                    //chaneLighting(rb);
                }

            }

        }
        else
            if (comb[3] == 3)//如果是矩形发射范围
        {
            Vector2 origin = new Vector2(transform.position.x + (sign * comb[6]), transform.position.y + comb[7]);
            Vector2 size = new Vector2(comb[8], comb[9]);
            float angel = comb[5];
            Vector2 direction = new Vector2(sign * Mathf.Cos(comb[10] * Mathf.Deg2Rad), Mathf.Sin(comb[10] * Mathf.Deg2Rad));
            float distance = comb[4];

            RaycastHit2D[] enimiesRay = Physics2D.BoxCastAll(origin, size, angel, direction, distance, 1 << LayerMask.NameToLayer("Player"));
            foreach (RaycastHit2D en in enimiesRay)
            {
                Rigidbody2D rb = en.collider.GetComponent<Rigidbody2D>();
                if (rb != null && rb.tag == "Player")
                {
                    // Find the Enemy script and set the enemy's health to zero.
                    StartCoroutine(rb.gameObject.GetComponent<getHurt>().Hurt(gameObject, comb[0], comb[1], comb[2], comb[12]));
                    //chaneLighting(rb);
                }

            }
        }



    }
    */
    private void chaneLighting(Rigidbody2D enemy)
    {
        Vector3 posA = transform.position;
        Vector3 posB = enemy.gameObject.transform.position;
        Vector2 pos1 = new Vector2(posA.x, posA.y);
        Vector2 pos2 = new Vector2(posB.x, posB.y);

        //GameObject.Find("DemoScript").GetComponent<DemoScript>().CreatePooledBolt(pos1, pos2, Color.white, 3f);
    }


    private void Novalightning()
    {
        Vector2 diff = new Vector2(5, 0);
        Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);

        //define how many bolts we want in our circle
        int boltsInBurst = 10;

        for (int i = 0; i < boltsInBurst; i++)
        {
            //rotate around the z axis to the appropriate angle
            Quaternion rot = Quaternion.AngleAxis((360f / boltsInBurst) * i, new Vector3(0, 0, 1));

            //Calculate the end position for the bolt
            Vector2 boltEnd = (Vector2)(rot * diff) + pos1;

            //create a (pooled) bolt from pos1 to boltEnd
            // GameObject.Find("DemoScript").GetComponent<DemoScript>().CreatePooledBolt(pos1, boltEnd, Color.white, 3f);
        }

    }


    public void startmove()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(sign * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        if (GetComponent<Rigidbody2D>().velocity.x < moveSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * sign * 80000f);
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > moveSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

    }


    public void moveback()
    {
        Vector2 back = new Vector2(-sign * 100000f, 0);
        GetComponent<Rigidbody2D>().AddForce(back);


    }


    public void endmove()
    {

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

    }

    public void rush()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * sign * 120000f);


    }





    private void monsterCantMove()
    {
        canmove = false;

    }

    private void monsterCanMove()
    {
        canmove = true;


    }

    private void monsterCantFace()
    {
        canface = false;


    }

    private void monsterCanFace()
    {
        canface = true;


    }




    public IEnumerator BeFuck(float fucktime)
    {
        befucked = true;
        yield return new WaitForSeconds(fucktime);
        befucked = false;


    }


    private void lockOver()
    {
        locktarget = false;

    }


    private void shoot()
    {

        float z = GunposBone.rotation.eulerAngles.z;
        Vector3 fangxiang = bulletBone.position - GunposBone.position;
        Vector3 mo = fangxiang.normalized;
        Vector3 gunposAjust = new Vector3(GunposBone.position.x+Random .Range(-0.5f,0.5f), GunposBone.position.y+Random.Range(-0.5f, 0.5f), GunposBone.position.z);
        Rigidbody2D bulletInstance = Instantiate(bullet , gunposAjust, Quaternion.Euler(new Vector3(0, 0, z))) as Rigidbody2D;

        bulletInstance.velocity = new Vector2(mo.x * 100, mo.y * 100);



    }


    private void startlock()
    {

        locktarget = true;
    }

    private void liserOff()
    {
        line.SetActive(false);

    }

    private void attackon(int num)
    {
        //如果是分段伤害 num代表此次攻击是第几段
        weapon1.GetComponent<PolygonCollider2D>().enabled = true;
        weapon1.GetComponent<enemy_robot_attack>().num = num;

    }

    private void attackoff()
    {

        weapon1.GetComponent<PolygonCollider2D>().enabled = false;
        weapon1.GetComponent<enemy_robot_attack>().clearTarget();
        weapon1.GetComponent<enemy_robot_attack>().num = 1;

    }


    private void callweaponClear()
    {

        weapon1.GetComponent<enemy_robot_attack>().clearTarget();

    }



    private IEnumerator TestThink(int i, float tarDistance)
    {
        actionready = false;
        switch (i)
        {
            case 1000:
                baseState = 0;
                endmove();
                anim.SetFloat("speed", 0);
                anim.SetTrigger("unactive");
                anim.ResetTrigger("active");

                break;
            case 1:
                anim.SetTrigger("moveback");
                if(Random .Range (0,100)<50)
                {

                    anim.SetTrigger("attack2");
                }
                StartCoroutine(Countdown(5f, 1));
                break;
            case 2:
                anim.SetTrigger("attack1");
                StartCoroutine(Countdown(2f, 2));

                break;
            case 3:
                anim.SetTrigger("locktarget");
                endmove();
                anim.SetFloat("speed", 0);
                yield return new WaitForSeconds(5f);
                anim.SetTrigger("fire");
                StartCoroutine(Countdown(10f, 3));
                break;
            case 100:
                baseState = 0;
                endmove();
                anim.SetFloat("speed", 0);
                anim.SetTrigger("unactive");
                anim.ResetTrigger("active");

                break;
            case 101:
                battle_move(tarDistance);
                break;

            default:

                yield return new WaitForSeconds(3f);
                battle_move(tarDistance);
                break;








        }


        actionready = true;




    }

    IEnumerator Countdown(float countdown, int num)
    {
        colddown[num] = false;
        for (float timer = countdown; timer >= 0; timer -= Time.deltaTime)
            yield return 0;
        colddown[num] = true;
    }




    /*
        public IEnumerator SubBattleState(float tarDistance, float fangxiangSign, int subindex)
        {



            if (subindex == 1)
            {

                anim.SetTrigger("moveback");
                waitingtime = Random.Range(1f, 2f);

            }
            if (subindex == 2)
            {

                anim.SetTrigger("attack2");
                waitingtime = Random.Range(3f, 4f);

            }

            if (subindex == 0)
            {

                waitingtime = Random.Range(1f, 1.5f);


            }


            if (subindex == 3)
            {

                anim.SetTrigger("locktarget");
                endmove();
                anim.SetFloat("speed", 0);
                yield return new WaitForSeconds(5f);
                anim.SetTrigger("fire");
                waitingtime = Random.Range(2f, 3f);

                //anim.SetTrigger("attack1");
                waitingtime = Random.Range(3f, 4f);

            }


            if (subindex == 100)
            {


                baseState = 0;
                endmove();
                anim.SetFloat("speed", 0);
                anim.SetTrigger("unactive");
                anim.ResetTrigger("active");

            }


            if (subindex == -1)
            {

                waitingtime = 0;


            }




            yield return new WaitForSeconds(waitingtime);
            canthink = true;

        }
        */



}
