using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Mgr : MonoBehaviour
{
    Camera cam = null;

    float camStartSize = 13.5f;
    float camEndSize = 13.0f;

    Vector3 camStartPos = new Vector3(4.5f, 10.0f, -10.0f);
    Vector3 camEndPos = new Vector3(4.5f, 11.0f, -10.0f);
    Vector3 curCamPos;

    float moveSpeed = 5.0f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        //if (Game_Mgr.State == GameStates.HOME || Game_Mgr.State == GameStates.PAUSE) //out -> in
        //    CamIn();
        //else if (Game_Mgr.State == GameStates.PLAYING) //in -> out
        //    CamOut();
    }

    void CamOut()
    {
        if (cam.orthographicSize > camEndSize)
            cam.orthographicSize -= moveSpeed * Time.deltaTime;
        else if (cam.orthographicSize <= camEndSize)
            cam.orthographicSize = camEndSize;

        if (cam.transform.position.y < camEndPos.y)
        {
            curCamPos = cam.transform.position;
            curCamPos.y += moveSpeed * Time.deltaTime;
            cam.transform.position = curCamPos;
        }
        else if (cam.transform.position.y >= camEndPos.y)
            cam.transform.position = camEndPos;
    }

    void CamIn()
    {
        if (cam.orthographicSize < camStartSize)
            cam.orthographicSize += moveSpeed * Time.deltaTime;
        else if (cam.orthographicSize >= camStartSize)
            cam.orthographicSize = camStartSize;

        if (cam.transform.position.y > camStartPos.y)
        {
            curCamPos = cam.transform.position;
            curCamPos.y -= moveSpeed * Time.deltaTime;
            cam.transform.position = curCamPos;
        }
        else if (cam.transform.position.y <= camStartPos.y)
            cam.transform.position = camStartPos;
    }
}
