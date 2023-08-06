using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //public static MainMenuManager Instance { get; private set; }

    [SerializeField]
    private Image _soundImage;

    [SerializeField]
    private Sprite _activeSoundSprite, _inactiveSoundSprite;

    [SerializeField]
    private GameObject _controlsInfoPanel;
    [SerializeField]
    private GameObject _mainMenuPanel;

    public static bool isSessionActive = false;
    public static bool isBannerAdEnable;
    public static string bannerAd_Id;

    public static bool isInterstitialAdEnable;
    public static string interstitialAd_Id;

    private void Start()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ?
           PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND) : 1) == 1;
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;

        AudioManager.Instance.AddButtonSound();

        if (!PlayerPrefs.HasKey("playCount"))
        {
            PlayerPrefs.SetInt("playCount", 0);
        }

        SetApplicationFPS();
        SetAccelerometerFrequency();
    }

    public void ClickedPlay()
    {
        int playCount = PlayerPrefs.GetInt("playCount");
        ++playCount;
        PlayerPrefs.SetInt("playCount", playCount);
        SceneManager.LoadScene(Constants.DATA.GAMEPLAY_SCENE);
    }

    public void ClickedQuit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void ToggleSound()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ? PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND)
             : 1) == 1;
        sound = !sound;
        PlayerPrefs.SetInt(Constants.DATA.SETTINGS_SOUND, sound ? 1 : 0);
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;
        AudioManager.Instance.ToggleSound();
    }
    public void ShowControlsInfo()
    {
        _controlsInfoPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
    }
    public void HideControlsInfo()
    {
        _mainMenuPanel.SetActive(true);
        _controlsInfoPanel.SetActive(false);
    }
    private void SetApplicationFPS()
    {
        QualitySettings.vSyncCount = 0;
        double deviceRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        if (deviceRefreshRate >= 60)
            Application.targetFrameRate = 45;
        if (deviceRefreshRate < 60 && deviceRefreshRate >= 45)
            Application.targetFrameRate = 30;
        if (deviceRefreshRate < 45 && deviceRefreshRate >= 30)
            Application.targetFrameRate = 30;
    }
    private void SetAccelerometerFrequency()
    {
        PlayerSettings.accelerometerFrequency = 1;
    }
}
