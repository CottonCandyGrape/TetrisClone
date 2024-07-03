using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Mgr : MonoBehaviour
{
    public Transform blockStack;
    public Transform[,] grid = new Transform[10, 20];
    public EffectMgr effMgr = null;

    void Start() { }

    //void Update()
    //{
    //}

    public void UpdateGrid(Transform block)
    {
        RemovePivot(); //정확히는 이전 턴에서 생성된 빈블록 지우는 것. 
        //같은 frame에서 destory가 적용 안되기 때문
        //그리고 한턴 늦게 지운다고 문제될 것이 없다.

        for (int i = 0; i < block.childCount; i++)
        {
            Transform child = block.GetChild(i);

            if (child.tag != "Block") continue;

            int posX = Mathf.RoundToInt(child.position.x);
            int posY = Mathf.RoundToInt(child.position.y);

            if (grid[posX, posY] == null)
                grid[posX, posY] = child;
        }

        CheckGrid();
    }

    void PopLineEffect(int posY)
    {
        GameObject lineEffect = effMgr.AddLineEffect();
        lineEffect.SetActive(true);
        Vector2 tmp = lineEffect.transform.position;
        tmp.y = posY;
        lineEffect.transform.position = tmp;

        LineExplosion line = lineEffect.GetComponent<LineExplosion>();
        line.OnLineEffect();
    }

    void DownLine(int r)
    {
        for (int y = r; y < 19; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (grid[x, y + 1] != null)
                {
                    Vector3 pos = grid[x, y + 1].position;
                    pos -= new Vector3(0, 1f, 0);
                    grid[x, y + 1].position = pos;
                }
                grid[x, y] = grid[x, y + 1];
            }
        }
    }

    void DeleteLine(int y)
    {
        PopLineEffect(y);

        for (int x = 0; x < 10; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        UI_Mgr.inst.AddScore(10);
    }

    bool HasLine(int y)
    {
        for (int x = 0; x < 10; x++)
        {
            if (grid[x, y] == null)
                return false;
        }

        return true;
    }

    void CheckGrid() //TODO : 연속된 줄이 꽉 찼을 경우 개선하기
    {
        bool line = false;
        for (int i = 0; i < 20; i++)
        {
            if (HasLine(i))
            {
                line = true;
                DeleteLine(i);
                if (i < 19) //마지막 줄은 안내려줘도 된다.
                    DownLine(i);
                i--; //뭔가 구리다..임시 방편. (이부분)
            }
        }

        if (line)
            Game_Mgr.inst.PlayAudio("Lineclear");
    }

    void RemovePivot()
    {
        for (int i = 0; i < blockStack.childCount; i++)
        {
            Transform tmp = blockStack.GetChild(i);
            if (tmp.childCount <= 1) //pivot만 남았으면
            {
                Destroy(tmp.gameObject);
            }
        }
    }
}
