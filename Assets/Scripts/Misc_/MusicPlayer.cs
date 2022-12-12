using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> songs = new List<AudioClip>();
    [SerializeField] float initialVolume;
    [SerializeField] float fadeSpeed;

    AudioSource audioSource;
    GameManager gm;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        audioSource.clip = songs[Random.Range(0, songs.Count)];
        audioSource.Play();
    }
    private void Update()
    {
        
        if (gm.isDead)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, fadeSpeed);
            return;
        }else
        audioSource.volume = initialVolume * GlobalInfo.settings.musicVolume;
    }
}
