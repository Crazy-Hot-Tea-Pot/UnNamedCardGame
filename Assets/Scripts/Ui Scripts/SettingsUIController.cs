using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsUIController : UiController
{
    //First Layer Settings Buttons
    private Button Optionsbtn;
    private Button Continuebtn;

    //Buttons for tabs
    private Button VideoTabbtn;
    private Button AudioTabbtn;

    //UI Containers
    private GameObject largeSettingMenu;
    private GameObject smallSettingMenu;
    private GameObject videoSettingTab;
    private GameObject audioSettingTab;

    //Global Volume Component profile list
    public List<VolumeProfile> VolumeSettings;

    //Global Volume Component Defaults list
    public List<VolumeProfile> VolumeDefaults;

    //Buttons for video settings
    private Slider Brightneslide;
    private Toggle bloomOn;

    //Apply&Discard buttons
    private Button Applybtn;
    private Button Discardbtn;
    private Button RestoreDefaultsbtn;

    //A bool for title screen
    private bool miniSkip;

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
        {
            GameObject.Find("Canvas").transform.Find("ButtonPanel").transform.Find("OptionsButton").GetComponent<Button>().onClick.AddListener(SkipMiniMenu);
        }
        miniSkip = false;
        //Menu Containers
        largeSettingMenu = this.gameObject.transform.Find("OptionsScreen").gameObject;
        smallSettingMenu = this.gameObject.transform.Find("ShortMenu").gameObject;
        //Find buttons and add listeners
        Optionsbtn = this.gameObject.transform.Find("ShortMenu").Find("Optionsbtn").GetComponent<Button>();
        Optionsbtn.onClick.AddListener(Options);
        Continuebtn = this.gameObject.transform.Find("ShortMenu").Find("Continuebtn").GetComponent<Button>();
        Continuebtn.onClick.AddListener(Continue);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Open and close the options menu
    /// </summary>
    public void Options()
    {
        //If the larger UI then close options
        if(largeSettingMenu.activeSelf)
        {
            largeSettingMenu.SetActive(false);
            smallSettingMenu.SetActive(true);
        }
        //If the smaller UI then open larger options
        else if (smallSettingMenu.activeSelf)
        {
            largeSettingMenu.SetActive(true);
            smallSettingMenu.SetActive(false);


            //Buttons for tabs
            VideoTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("VideoSettingsbtn").GetComponent<Button>();
            VideoTabbtn.onClick.AddListener(OpenVideoSettingsTab);
            AudioTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("Audiobtn").GetComponent<Button>();
            AudioTabbtn.onClick.AddListener(OpenAudioSettingsTab);
            Applybtn = this.gameObject.transform.Find("OptionsScreen").Find("Applybtn").GetComponent<Button>();
            Applybtn.onClick.AddListener(ApplySettings);
            Discardbtn = this.gameObject.transform.Find("OptionsScreen").Find("Discardbtn").GetComponent<Button>();
            Discardbtn.onClick.AddListener(DiscardSettings);
            RestoreDefaultsbtn = this.gameObject.transform.Find("OptionsScreen").Find("Defaultsbtn").GetComponent<Button>();
            RestoreDefaultsbtn.onClick.AddListener(RestoreDefaults);
        }
    }

    /// <summary>
    /// Close this UI
    /// </summary>
    public void Continue()
    {
        UiManager.Instance.CloseSettingsOnClick();
    }

    /// <summary>
    /// Settings for opening and closing video settings tab
    /// </summary>
    public void OpenVideoSettingsTab()
    {
        //Find the settings tab
        videoSettingTab = VideoTabbtn.transform.parent.Find("VideoSettingsTab").gameObject;
        AudioTabbtn.transform.parent.Find("AudioSettingsTab").gameObject.SetActive(false);
        //Open/Close tab
        if (videoSettingTab.activeSelf)
        {
            videoSettingTab.SetActive(false);
        }
        else
        {
            videoSettingTab.SetActive(true);
        }

        setVolumeValues();
    }

    ///<summary>
    ///Settings for opening and closing audio settings tab
    ///</summary>
    public void OpenAudioSettingsTab()
    {
        audioSettingTab = AudioTabbtn.transform.parent.Find("AudioSettingsTab").gameObject;
        VideoTabbtn.transform.parent.Find("VideoSettingsTab").gameObject.SetActive(false);
        if (audioSettingTab.activeSelf)
        {
            audioSettingTab.SetActive(false);
        }
        else
        {
            audioSettingTab.SetActive(true);
        }
    }

    /// <summary>
    ///Set all buttons values inside of video settings
    /// </summary>
    public void setVolumeValues()
    {
            Brightneslide = videoSettingTab.transform.Find("BrightnessSlider").GetComponent<Slider>();
            Brightneslide.enabled = true;
            bloomOn = videoSettingTab.transform.Find("ToggleBloom").GetComponent<Toggle>();
            bloomOn.enabled = true;

            //Set values
            //Try to get the variable for gain
            if (VolumeSettings[0].TryGet(out LiftGammaGain gainSettings))
            {
                //Set brightness to meet this value. W represents the value of the intensity and we add +0.5f so it's usable as this value uses negative values but sliders don't.
                Brightneslide.value = gainSettings.gamma.value.w + 0.5f;
            }
            //If this value doesn't exist
            else
            {
                Debug.Log("There is no gain");
                Brightneslide.enabled = false;
            }

            //Enable and disable bloom check
            if (VolumeSettings[0].TryGet(out Bloom bloomSettings))
            {
                bloomOn.isOn = bloomSettings.active;
            }
            //If there is no bloom
            else
            {
                Debug.Log("There is no bloom");
                bloomOn.enabled = false;
            }
    }

    //If we are applying settings
    public void ApplySettings()
    {
            foreach (VolumeProfile levelProfile in VolumeSettings)
            {
                //Try to get the variable for gain
                if (levelProfile.TryGet(out LiftGammaGain gainSettings))
                {
                    //Set brightness to meet the new value. W represents the value of the intensity and we add +0.5f so it's usable as this value uses negative values but sliders don't.
                    gainSettings.gamma.value += new Vector4(0, 0, 0, Brightneslide.value - 0.5f);
                }
                //If this value doesn't exist
                else
                {
                    Debug.Log("There is no gain");
                }

                //Enable and disable bloom check
                if (levelProfile.TryGet(out Bloom bloomSettings))
                {
                    bloomSettings.active = bloomOn.isOn;
                }
                //If there is no bloom
                else
                {
                    Debug.Log("There is no bloom");
                }
            }

            Debug.Log("Graphics settings applied");
            Options();
            ReturnToMiniMenu();
    }

    /// <summary>
    /// If we don't apply any of the settings
    /// </summary>
    public void DiscardSettings()
    {
        setVolumeValues();
        Options();
        ReturnToMiniMenu();
    }

    //Restore default values
    public void RestoreDefaults()
    {
        ApplySettings();
        if (VolumeSettings.Count == VolumeDefaults.Count)
        {
            //Run through the list of settings
            for (int i = 0; i < VolumeSettings.Count; i++)
            {
                //Try to get the variable for gain
                if (VolumeSettings[i].TryGet(out LiftGammaGain gainSettings))
                {
                    if(VolumeDefaults[i].TryGet(out LiftGammaGain gainDefaults))
                    {
                        //Set brightness to meet the new value. W represents the value of the intensity and we add +0.5f so it's usable as this value uses negative values but sliders don't.
                        gainSettings.gamma.value = gainDefaults.gamma.value;
                    }
                }
                //If this value doesn't exist
                else
                {
                    Debug.Log("There is no gain");
                }

                //Enable and disable bloom check
                if (VolumeSettings[i].TryGet(out Bloom bloomSettings))
                {
                    if(VolumeDefaults[i].TryGet(out Bloom bloomDefaults))
                    {
                        bloomSettings.active = bloomDefaults.IsActive();
                    }
                }
                //If there is no bloom
                else
                {
                    Debug.Log("There is no bloom");
                }
            }
        }
        else
        {
            Debug.Log("Developer error there must be as many volume profiles as defaults");
        }
        DiscardSettings();
    }

    public void ReturnToMiniMenu()
    {
        if(miniSkip)
        {
            largeSettingMenu.SetActive(false);
        }
        else
        {
            largeSettingMenu.SetActive(false);
            smallSettingMenu.SetActive(true);
        }
    }

    public void SkipMiniMenu()
    {
        miniSkip = true;
        largeSettingMenu.SetActive(true);
        smallSettingMenu.SetActive(false);
    }
}
