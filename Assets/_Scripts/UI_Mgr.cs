using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mgr : MonoBehaviour
{
    [Header("Logo UI")]
    public GameObject Logo = null;

    Vector3 logoStartPos = new Vector3(0, 805f, 0);
    Vector3 logoEndPos = new Vector3(0, 1060f, 0);
    Vector3 curLogoPos;

    [Header("KeyPad UI")]
    public GameObject KeyPadUI = null;
    public Button Left_Btn = null;
    public Button Drop_Btn = null;
    public Button Rot_Btn = null;
    public Button Right_Btn = null;
    Vector3 curKeyPadPos;
    Transform blockStack = null;

    [Header("Main UI")]
    public GameObject MainUI = null;
    public Button Play_Btn = null;
    public Button Reset_Btn = null;
    public Button Setting_Btn = null;
    public Button Records_Btn = null;

    Vector3 mainUIStartPos = new Vector3(0, -850f, 0);
    Vector3 mainUIEndPos = new Vector3(0, -1105f, 0);
    Vector3 curMainPos;

    [Header("Score UI & Pause")]
    public GameObject ScoreUI = null;
    public Text Score_txt = null;
    public Text BestScore_txt = null;
    public Button Pause_Btn = null;

    Vector3 scoreUIStartPos = new Vector3(0, 1100f, 0);
    Vector3 scoreUIEndPos = new Vector3(0, 810f, 0);
    Vector3 curScorePos;

    int maxVal = int.MaxValue - 20;
    int score = 0;
    int bestScore = 0;

    float moveSpeed = 1500.0f;

    public static UI_Mgr inst = null;
    Camera_Mgr camMgr = null;
    Spawn_Mgr spawnMgr = null;

    void Awake()
    {
        if (!inst)
            inst = this;
    }

    void Start()
    {
        if (Play_Btn != null)
            Play_Btn.onClick.AddListener(PlayBtnClick);

        if (Pause_Btn != null)
            Pause_Btn.onClick.AddListener(PauseBtnClick);

        if (Left_Btn != null)
            Left_Btn.onClick.AddListener(LeftBtnClick);

        if (Drop_Btn != null)
            Drop_Btn.onClick.AddListener(DropBtnClick);
        
        if (Rot_Btn != null)
            Rot_Btn.onClick.AddListener(RotBtnClick);

        if (Right_Btn != null)
            Right_Btn.onClick.AddListener(RightBtnClick);

        //TODO : Setting, Records Btn Listner 추가하기

        if (Reset_Btn != null)
            Reset_Btn.onClick.AddListener(ResetBtnClick);

        if (camMgr == null)
            camMgr = Camera.main.GetComponent<Camera_Mgr>();

        if (spawnMgr == null)
            spawnMgr = FindObjectOfType<Spawn_Mgr>();

        bestScore = PlayerPrefs.GetInt(LocalValues.BESTSCORE.ToString(), 0);
        BestScore_txt.text = bestScore.ToString();

        blockStack = GameObject.Find("BlockStack").transform;
    }

    void Update()
    {
        if (Game_Mgr.State == GameStates.HOME || Game_Mgr.State == GameStates.PAUSE)
        {
            LogoOn();
            MainUIOn();
            KeyPadUIOff();
        }
        else if (Game_Mgr.State == GameStates.PLAYING)
        {
            LogoOff();
            MainUIOff();
            KeyPadUIOn();
            ScoreUIOn();
        }
    }

    BlockController GetCurblock()
    {
        int blockCnt = blockStack.childCount;
        Transform block = blockStack.GetChild(blockCnt - 1);
        BlockController blockCtrl = block.GetComponent<BlockController>();
        if (blockCtrl != null) return blockCtrl;
        else return null;
    }

    void LeftBtnClick()
    {
        BlockController blockCtrl = GetCurblock();
        if (blockCtrl != null) blockCtrl.MoveLeft();
    }

    void DropBtnClick()
    {
        BlockController blockCtrl = GetCurblock();
        if (blockCtrl != null) blockCtrl.DropBlock();
    }

    void RotBtnClick()
    {
        BlockController blockCtrl = GetCurblock();
        if (blockCtrl != null) blockCtrl.RotateBlock();
    }

    void RightBtnClick()
    {
        BlockController blockCtrl = GetCurblock();
        if (blockCtrl != null) blockCtrl.MoveRight();
    }

    void PlayBtnClick()
    {
        if (Game_Mgr.State == GameStates.HOME)
            spawnMgr.SpawnBlock();

        //Time.timeScale = 1.0f; //TODO : timescale 0으로 만드는 부분이 없어서 지금 필요는 없다.
        Game_Mgr.State = GameStates.PLAYING;
        //Restart_Btn.gameObject.SetActive(false);

        Game_Mgr.inst.PlayAudio("GameStart");
    }

    void ResetBtnClick()
    {
        Game_Mgr.inst.PlayAudio("Pause");
        PlayerPrefs.DeleteAll();
        //if (ScoreUI.activeSelf)
        {
            score = 0;
            Score_txt.text = score.ToString();

            bestScore = PlayerPrefs.GetInt(LocalValues.BESTSCORE.ToString(), 0);
            BestScore_txt.text = bestScore.ToString();
        }
    }

    void PauseBtnClick()
    {
        //Time.timeScale = 0.0f;
        Game_Mgr.State = GameStates.PAUSE;

        //Restart_Btn.gameObject.SetActive(true);

        Game_Mgr.inst.PlayAudio("Pause");
    }

    void LogoOff()
    {
        if (Logo.transform.localPosition.y < logoEndPos.y)
        {
            curLogoPos = Logo.transform.localPosition;
            curLogoPos.y += moveSpeed * Time.deltaTime;
            Logo.transform.localPosition = curLogoPos;
        }
        else if (Logo.transform.localPosition.y >= logoEndPos.y)
        {
            Logo.transform.localPosition = logoEndPos;
            Logo.SetActive(false);

            Pause_Btn.gameObject.SetActive(true);
        }
    }

    void LogoOn()
    {
        if (!Logo.activeSelf)
            Logo.SetActive(true);

        if (Pause_Btn.gameObject.activeSelf)
            Pause_Btn.gameObject.SetActive(false);

        if (ScoreUI.activeSelf)
            ScoreUIOff();

        if (Logo.transform.localPosition.y > logoStartPos.y)
        {
            curLogoPos = Logo.transform.localPosition;
            curLogoPos.y -= moveSpeed * Time.deltaTime;
            Logo.transform.localPosition = curLogoPos;
        }
        else if (Logo.transform.localPosition.y <= logoStartPos.y)
            Logo.transform.localPosition = logoStartPos;
    }

    void MainUIOff()
    {
        if (MainUI.transform.localPosition.y > mainUIEndPos.y)
        {
            curMainPos = MainUI.transform.localPosition;
            curMainPos.y -= moveSpeed * Time.deltaTime;
            MainUI.transform.localPosition = curMainPos;
        }
        else if (MainUI.transform.localPosition.y <= mainUIEndPos.y)
        {
            MainUI.transform.localPosition = mainUIEndPos;
            MainUI.SetActive(false);
        }
    }

    void MainUIOn()
    {
        if (!MainUI.activeSelf)
            MainUI.SetActive(true);

        if (MainUI.transform.localPosition.y < mainUIStartPos.y)
        {
            curMainPos = MainUI.transform.localPosition;
            curMainPos.y += moveSpeed * Time.deltaTime;
            MainUI.transform.localPosition = curMainPos;
        }
        else if (MainUI.transform.localPosition.y >= mainUIStartPos.y)
            MainUI.transform.localPosition = mainUIStartPos;
    }

    void KeyPadUIOff()
    {
        if (KeyPadUI.transform.localPosition.y > mainUIEndPos.y)
        {
            curKeyPadPos = KeyPadUI.transform.localPosition;
            curKeyPadPos.y -= moveSpeed * Time.deltaTime;
            KeyPadUI.transform.localPosition = curKeyPadPos;
        }
        else if (KeyPadUI.transform.localPosition.y <= mainUIEndPos.y)
        {
            KeyPadUI.transform.localPosition = mainUIEndPos;
            KeyPadUI.SetActive(false);
        }
    }

    void KeyPadUIOn()
    {
        if (!KeyPadUI.activeSelf)
            KeyPadUI.SetActive(true);

        if (KeyPadUI.transform.localPosition.y < mainUIStartPos.y)
        {
            curKeyPadPos = KeyPadUI.transform.localPosition;
            curKeyPadPos.y += moveSpeed * Time.deltaTime;
            KeyPadUI.transform.localPosition = curKeyPadPos;
        }
        else if (KeyPadUI.transform.localPosition.y >= mainUIStartPos.y)
            KeyPadUI.transform.localPosition = mainUIStartPos;
    }

    void ScoreUIOn()
    {
        if (!ScoreUI.activeSelf)
            ScoreUI.SetActive(true);

        if (ScoreUI.transform.localPosition.y > scoreUIEndPos.y)
        {
            curScorePos = ScoreUI.transform.localPosition;
            curScorePos.y -= moveSpeed * Time.deltaTime;
            ScoreUI.transform.localPosition = curScorePos;
        }
        else if (ScoreUI.transform.localPosition.y <= scoreUIEndPos.y)
            ScoreUI.transform.localPosition = scoreUIEndPos;
    }

    void ScoreUIOff()
    {
        ScoreUI.transform.localPosition = scoreUIStartPos;
        ScoreUI.SetActive(false);
    }

    public void AddScore(int num)
    {
        score += num;

        if (maxVal < score)
            score = maxVal;

        if (bestScore < score)
        {
            bestScore = score;
            BestScore_txt.text = bestScore.ToString();
            PlayerPrefs.SetInt(LocalValues.BESTSCORE.ToString(), bestScore);
        }

        Score_txt.text = score.ToString();
    }
}