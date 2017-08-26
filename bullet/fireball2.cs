using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball2 : MonoBehaviour {

    public GameObject lavaexp;

     /// <summary>
   /// 每秒最大可旋转的角度.     /// </summary>
    private const float MAX_ROTATION = 120;

    /// <summary>
    /// 每帧最大可旋转的角度.
    /// </summary>
    private static float MAX_ROTATION_FRAME;

     /// <summary>
     /// 攻击目标.
     /// </summary>
    public GameObject target;
    public float speed = 0.1f;//追踪起始速度
    public float maxSpd = 8f;//追踪最大速度，超过这个速度不再调整方向
    public float Vt = 0.001f; //加速度，每帧的加速度，尽量小一些
    private float spd = 0.1f;//保存加速度

    void Start()
    {
        // Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
        Destroy(gameObject, 6);
        MAX_ROTATION_FRAME = MAX_ROTATION / ((float)(Application.targetFrameRate == -1 ? 60 : Application.targetFrameRate));


    }


    void OnTriggerEnter2D(Collider2D col)
    {
        // If it hits an enemy...
        if (col.tag == "Ground")
        {
            // ... find the Enemy script and call the Hurt function.
            //col.gameObject.GetComponent<Enemy>().Hurt();

            // Call the explosion instantiation.
            OnExplode();
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().shakeCamera();

            // Destroy the rocket.
            Destroy(gameObject);
        }
          else if (col.tag == "Player")
          {
              OnExplode();
              GameObject.Find("Main Camera").GetComponent<CameraFollow>().shakeCamera();
             StartCoroutine(col.gameObject.GetComponent<getHurt>().Hurt(gameObject,0.1f,10f,100f, 100f));
            // Destroy the rocket.


            // Check if it has a rigidbody (since there is only one per enemy, on the parent).
            //  Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            //    if (rb != null && rb.tag == "Player")
            //  {

            //   Vector3 deltaPos = rb.transform.position - transform.position;

            // Apply a force in this direction with a magnitude of bombForce.
            //  Vector3 force = deltaPos.normalized * 100;
            //  rb.AddForce(force);

            // }
            transform.position = new Vector3(1000, 1000, 0);
            Destroy(gameObject,1);
          }
    }

    void FixedUpdate()
    {
        zhuizongdan();

    }
    void Update()
     {
       //zhuizongdan();

    }

    /// <summary>
    /// 确保角度在 [0, 360) 这个区间内.
    /// </summary>
    /// <param name="rotation">任意数值的角度.</param>
    /// <returns>对应的在 [0, 360) 这个区间内的角度.</returns>
    private float MakeSureRightRotation(float rotation)
     {
         rotation += 360;
         rotation %= 360;
         return rotation;
     }


    void OnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(lavaexp, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }


   



     private void zhuizongdan()

     {
        //跟踪弹到达最大速度后，停止转向，意味着敏感系数，速度越大，跟踪效果越强  不判断速度则无限追踪
        if (spd <= maxSpd)
        {
            GameObject tar = GameObject.Find("yu");
            float dx = tar.transform.position.x - this.transform.position.x;
            float dy = tar.transform.position.y - this.transform.position.y;
            float rotationZ = Mathf.Atan2(dy, dx) * 180 / Mathf.PI;
            //得到最终的角度并且确保在 [0, 360) 这个区间内
            rotationZ -= 90;
            rotationZ = MakeSureRightRotation(rotationZ);
            //获取增加的角度
            float originRotationZ = MakeSureRightRotation(this.transform.eulerAngles.z);
            float addRotationZ = rotationZ - originRotationZ;
            //超过 180 度需要修改为负方向的角度
            if (addRotationZ > 180)
            {
                addRotationZ -= 360;
            }
            //不超过每帧最大可旋转的阀值
            addRotationZ = Mathf.Max(-MAX_ROTATION_FRAME, Mathf.Min(MAX_ROTATION_FRAME, addRotationZ));
            //应用旋转
            this.transform.eulerAngles = new Vector3(0, 0, this.transform.eulerAngles.z + addRotationZ);

        }
            spd += speed + Vt;
            //移动
            this.transform.Translate(new Vector3(0, spd * Time.deltaTime, 0), Space.Self);
     }

    


}
