using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class settingsUI : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle hardModeSlider;

    private void Start()
    {
        musicSlider.value = GlobalInfo.settings.musicVolume;
        sfxSlider.value = GlobalInfo.settings.sfxVolume;
        hardModeSlider.isOn = !GlobalInfo.settings.lockOrbit;
    }
    private void Update()
    {
        GlobalInfo.settings.musicVolume = musicSlider.value;
        GlobalInfo.settings.sfxVolume = sfxSlider.value;
        GlobalInfo.settings.lockOrbit = !hardModeSlider.isOn;
        GlobalInfo.settings.predictTrajectory = !hardModeSlider.isOn;
    }
}
