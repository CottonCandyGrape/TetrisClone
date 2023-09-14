using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public Transform rotationPivot;
    float elapsedTime = 0f;
    float fallTime = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(rotationPivot.position, new Vector3(0,0,1), 90f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -1f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1f, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1f, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        //저절로 내려오기 
        //if (elapsedTime >= fallTime)
        //{
        //    transform.position += new Vector3(0, -1f, 0);
        //    elapsedTime = 0f;
        //}
        //else
        //{
        //    elapsedTime += Time.deltaTime;
        //}
    }
}
