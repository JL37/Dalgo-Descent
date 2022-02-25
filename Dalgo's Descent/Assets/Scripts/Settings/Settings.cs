using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Settings : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetVolume(volume, "Testing");
    }

    public void SetQuality(int qualityIndex) //this does not work hmmm
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
