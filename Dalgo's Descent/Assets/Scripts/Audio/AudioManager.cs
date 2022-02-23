using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] soundsArr;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound sound in soundsArr)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    public void Play(string name)
    {
        Sound sound_ToPlay = Array.Find(soundsArr, sound => sound.name == name); //looping thru the array to find the sound
        if (sound_ToPlay == null)
        {
            Debug.LogError("SOUND " + name + " CANT BE PLAYED, CHECK SPELLING OF THE SONG");
            return;
        }
        sound_ToPlay.source.Play();
    }

    private void Start()
    {
        Play("Testing");
    }


    void Update()
    {
        
    }
}
