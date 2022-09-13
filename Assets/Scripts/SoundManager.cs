using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioClip defaultBGM;
    private static SoundManager _instance;
    private readonly string sfxTag = "SFX";

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static SoundManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Start()
    {
        if (!bgmSource)
        {
            bgmSource = this.gameObject.AddComponent<AudioSource>();
            bgmSource.clip = defaultBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void Update()
    {
        // Clear all unused SFX components
        GameObject[] effects = GameObject.FindGameObjectsWithTag(sfxTag);
        foreach(GameObject effect in effects)
        {
            if (!effect.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(effect);
            }
        }
    }

    public void PlayEffect(string effectName, float volumn = 1f)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(LocalPath.sounds + effectName);
        if (audioClip != null)
        {
            AudioSource audioSource = Instantiate(new GameObject(), this.transform).AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = volumn;
            audioSource.gameObject.tag = sfxTag;
            audioSource.Play();
        }
    }

    public void ChangeBGMVolumn(float volumn)
    {
        bgmSource.volume = volumn;
    }

    public void ChangeBGMClip(AudioClip clip)
    {
        bgmSource.clip = clip;
    }
}
