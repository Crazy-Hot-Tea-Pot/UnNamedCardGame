using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SettingsUIController : UiController
{
    //First Layer Settings Buttons
    private Button Optionsbtn;
    private Button Continuebtn;
    private Button Exitbtn;
    private Button MainMenubtn;

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
    private Slider GammaSlider;
    private Slider GainSlider;
    private Toggle bloomOn;
    private TMP_Dropdown resolutionDropDown;
    private TMP_Dropdown aspectDropDown;
    private Toggle windowedOn;

    //Apply&Discard buttons
    private Button Applybtn;
    private Button Discardbtn;
    private Button RestoreDefaultsbtn;

    //A bool for title screen
    public bool miniSkip = false;
    private GameObject titleScreenUI;

    // Start is called before the first frame update
    void Start()
    {
        //Menu Containers
        largeSettingMenu = this.gameObject.transform.Find("OptionsScreen").gameObject;
        smallSettingMenu = this.gameObject.transform.Find("ShortMenu").gameObject;


        //Buttons for tabs
        VideoTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("VideoSettingsbtn").GetComponent<Button>();
        VideoTabbtn.onClick.RemoveAllListeners();
        VideoTabbtn.onClick.AddListener(OpenVideoSettingsTab);
        AudioTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("Audiobtn").GetComponent<Button>();
        AudioTabbtn.onClick.RemoveAllListeners();
        AudioTabbtn.onClick.AddListener(OpenAudioSettingsTab);
        Applybtn = this.gameObject.transform.Find("OptionsScreen").Find("Applybtn").GetComponent<Button>();
        Applybtn.onClick.RemoveAllListeners();
        Applybtn.onClick.AddListener(ApplySettings);
        Discardbtn = this.gameObject.transform.Find("OptionsScreen").Find("Discardbtn").GetComponent<Button>();
        Discardbtn.onClick.RemoveAllListeners();
        Discardbtn.onClick.AddListener(DiscardSettings);
        RestoreDefaultsbtn = this.gameObject.transform.Find("OptionsScreen").Find("Defaultsbtn").GetComponent<Button>();
        RestoreDefaultsbtn.onClick.RemoveAllListeners();
        RestoreDefaultsbtn.onClick.AddListener(RestoreDefaults);

        //If title screen skip mini menu
        if (miniSkip)
        {
            SkipMiniMenu();
        }
        else
        {
            //Find buttons and add listeners
            Optionsbtn = this.gameObject.transform.Find("ShortMenu").Find("Optionsbtn").GetComponent<Button>();
            Optionsbtn.onClick.RemoveAllListeners();
            Optionsbtn.onClick.AddListener(Options);
            Continuebtn = this.gameObject.transform.Find("ShortMenu").Find("Continuebtn").GetComponent<Button>();
            Continuebtn.onClick.RemoveAllListeners();
            Continuebtn.onClick.AddListener(Continue);
            Exitbtn = this.gameObject.transform.Find("ShortMenu").Find("Exitbtn").GetComponent<Button>();
            Exitbtn.onClick.RemoveAllListeners();
            Exitbtn.onClick.AddListener(Exit);
            MainMenubtn = this.gameObject.transform.Find("ShortMenu").Find("MainMenubtn").GetComponent<Button>();
            MainMenubtn.onClick.AddListener(MainMenu);
            MainMenubtn.onClick.AddListener(MainMenu);
        }
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
        //If the larger UI is open then close options
        if(largeSettingMenu.activeSelf)
        {
            largeSettingMenu.SetActive(false);
            //If we aren't skipping main menu open it again
            if (miniSkip == false)
            {
                smallSettingMenu.SetActive(true);
            }
        }
        //If the smaller UI then open larger options
        else if (smallSettingMenu.activeSelf)
        {
            largeSettingMenu.SetActive(true);
            smallSettingMenu.SetActive(false);


            //Buttons for tabs
            VideoTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("VideoSettingsbtn").GetComponent<Button>();
            VideoTabbtn.onClick.RemoveAllListeners();
            VideoTabbtn.onClick.AddListener(OpenVideoSettingsTab);
            AudioTabbtn = this.gameObject.transform.Find("OptionsScreen").Find("Audiobtn").GetComponent<Button>();
            AudioTabbtn.onClick.RemoveAllListeners();
            AudioTabbtn.onClick.AddListener(OpenAudioSettingsTab);
            Applybtn = this.gameObject.transform.Find("OptionsScreen").Find("Applybtn").GetComponent<Button>();
            Applybtn.onClick.RemoveAllListeners();
            Applybtn.onClick.AddListener(ApplySettings);
            Discardbtn = this.gameObject.transform.Find("OptionsScreen").Find("Discardbtn").GetComponent<Button>();
            Discardbtn.onClick.RemoveAllListeners();
            Discardbtn.onClick.AddListener(DiscardSettings);
            RestoreDefaultsbtn = this.gameObject.transform.Find("OptionsScreen").Find("Defaultsbtn").GetComponent<Button>();
            RestoreDefaultsbtn.onClick.RemoveAllListeners();
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
        #region VolumProfileAdjustments
            GammaSlider = videoSettingTab.transform.Find("GammaSlider").GetComponent<Slider>();
            GammaSlider.enabled = true;
            bloomOn = videoSettingTab.transform.Find("ToggleBloom").GetComponent<Toggle>();
            bloomOn.enabled = true;
            GainSlider = videoSettingTab.transform.Find("GainSlider").GetComponent<Slider>();
            GainSlider.enabled = true;

            //Set values
            //Try to get the variable for gain
            if (VolumeSettings[0].TryGet(out LiftGammaGain gainSettings))
            {
                //Set gamma to meet this value. W represents the value of the intensity and we add +0.5f so it's usable as this value uses negative values but sliders don't.
                GammaSlider.value = gainSettings.gamma.value.w + 0.5f;
                //Repeat the same process for gain
                GainSlider.value = gainSettings.gain.value.w + 0.5f;
            }
            //If this value doesn't exist
            else
            {
                Debug.Log("There is no gain");
                GammaSlider.enabled = false;
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
        #endregion
        #region UnitySettingsChange
        aspectDropDown = videoSettingTab.transform.Find("AspectDropDown").GetComponent<TMP_Dropdown>();
        ////aspectDropDown.enabled = true;
        resolutionDropDown = videoSettingTab.transform.Find("ResolutionDropDown").GetComponent<TMP_Dropdown>();
        //resolutionDropDown.enabled = true;
        windowedOn = videoSettingTab.transform.Find("ToggleWindowed").GetComponent<Toggle>();
        windowedOn.enabled = true;

        //Screen resolution
        if (UnityEngine.Screen.currentResolution.width == 1920 && UnityEngine.Screen.currentResolution.height == 1080)
        {
            resolutionDropDown.value = 0;
            Debug.Log("1920x1080");
        }
        else if (UnityEngine.Screen.currentResolution.width == 1366 && UnityEngine.Screen.currentResolution.height == 763)
        {
            resolutionDropDown.value = 1;
            Debug.Log("1366x763");
        }
        else if (UnityEngine.Screen.currentResolution.width == 2560 && UnityEngine.Screen.currentResolution.height == 1440)
        {
            resolutionDropDown.value = 2;
            Debug.Log("2560x1440");
        }
        else if (UnityEngine.Screen.currentResolution.width == 3840 && UnityEngine.Screen.currentResolution.height == 2160)
        {
            resolutionDropDown.value = 3;
            Debug.Log("3840x2160");
        }
        else
        {
            resolutionDropDown.value = 0;
            UnityEngine.Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            Debug.Log("We don't know");
        }

        //Check if windowed and assign the toggle to match
        if (UnityEngine.Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow))
        {
            windowedOn.isOn = false;
            Debug.Log("Full screen");
        }
        else
        {
            windowedOn.isOn = true;
            Debug.Log("Windowed");
        }

        #endregion
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
                    gainSettings.gamma.value += new Vector4(0, 0, 0, GammaSlider.value - 0.5f);
                    //Repeat the same process for gain
                    gainSettings.gain.value += new Vector4(0, 0, 0, GainSlider.value - 0.5f);
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

        //Apply Unity Settings
        //Check if windowed and assign the toggle to match
        if(windowedOn.isOn == false)
        {
            //Sets full screen
            UnityEngine.Screen.fullScreen = true;
            Debug.Log("Full screen");
        }
        else
        {
            //Sets windowed
            UnityEngine.Screen.fullScreen = false;
            Debug.Log("Windowed");
        }


        //Screen resolution
        if (resolutionDropDown.value == 0)
        {
            UnityEngine.Screen.SetResolution(1920, 1080, !windowedOn.isOn);
            Debug.Log("1920x1080");
        }
        else if (resolutionDropDown.value == 1)
        {
            UnityEngine.Screen.SetResolution(1366, 763, !windowedOn.isOn);
            Debug.Log("1366x763");
        }
        else if (resolutionDropDown.value == 2)
        {

            UnityEngine.Screen.SetResolution(2560, 1440, !windowedOn.isOn);
            Debug.Log("2560x1440");
        }
        else if (resolutionDropDown.value == 3)
        {

            UnityEngine.Screen.SetResolution(3840, 2160, !windowedOn.isOn);
            Debug.Log("3840x2160");
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
                    //Get the defaults
                    if(VolumeDefaults[i].TryGet(out LiftGammaGain gainDefaults))
                    {
                        //Set gain equal to the defult profile in the same column of the list
                        gainSettings.gamma.value = gainDefaults.gamma.value;
                        //Repeat for gain
                        gainSettings.gain.value = gainDefaults.gain.value;
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

    /// <summary>
    /// Return to the first menu
    /// </summary>
    public void ReturnToMiniMenu()
    {
        //Skip this menu if applicable
        if(miniSkip)
        {
            miniSkip = false;
            Debug.Log(miniSkip);
            titleScreenUI.SetActive(true);
            largeSettingMenu.SetActive(false);
            UiManager.Instance.CloseSettingsOnClickTitle();
        }
        else
        {
            largeSettingMenu.SetActive(false);
            smallSettingMenu.SetActive(true);
        }
    }

    public void SkipMiniMenu()
    {
        //For some reason it is running skip menu way too much
        try
        {
            titleScreenUI = GameObject.Find("Canvas");
            titleScreenUI.SetActive(false);
        }
        catch
        {
            Debug.Log("Not Title");
        }
        largeSettingMenu.SetActive(true);
        smallSettingMenu.SetActive(false);
    }

    /// <summary>
    /// This might need to be removed
    /// </summary>
    public void MainMenu()
    {
        smallSettingMenu.SetActive(false);
        Destroy(UiManager.Instance.CurrentUI);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
