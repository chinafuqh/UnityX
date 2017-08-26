using UnityEngine;
using System.Collections.Generic;

class LightingBolt : MonoBehaviour
{
    
    public List<GameObject> ActiveLineObj;
    public List<GameObject> InactiveLineObj;

    //线的预设
    public GameObject LinePrefab;

    //透明度
    public float Alpha { get; set; }

    //闪电淡出速度
    public float FadeOutRate { get; set; }

    //闪电颜色
    public Color Tint { get; set; }

    //闪电起始位置
    public Vector2 Start { get { return ActiveLineObj[0].GetComponent<Line>().A; } }

    //闪电终止位置
    public Vector2 End { get { return ActiveLineObj[ActiveLineObj.Count - 1].GetComponent<Line>().B; } }

    //闪电完全淡出时为真
    public bool IsComplete { get { return Alpha <= 0; } }

    public void Initialize(int maxSegments)
    {
        //初始化池的list
        ActiveLineObj = new List<GameObject>();
        InactiveLineObj = new List<GameObject>();

        for (int i = 0; i < maxSegments; i++)
        {
            //初始化Line的预设
            GameObject line = Instantiate(LinePrefab);

            //父物体设为闪电对象
            line.transform.parent = transform;

            //设为非激活
            line.SetActive(false);

            //加入到list里
            InactiveLineObj.Add(line);
        }
    }

    public void ActivateBolt(Vector2 source, Vector2 dest, Color color, float thickness)
    {
        //存储颜色
        Tint = color;

        //存储透明值
        Alpha = 1.5f;

        //存储淡出速度
        FadeOutRate = 0.05f;

        //实际创建闪电
        //防止生成长度为0的线段
        if (Vector2.Distance(dest, source) <= 0)
        {
            Vector2 adjust = Random.insideUnitCircle;
            if (adjust.magnitude <= 0) adjust.x += .1f;
            dest += adjust;
        }

        //源点和目标点的差值
        Vector2 slope = dest - source;
        Vector2 normal = (new Vector2(slope.y, -slope.x)).normalized;

        //源点和目标点的距离
        float distance = slope.magnitude;

        List<float> positions = new List<float>();
        positions.Add(0);

        for (int i = 0; i < distance / 4; i++)
        {
            //0到1之间随机生成位置来分割闪电
            //positions.Add (Random.Range(0f, 1f));
            positions.Add(Random.Range(.25f, .75f));
        }

        positions.Sort();

        const float Sway = 200;
        const float Jaggedness = 1 / Sway;

        //影响闪电传播的宽域
        float spread = 1.0f;

        //从源点开始
        Vector2 prevPoint = source;

        //没有之前的点，所以为0
        float prevDisplacement = 0;

        for (int i = 1; i < positions.Count; i++)
        {
            int inactiveCount = InactiveLineObj.Count;
            if(inactiveCount <= 0) break;
 
                float pos = positions[i];

    
                float scale = (distance * Jaggedness) * (pos - positions[i - 1]);

    //定义一个envelope。接近闪电中部的点能够离中心线更远
                float envelope = pos > 0.95f ? 20 * (1 - pos) : spread;

                float displacement = Random.Range(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;
 
//计算终点
                Vector2 point = source + (pos * slope) + (displacement * normal);


                activateLine(prevPoint, point, thickness);
                prevPoint = point;
                prevDisplacement = displacement;
        }


        activateLine(prevPoint, dest, thickness);



}
 
public void DeactivateSegments()
{
    for (int i = ActiveLineObj.Count - 1; i >= 0; i--)
    {
        GameObject line = ActiveLineObj[i];
        line.SetActive(false);
        ActiveLineObj.RemoveAt(i);
        InactiveLineObj.Add(line);
    }
}

void activateLine(Vector2 A, Vector2 B, float thickness)
{
int inactiveCount = InactiveLineObj.Count;
 
//有非激活对象的时候才能激活
if(inactiveCount <= 0) return;

//拿出GameObject
GameObject line = InactiveLineObj[inactiveCount - 1];
 
//设为激活的
line.SetActive(true);
 
//获取Line组件
Line lineComponent = line.GetComponent<Line>();
lineComponent.SetColor(Color.white);
lineComponent.A = A;
lineComponent.B = B;
lineComponent.Thickness = thickness;
InactiveLineObj.RemoveAt(inactiveCount - 1);
ActiveLineObj.Add(line);
}
 
public void Draw()
{
    //如果闪电已经淡出，就不需要绘制了
    if (Alpha <= 0) return;

    foreach (GameObject obj in ActiveLineObj)
    {
        Line lineComponent = obj.GetComponent<Line>();
        lineComponent.SetColor(Tint * (Alpha * 0.6f));
        lineComponent.Draw();
    }
}


    public void UpdateBolt()
    {
        Alpha -= FadeOutRate;
    }


    public Vector2 GetPoint(float position)
    {
        Vector2 start = Start;
        float length = Vector2.Distance(start, End);
        Vector2 dir = (End - start) / length;
        position *= length;

        //找出合适的线。
        Line line = ActiveLineObj.Find(x => Vector2.Dot(x.GetComponent<Line>().B - start, dir) >= position).GetComponent<Line>();
        float lineStartPos = Vector2.Dot(line.A - start, dir);
        float lineEndPos = Vector2.Dot(line.B - start, dir);
        float linePos = (position - lineStartPos) / (lineEndPos - lineStartPos);

        return Vector2.Lerp(line.A, line.B, linePos);
    }

    //...
}
