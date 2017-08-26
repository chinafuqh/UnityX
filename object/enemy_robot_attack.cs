using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_robot_attack : MonoBehaviour
{
    private GameObject self;
    public int targetCount = 10;
    public int num = 1;
    [Tooltip("数组: 时间 伤害  作用力 削韧 韧性 类型 6个一组")]
    public float[] comblist1 = new float[30];
    public float[] comblist2 = new float[30];
    public float[] comblist3 = new float[30];
    public float[] comblist4 = new float[30];
    public float[] comblist5 = new float[30];

    private static string IdelState = "idel";
    private static string WalkState = "run";
    private static string attack1State = "attack1";
    private static string attack2State = "attack2";
    private static string chargeattack1State = "chargeattack1";
    List<GameObject> attacktarget;
    List<GameObject> attacktargetDelta;
    [HideInInspector]
    public float[] comb = new float[30];


    // Use this for initialization
    void Start()
    {
        self = transform.parent.gameObject;
        attacktarget = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.gameObject.GetComponent<getHurt>().befucked)
        {
            int numbTrue = 6 * (num - 1) + 1;



            comb = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//默认值
            if (self.GetComponent<enemy_robot>().animState.IsName(attack1State))
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist1[i];
                }
            else
                if (self.GetComponent<enemy_robot>().animState.IsName(attack2State))
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist2[i];
                }
            else
                if (self.GetComponent<enemy_robot>().animState.IsName(chargeattack1State) )
                for (int i = 0; i < comb.Length; i++)
                {
                    comb[i] = comblist5[i];
                }
            StartCoroutine(collision.GetComponent<getHurt>().Hurt(self, comb[numbTrue - 1], comb[numbTrue], comb[numbTrue + 1], comb[numbTrue + 2]));
            collision.gameObject.GetComponent<getHurt>().befucked = true;
            attacktarget.Add(collision.gameObject);
        }


    }

    public void clearTarget()
    {
        foreach (GameObject g in attacktarget)
        {
            g.GetComponent<getHurt>().befucked = false;


        }



    }



}
