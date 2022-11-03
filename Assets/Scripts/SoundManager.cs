using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioClip defaultBGM;
    static float BGMVolume = 1f;
    static float effectVolume = 1f;
    static SoundManager _instance;
    public static readonly string sfxTag = "SFX";

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
            ChangeBGMVolume(BGMVolume);
            ChangeEffectVolume(effectVolume);
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

    public void PlayEffect(string effectName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(LocalPath.sounds + effectName);
        if (audioClip != null)
        {
            AudioSource audioSource = Instantiate(new GameObject(), this.transform).AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = effectVolume;
            audioSource.gameObject.tag = sfxTag;
            audioSource.Play();
        }
    }

    public void PlayEffect(string effectName, float volume = 1f)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(LocalPath.sounds + effectName);
        if (audioClip != null)
        {
            AudioSource audioSource = Instantiate(new GameObject(), this.transform).AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = volume;
            audioSource.gameObject.tag = sfxTag;
            audioSource.Play();
        }
    }

    public void ChangeBGMVolume(float volume)
    {
        BGMVolume = volume;
        bgmSource.volume = volume;
    }

    public void ChangeEffectVolume(float volume)
    {
        effectVolume = volume;
        GameObject[] effects = GameObject.FindGameObjectsWithTag(sfxTag);
        foreach (GameObject effect in effects)
        {
            effect.GetComponent<AudioSource>().volume = volume;
        }
    }

    public void ChangeBGMClip(AudioClip clip)
    {
        bgmSource.clip = clip;
    }
}
