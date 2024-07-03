using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    HOME,
    PLAYING,
    PAUSE,
}

public enum LocalValues
{
    BESTSCORE,
}

public class Game_Mgr : MonoBehaviour
{
    public static Game_Mgr inst = null;
    public static GameStates State = GameStates.HOME;

    AudioSource adSource = null;
    Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (!inst)
            inst = this;
    }

    void Start()
    {
        if (!adSource)
            adSource = GetComponent<AudioSource>();

        object[] temp = Resources.LoadAll("Audio");
        AudioClip clip = null;
        for (int i = 0; i < temp.Length; i++)
        {
            clip = temp[i] as AudioClip;
            if (audioDict.ContainsKey(clip.name))
                continue;
            audioDict.Add(clip.name, clip);
        }
    }

    public void PlayAudio(string name)
    {
        adSource.PlayOneShot(audioDict[name], 1.0f);
    }
}
