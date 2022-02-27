using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] soundsArr;
    public AudioMixer audioMixer;
    protected override void OnAwake()
    {
        foreach(Sound sound in soundsArr)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
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

    public void Stop(string name)
    {
        Sound sound_ToStop = Array.Find(soundsArr, sound => sound.name == name); 
        if (sound_ToStop == null)
        {
            Debug.LogError("SOUND " + name + " CANT BE STOP, CHECK SPELLING OF THE SONG");
            return;
        }
        sound_ToStop.source.Stop();
        
    }

    public void StopAll()
    {
        foreach (Sound sound in soundsArr)
        {
            sound.source.Stop();
        }
    }

    private void Start()
    {
        
    }

    public Sound GetSound(string name)
    {
        Sound sound_ToPlay = Array.Find(soundsArr, sound => sound.name == name);
        if (sound_ToPlay == null)
        {
            Debug.LogError("SOUND " + name + " CANT BE RETURNED, CHECK SPELLING OF THE SONG");
            return null;
        }
        else
        {
            return sound_ToPlay;
        }
    }

    public void SetVolume(float vol,string name)
    {
        Sound sound_ToPlay = Array.Find(soundsArr, sound => sound.name == name);
        if(sound_ToPlay == null)
        {
            Debug.LogError("SOUND " + name + " CANT BE ADJUSTED, CHECK SPELLING OF THE SONG");
            return;
        }
        else
        {
            sound_ToPlay.source.volume = vol;
        }
    }

    void Update()
    {
        
    }
}
