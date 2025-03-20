using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private List<AudioSource> Sfx;
    [SerializeField] private List<AudioSource> BGM;
    private Transform sfxHolder;
    private Transform bgmHolder;
    public int currentBGM;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        sfxHolder = transform.Find("SFX");
        bgmHolder = transform.Find("BGM");
        LoadAudio();
    }
    private void Reset()
    {
        sfxHolder = transform.Find("SFX");
        bgmHolder = transform.Find("BGM");
        LoadAudio();
    }
    private void LoadAudio()
    {
        Debug.Log("Load Audio!");
        Sfx.Clear();
        BGM.Clear();
        foreach(Transform source in sfxHolder)
        {
            Sfx.Add(source.GetComponent<AudioSource>());
        }
        foreach (Transform source in bgmHolder)
        {
            BGM.Add(source.GetComponent<AudioSource>());
        }

    }
    private void Start()
    {
        if(currentBGM != -1)
        {
            PlayBGM(currentBGM);
        }
    }
    public void PlaySFX(int _index)
    {
        if(_index < Sfx.Count)
            Sfx[_index].Play();
    }
    public void StopSFX(int _index)
    {
        Sfx[_index].Stop();
    }
    public void PlayBGM(int _index)
    {
        if(_index < BGM.Count)
        {
            StopAllBGM();
            BGM[_index].Play();
            currentBGM = _index;
        }
    }
    public void StopAllBGM()
    {
        foreach (var m in BGM)
        {
            m.Stop();
            currentBGM = -1;
        }
    }
}
