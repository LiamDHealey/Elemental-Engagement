using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static bool fullScreen
    {
        get => Screen.fullScreen;
        set => Screen.fullScreen = value;
    }
    public static float uiScale { get; set; } = 1f;

    public static bool tooltips { get; set; } = true;
    public static bool contextualControls { get; set; } = true;

    public static float p1PanSpeed { get; set; } = 1f;
    public static float p2PanSpeed { get; set; } = 1f;

    public static float masterVolume
    {
        get
        {
            mixer.GetFloat("masterVolume", out float vol);
            return Mathf.Pow(10, vol/20);
        }
        set
        {
            mixer.SetFloat("masterVolume", Mathf.Log10(value) * 20);
        }
    }
    public static float musicVolume
    {
        get
        {
            mixer.GetFloat("musicVolume", out float vol);
            return Mathf.Pow(10, vol / 20);
        }
        set
        {
            mixer.SetFloat("musicVolume", Mathf.Log10(value) * 20);
        }
    }
    public static float sfxVolume
    {
        get
        {
            mixer.GetFloat("sfxVolume", out float vol);
            return Mathf.Pow(10, vol / 20);
        }
        set
        {
            mixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20);
        }
    }
    public static float announcerVolume
    {
        get
        {
            mixer.GetFloat("announcerWrapperVolume", out float vol);
            return Mathf.Pow(10, vol / 20);
        }
        set
        {
            mixer.SetFloat("announcerWrapperVolume", Mathf.Log10(value) * 20);
        }
    }




    [SerializeField] private AudioMixer _mixer;
    private static AudioMixer mixer;
    public Toggle fullscreenToggle;
    public Slider uiScaleSlider;
    public Toggle tooltipsToggle;
    public Toggle contextualControlsToggle;
    public Slider p1PanSpeedSlider;
    public Slider p2PanSpeedSlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider announcerVolumeSlider;

    private void Start()
    {
        mixer = _mixer;

        if (PlayerPrefs.HasKey("fullScreen"))
            fullScreen = PlayerPrefs.GetInt("fullScreen") == 1;
        fullscreenToggle.isOn = fullScreen;

        if (PlayerPrefs.HasKey("uiScale"))
            uiScale = PlayerPrefs.GetFloat("uiScale");
        uiScaleSlider.value = uiScale;

        if (PlayerPrefs.HasKey("tooltips"))
            tooltips = PlayerPrefs.GetInt("tooltips") == 1;
        tooltipsToggle.isOn = tooltips;

        if (PlayerPrefs.HasKey("contextualControls"))
            contextualControls = PlayerPrefs.GetInt("contextualControls") == 1;
        contextualControlsToggle.isOn = contextualControls;

        if (PlayerPrefs.HasKey("p1PanSpeed"))
            p1PanSpeed = PlayerPrefs.GetFloat("p1PanSpeed");
        p1PanSpeedSlider.value = p1PanSpeed;

        if (PlayerPrefs.HasKey("p2PanSpeed"))
            p2PanSpeed = PlayerPrefs.GetFloat("p2PanSpeed");
        p2PanSpeedSlider.value = p2PanSpeed;

        if (PlayerPrefs.HasKey("masterVolume"))
            masterVolume = PlayerPrefs.GetFloat("masterVolume");
        masterVolumeSlider.value = masterVolume;

        if (PlayerPrefs.HasKey("musicVolume"))
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        musicVolumeSlider.value = musicVolume;

        if (PlayerPrefs.HasKey("sfxVolume"))
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        sfxVolumeSlider.value = sfxVolume;

        if (PlayerPrefs.HasKey("announcerVolume"))
            announcerVolume = PlayerPrefs.GetFloat("announcerVolume");
        announcerVolumeSlider.value = announcerVolume;
    }

    private void Update()
    {
        PlayerPrefs.SetInt("fullScreen", fullScreen ? 1 : 0);
        PlayerPrefs.SetFloat("uiScale", uiScale);
        PlayerPrefs.SetInt("tooltips", tooltips ? 1 : 0);
        PlayerPrefs.SetInt("contextualControls", contextualControls ? 1 : 0);
        PlayerPrefs.SetFloat("p1PanSpeed", p1PanSpeed);
        PlayerPrefs.SetFloat("p2PanSpeed", p2PanSpeed);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("announcerVolume", announcerVolume);
        PlayerPrefs.Save();
    }
}
