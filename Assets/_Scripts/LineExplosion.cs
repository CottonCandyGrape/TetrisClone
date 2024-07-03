using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineExplosion : MonoBehaviour
{
    public GameObject[] LineExp = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (IsDone()) gameObject.SetActive(false);
    }

    bool IsDone()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return false;
        }

        return true;
    }

    public void OnLineEffect()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
}
