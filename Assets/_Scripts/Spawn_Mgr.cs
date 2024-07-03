using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Mgr : MonoBehaviour
{
    public GameObject[] blockShapes;
    public Transform blockStack;

    void Start() { } 

    public void SpawnBlock()
    {
        int ran = Random.Range(0, blockShapes.Length);
        Instantiate(blockShapes[ran], blockStack);
    }
}
