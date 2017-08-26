using UnityEngine;
using System.Collections;

public class damageShow : MonoBehaviour
{

    //目标位置  
    private Vector3 mTarget;
    //屏幕坐标  
    private Vector3 mScreen;
    //伤害数值  
    public int Value;


    //文本宽度  
    public float ContentWidth = 100;
    //文本高度  
    public float ContentHeight = 50;

    public float m_fScreenWidth = 800;
    public float m_fScreenHeight = 1280;

    // scale factor
    private float m_fScaleWidth;
    private float m_fScaleHeight;


    public Font customFont;
    public Color color;
    public Color color2;
    //GUI坐标  
    private Vector2 mPoint;
    private GUIStyle bb;
    private GUIStyle cc;
    private float R = 1;
    private float G = 0;
    private float B = 0;
    private float A = 1;

    //销毁时间  
    public float FreeTime = 1.5F;

    private void Awake()
    {
        m_fScaleWidth = Screen.width / m_fScreenWidth;
        m_fScaleHeight = Screen.height / m_fScreenHeight;
    }


    void Start()
    {

        //获取目标位置  
        mTarget = transform.position;
        //获取屏幕坐标  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //开启自动销毁线程  
        StartCoroutine("Free");
        bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = color;
        bb.fontSize = Mathf.Max(25 * (int)(m_fScaleWidth + m_fScaleHeight) / 2, 25);
        bb.font = customFont;
        cc = new GUIStyle();
        cc.normal.background = null;    //这是设置背景填充的
        cc.normal.textColor = color2;
        cc.fontSize = Mathf.Max(25 * (int)(m_fScaleWidth + m_fScaleHeight) / 2, 25);
        cc.font = customFont;
    }

    void Update()
    {
        //使文本在垂直方向山产生一个偏移  
        transform.Translate(Vector3.up * 3f * Time.deltaTime);
        //重新计算坐标  
        mTarget = transform.position;
        //获取屏幕坐标  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);

        A = Mathf.Max(A - 0.03f, 0);

        bb.normal.textColor = new Color(color.r, color.g, color.b, A);

        cc.normal.textColor = new Color(color2.r, color2.g, color2.b, A);
    }

    void OnGUI()
    {
        //保证目标在摄像机前方  
        //内部使用GUI坐标进行绘制  



        string value = Value.ToString();
        Rect rect = new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight);
        //GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), Value.ToString(), bb);d
        MakeStroke(rect, value, color, color2, 2);

    }

    IEnumerator Free()
    {
        yield return new WaitForSeconds(FreeTime);
        Destroy(this.gameObject);
    }

    private void MakeStroke(Rect position, string txtString, Color txtColor, Color outlineColor, int outlineWidth)
    {

        position.y -= outlineWidth;
        GUI.color = outlineColor;
        GUI.Label(position, txtString, cc);
        position.y += outlineWidth * 2;
        GUI.Label(position, txtString, cc);
        position.y -= outlineWidth;
        position.x -= outlineWidth;
        GUI.Label(position, txtString, cc);
        position.x += outlineWidth * 2;
        GUI.Label(position, txtString, cc);
        position.x -= outlineWidth;
        GUI.color = txtColor;
        GUI.Label(position, txtString, bb);
    }
}
