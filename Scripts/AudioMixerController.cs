using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", MathF.Log10(sliderValue) * 20);
    }
}
