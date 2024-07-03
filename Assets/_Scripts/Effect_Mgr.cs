using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMgr : MonoBehaviour
{
    public Transform EffectPool = null;
    public GameObject[] TetrisEffects = null;
    List<GameObject> effects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < TetrisEffects.Length; i++)
        {
            GameObject eft = Instantiate(TetrisEffects[i], EffectPool);
            eft.SetActive(false);
            effects.Add(eft);
        }
    }

    //void Update() { }

    public GameObject AddLineEffect()
    {
        List<int> randList = GetInActiveIdx();
        if (randList.Count > 0)
        {
            int idx = Random.Range(0, randList.Count);
            return effects[randList[idx]];
        }

        GameObject eft = Instantiate(TetrisEffects[Random.Range(0, TetrisEffects.Length)], EffectPool);
        eft.SetActive(false);
        effects.Add(eft);

        return eft;
    }

    List<int> GetInActiveIdx()
    {
        List<int> result = new List<int>();

        for (int i = 0; i < effects.Count; i++)
        {
            if (!effects[i].gameObject.activeSelf)
                result.Add(i);
        }

        return result;
    }
}
