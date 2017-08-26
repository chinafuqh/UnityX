using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
public class spineboy : MonoBehaviour {
    private float hspeed;
    private Animator anim;
    public float SpeedForce = 10;
    private Rigidbody2D rigi;
    public Spine.Skeleton skeleton;
    SkeletonAnimator skeletonAnimation;
    private bool facingRight = true;
    private float sign = 1;
     [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    public new Camera camera;
    [SpineBone(dataField: "skeletonAnimation")]
    public string GunposName;
    Bone bone;
    private GameObject GunposBone;
    private GameObject bulletBone;
    private GameObject player;
    private Transform playerpos;
    public Rigidbody2D rocket;
    private float time;
    private float deltatime = 1.5f;


    void OnValidate()
    {
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimator>();
    }
    private void Awake()
    {
        player = GameObject.Find("robot");
    }
    // Use this for initialization
    void Start () {
        skeletonAnimation = GetComponent<SkeletonAnimator>();
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        skeleton = skeletonAnimation.Skeleton;
        bone = skeletonAnimation.Skeleton.FindBone(boneName);
        GunposBone = GameObject.Find("gunpos");
        bulletBone = GameObject.Find("bulletpos");
        playerpos = player.transform.GetChild(0);

    }



    // Update is called once per frame
    void Update () {
        /*
        var mousePosition = Input.mousePosition;
        var worldMousePosition = camera.ScreenToWorldPoint(mousePosition);
        var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
        if (skeletonAnimation.Skeleton.FlipX)
            skeletonSpacePoint.x *= -1;
        bone.SetPosition(skeletonSpacePoint);
        */

        var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(playerpos.position);
        bone.SetPosition(skeletonSpacePoint);

        if (Input.GetButtonDown("lightattack"))
            anim.SetTrigger("shoot");
        if (Time.time> time + deltatime )
        {
            anim.SetTrigger("shoot");

            time = Time.time;
        }
        



    }
    private void FixedUpdate()
    {
        /*
        hspeed = Input.GetAxis("Horizontal");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(SpeedForce * hspeed));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached walkSpeed yet...
        //if (hspeed * GetComponent<Rigidbody2D>().velocity.x < walkSpeed)
        // ... add a force to the player.
        rigi.velocity = new Vector2(SpeedForce * hspeed, GetComponent<Rigidbody2D>().velocity.y);

        if (hspeed > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (hspeed < 0 && facingRight)
            // ... flip the player.





            Flip();


       */
    }
    public void Flip()
    {
        facingRight = !facingRight;
        skeleton.FlipX = !skeleton.FlipX;
        sign *= -1;


    }


    private void shoot()
    {

        float z = GunposBone.transform.rotation.eulerAngles.z;
        Vector3 fangxiang = bulletBone.transform.position - GunposBone.transform.position;
        Vector3 mo = fangxiang.normalized;
        Rigidbody2D bulletInstance = Instantiate(rocket, GunposBone.transform.position, Quaternion.Euler(new Vector3(0, 0, z))) as Rigidbody2D;

        bulletInstance.velocity = new Vector2(mo.x * 30, mo.y * 30);





    }

}
