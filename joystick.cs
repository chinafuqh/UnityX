using UnityEngine;
using System.Collections;
using System;


/// <summary>  
/// 测试游戏手柄键值  
/// </summary>  
public class joystick : MonoBehaviour
{
    private string currentButton;//当前按下的按键  

    // Use this for initialization   
    void Start()
    {

    }
    // Update is called once per frame   
    void Update()
    {
        var values = Enum.GetValues(typeof(KeyCode));//存储所有的按键  
        for (int x = 0; x < values.Length; x++)
        {
            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                currentButton = values.GetValue(x).ToString();//遍历并获取当前按下的按键  
            }
        }
    }
    // Show some data   
    void OnGUI()
    {
        GUI.TextArea(new Rect(0, 0, 250, 40), "Current Button : " + currentButton);//使用GUI在屏幕上面实时打印当前按下的按键  
    }
}
