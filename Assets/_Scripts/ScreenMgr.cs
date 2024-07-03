using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMgr : MonoBehaviour
{
    const int width = 9;
    const int height = 16;

    Camera cam = null;

    public static ScreenMgr Inst = null;

    void Awake()
    {
        Inst = this;

        cam = Camera.main;

        Rect rect = cam.rect;

        //기기 화면비
        float deviceRatio = (float)Screen.width / Screen.height;
        //원하는 화면비
        float targetRatio = (float)width / height;

        //Viewport Coords에서의 Height, Width 크기 (0f~1f)
        float scaleHeight = deviceRatio / targetRatio;
        float scaleWidth = 1.0f / scaleHeight;

        if (scaleHeight < 1.0f) //세로가 더 큰경우(==위아래에 레터박스 생김)
        {
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else //가로가 더 큰경우(==좌우에 레터박스 생김)
        {
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleWidth) / 2.0f;
        }

        cam.rect = rect;
    }
}
