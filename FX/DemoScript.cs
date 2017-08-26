using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoScript : MonoBehaviour
{
    //要在编辑器里指定预设
    public GameObject BoltPrefab;

    //对象池
    List<GameObject> activeBoltsObj;
    List<GameObject> inactiveBoltsObj;
    int maxBolts = 1000;

    //用来处理鼠标点击
    int clicks = 0;
    Vector2 pos1, pos2;

    void Start()
    {
        //初始化list
        activeBoltsObj = new List<GameObject>();
        inactiveBoltsObj = new List<GameObject>();

        //找到存储闪电池的父物体
        GameObject p = GameObject.Find("LightningPoolHolder");

        //指定闪电数量
        for (int i = 0; i < maxBolts; i++)
        {
    GameObject bolt = (GameObject) Instantiate(BoltPrefab);

    //指定父物体
    bolt.transform.parent = p.transform;
 
//用预先设置的最大数量初始化我们的闪电
    bolt.GetComponent<LightingBolt>().Initialize(25);

    //设置初始为非激活
    bolt.SetActive(false);
 
//存储到非激活list中
    inactiveBoltsObj.Add(bolt);
}
}
 
void FixedUpdate()
{
    //为后面的使用声明变量
    GameObject boltObj;
    LightingBolt boltComponent;
    GameObject p = GameObject.Find("LightningPoolHolder");

        //存储被激活的对象的数量
        int activeLineCount = activeBoltsObj.Count;

    //循环遍历激活的线（逆向遍历，因为我们将要从list中移除）
    for (int i = activeLineCount - 1; i >= 0; i--)
    {
        //拿出GameObject
        boltObj = activeBoltsObj[i];

    //获取LightnBolt组件
         boltComponent = boltObj.GetComponent<LightingBolt>();
 
//如果闪电已经淡出
if(boltComponent.IsComplete)
{
//禁用它包含的分段
boltComponent.DeactivateSegments();
 
//把它设置为非激活
boltObj.SetActive(false);
 
//移到非激活list里
activeBoltsObj.RemoveAt(i);
inactiveBoltsObj.Add(boltObj);
boltObj.transform.position = p.transform.position;
}
}


 
//如果鼠标左键按下
/*if(Input.GetMouseButtonDown(0))
{
//如果第一次点击
if(clicks == 0)
{
//存储开始位置
Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
pos1 = new Vector2(temp.x, temp.y);
}
else if(clicks == 1) //second click //第二次点击
{
//存储终点位置
Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
pos2 = new Vector2(temp.x, temp.y);

//从pos1到pos2激活闪电池里的闪电
CreatePooledBolt(pos1, pos2, Color.white, 1f);

            }
 
//递增点击次数
clicks++;
 
//在两次点击后重置次数
if(clicks > 1) clicks = 0;
}
*/


 
//更新和绘制激活的闪电
for(int i = 0; i<activeBoltsObj.Count; i++)
{
activeBoltsObj[i].GetComponent<LightingBolt>().UpdateBolt();
activeBoltsObj[i].GetComponent<LightingBolt>().Draw();
}
}






    public void CreatePooledBolt(Vector2 source, Vector2 dest, Color color, float thickness)
{
    //如果闪电池中有非激活的闪电则取出
    if (inactiveBoltsObj.Count > 0)
    {
        //取出GameObject
        GameObject boltObj = inactiveBoltsObj[inactiveBoltsObj.Count - 1];

       //设为激活
        boltObj.SetActive(true);

      //加入avtive list
        activeBoltsObj.Add(boltObj);
        inactiveBoltsObj.RemoveAt(inactiveBoltsObj.Count - 1);

        //获取bolt组件
        LightingBolt boltComponent = boltObj.GetComponent<LightingBolt>();

        //用给定的位置数据激活闪电
        boltComponent.ActivateBolt(source, dest, color, thickness);
        boltComponent.Draw();


        }
}
}