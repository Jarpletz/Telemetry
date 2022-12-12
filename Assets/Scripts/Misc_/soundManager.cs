using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    AudioSource source;
    float volume = 1;

    [System.Serializable]
    public class soundEffect
    {
        public AudioClip[] clips;
        public string soundName;
        public bool noOverlap;//Can it play over another instance of itself? true for no.
        public float overlapTime;//if true, the amount of time until it can play again
        //[HideInInspector]
        public float timer = 0;//time till can repeat again 
    }
    public soundEffect[] soundEffects;

    private void Start()
    {
        if (GetComponent<AudioSource>() == null)
        {
            source = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
        }
        else source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        foreach (soundEffect s in soundEffects)
        {
            if (s.noOverlap && s.timer > 0)
            {
                s.timer -= Time.deltaTime;
            }
        }
       
    }
    public void playSound(string soundName, float volumeMult = 1f, float pitch = 1f)
    {
        foreach (soundEffect s in soundEffects)
        {
            if (soundName == s.soundName)
            {
                if (s.timer <= 0)
                {
                    source.pitch = pitch;
                    source.PlayOneShot(s.clips[Random.Range(0, s.clips.Length)], volume * volumeMult*GlobalInfo.settings.sfxVolume);
                    if (s.noOverlap) s.timer = s.overlapTime;
                }
                return;
            }
        }

        Debug.LogError("Sound of name '" + soundName + "' not found!");
    }
    public void playSoundAnimation(string soundName)
    {
        foreach (soundEffect s in soundEffects)
        {
            if (soundName == s.soundName)
            {
                if (s.timer <= 0)
                {
                    source.PlayOneShot(s.clips[Random.Range(0, s.clips.Length)],GlobalInfo.settings.sfxVolume);
                    if (s.noOverlap) s.timer = s.overlapTime;
                }
                return;
            }
        }

        Debug.LogError("Sound of name '" + soundName + "' not found for " + gameObject.name + "!");
    }
    public soundEffect returnSound(string soundName)
    {
        foreach (soundEffect s in soundEffects)
        {
            if (s.soundName == soundName) return s;
        }
        Debug.LogWarning("Sound of name '" + soundName + "' not found for " + gameObject.name + "!");
        return null;
    }
}
