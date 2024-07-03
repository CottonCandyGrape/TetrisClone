using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public GameObject c1s;
    public GameObject c1e;

    public GameObject c2s;
    public GameObject c2e;

    float ani = 1.0f;
    float cac = 0.0f;
    float timer = 0.0f;

    public bool tri = false;

    Vector3 spos;

    void Start()
    {
        spos = c2s.transform.position;
    }

    void Update()
    {
        if (tri)
        {
            if (cac < 1.0f)
            {
                timer += Time.deltaTime;
                cac = timer / ani;

                Vector3 c1pos = c1s.transform.position;
                c1pos = Vector3.Lerp(c1s.transform.position, c1e.transform.position, cac);
                c1s.transform.position = c1pos;

                Vector3 c2pos = c2s.transform.position;
                c2pos = Vector3.Lerp(spos, c2e.transform.position, cac);
                c2s.transform.position = c2pos;

            }
        }
    }
}
