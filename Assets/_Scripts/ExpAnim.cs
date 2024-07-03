using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpAnim : MonoBehaviour
{
    Animator anim = null;

    void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            gameObject.SetActive(false);
    }
}
