using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

  public Vector2 A;//起点


  public Vector2 B;//终点

    //线的宽度
  public float Thickness;

    //组成线的包含片段的子物体 

  public GameObject StartCapChild, LineChild, EndCapChild;

    //新建线段
  public Line(Vector2 a, Vector2 b, float thickness)
    {
        A = a;
        B = b;
        Thickness = thickness;
    }

    //设置线的颜色
 public void SetColor(Color color)
    {
        StartCapChild.GetComponent<SpriteRenderer>().color = color;
        LineChild.GetComponent<SpriteRenderer>().color = color;
        EndCapChild.GetComponent<SpriteRenderer>().color = color;
    }



    public void Draw()
    {
        Vector2 difference = B - A;
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        //设置线的缩放来体现出长度和宽度
        LineChild.transform.localScale = new Vector3(100 * (difference.magnitude / LineChild.GetComponent<SpriteRenderer>().sprite.rect.width),
        Thickness,
        LineChild.transform.localScale.z);

        StartCapChild.transform.localScale = new Vector3(StartCapChild.transform.localScale.x,
        Thickness,
        StartCapChild.transform.localScale.z);

        EndCapChild.transform.localScale = new Vector3(EndCapChild.transform.localScale.x,
        Thickness,
        EndCapChild.transform.localScale.z);

        //旋转线让它朝向正确的方向
        LineChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        StartCapChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        EndCapChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation)); //如果头尾图片一样，rotation要+180

        //移动线段到以起点为中心
        LineChild.transform.position = new Vector3(A.x, A.y, LineChild.transform.position.z);
        StartCapChild.transform.position = new Vector3(A.x, A.y, StartCapChild.transform.position.z);
        EndCapChild.transform.position = new Vector3(A.x, A.y, EndCapChild.transform.position.z);

        //需要把旋转度数转化为弧度进行运算
        rotation *= Mathf.Deg2Rad;

        //存储这些值这样就只需访问一次
        float lineChildWorldAdjust = LineChild.transform.localScale.x * LineChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
        float startCapChildWorldAdjust = StartCapChild.transform.localScale.x * StartCapChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
        float endCapChildWorldAdjust = EndCapChild.transform.localScale.x * EndCapChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;

        //调整中间段到合适的位置
        LineChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * lineChildWorldAdjust,
        .01f * Mathf.Sin(rotation) * lineChildWorldAdjust,
        0);

        //调整开始段到合适的位置

        StartCapChild.transform.position -= new Vector3(.01f * Mathf.Cos(rotation) * startCapChildWorldAdjust,
        .01f * Mathf.Sin(rotation) * startCapChildWorldAdjust,
        0);

        //调整终点段到合适的位置

        EndCapChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * lineChildWorldAdjust * 2,
        .01f * Mathf.Sin(rotation) * lineChildWorldAdjust * 2,
        0);
        EndCapChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * endCapChildWorldAdjust,
        .01f * Mathf.Sin(rotation) * endCapChildWorldAdjust,
        0);
    }
}
