using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockController : MonoBehaviour
{
    public Transform rotationPivot;

    const float centerPos = 4.5f; 


    bool isDrop = false;
    float fallTime = 1f;
    float lastFallTime;

    Grid_Mgr gridManager;
    Spawn_Mgr spawnManager;

    void Start()
    {
        //TODO : 매번 찾는데 싱글톤? 프로퍼티? 로 바꿀까?
        gridManager = GameObject.Find("Grid_Mgr").GetComponent<Grid_Mgr>();
        spawnManager = GameObject.Find("Spawn_Mgr").GetComponent<Spawn_Mgr>();

        lastFallTime = Time.time;

        if (IsDie()) //TODO: die가 아니고 valid??
        {
            Game_Mgr.inst.PlayAudio("GameOver");
            Time.timeScale = 0.0f;
        }
    }

    void Update()
    {
        if (Game_Mgr.State == GameStates.PAUSE) return;

        if (!isDrop)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                RotateBlock();
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                MoveLeft();
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                MoveRight();
            //else if (Input.GetKeyDown(KeyCode.DownArrow))
            //    MoveDown();
        }

        if (Time.time - lastFallTime >= fallTime || Input.GetKeyDown(KeyCode.DownArrow))
            MoveDown();

        if (Input.GetKeyDown(KeyCode.Space)) // TODO : 확 내려가기 효과
            DropBlock();
    }

    public void RotateBlock()
    {
        transform.RotateAround(rotationPivot.position, new Vector3(0, 0, 1), -90);
        if (!IsValid())
        {
            if (transform.position.x < centerPos)
            {
                while (true)
                {
                    bool flag = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (!transform.GetChild(i).CompareTag("Block")) continue;

                        int posX = Mathf.RoundToInt(transform.GetChild(i).position.x);
                        if (posX < 0)
                        {
                            Vector2 tmp = transform.position;
                            tmp += Vector2.right;
                            transform.position = tmp;

                            flag = true;
                        }
                    }

                    if (!flag) break;
                }
            }
            else if (transform.position.x > centerPos)
            {
                while (true)
                {
                    bool flag = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (!transform.GetChild(i).CompareTag("Block")) continue;

                        int posX = Mathf.RoundToInt(transform.GetChild(i).position.x);
                        if (posX > 9)
                        {
                            Vector2 tmp = transform.position;
                            tmp += Vector2.left;
                            transform.position = tmp;

                            flag = true;
                        }
                    }

                    if (!flag) break;
                }
            }
        }
    }

    public void DropBlock()
    {
        fallTime = 0.03f;
        isDrop = true;
    }

    void MoveDown()
    {
        Game_Mgr.inst.PlayAudio("Drop");

        transform.position += new Vector3(0, -1, 0);
        if (!IsValid())
        {
            this.enabled = false; //이게 없으면 script 계속 돌아서 계속 spawn 됨
            transform.position -= new Vector3(0, -1, 0);
            if (IsDie())
            {
                //transform.position -= new Vector3(0, -1, 0);
                Game_Mgr.inst.PlayAudio("GameOver");
                Time.timeScale = 0.0f;
                return;
            }
            gridManager.UpdateGrid(transform);
            spawnManager.SpawnBlock();
        }

        lastFallTime = Time.time;
    }

    public void MoveLeft() //TODO : MoveHorizontal 로 합칠 수 있음
    {
        transform.position += new Vector3(-1, 0, 0);
        if (!IsValid())
            transform.position -= new Vector3(-1, 0, 0);
    }

    public void MoveRight()
    {
        transform.position += new Vector3(1, 0, 0);
        if (!IsValid())
            transform.position -= new Vector3(1, 0, 0);
    }

    bool IsValid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "Block") continue; // pivot 제외 

            int posX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int posY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            if (posX < 0 || posX > 9 || posY < 0) return false;

            if (gridManager.grid[posX, posY] != null) return false;
        }

        return true;
    }

    //TODO : 죽은 뒤에 삐져나온 블록 안보이게 하기
    bool IsDie()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag != "Block") continue;
            int posX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int posY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            if (posY > 19 || gridManager.grid[posX, posY] != null) // Die 조건
                return true;
        }

        return false;
    }
}
