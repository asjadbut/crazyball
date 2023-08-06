using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text _scoreText, _endScoreText,_highScoreText;

    private int score;

    public static float timeToMove;
    [SerializeField]
    private Animator _scoreAnimator;

    [SerializeField]
    private AnimationClip _scoreClip;

    [SerializeField]
    private GameObject _endPanel;

    [SerializeField]
    private Image _soundImage;

    [SerializeField]
    private Sprite _activeSoundSprite, _inactiveSoundSprite;

    [SerializeField]
    private GameObject _scorePrefab, _obstaclePrefab;

    public static event Action GameStarted, GameEnded, onGameoverLoadInterstitialAd;

    [SerializeField]
    GameObject scores, buttonsPanel;

    [SerializeField]
    GameObject celebrationEffect;

    //public float fps;
    //public TMP_Text fpsText;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.AddButtonSound();
        score = 0;
        timeToMove = 2;
        _scoreText.text = score.ToString();
        _scoreAnimator.Play(_scoreClip.name, -1, 0f);
        GameStarted?.Invoke();
        InvokeRepeating(nameof(SpawnObstacle), 0f, 1f);
        //InvokeRepeating(nameof(GetFPS), 1f, 1f);

    }

    private void SpawnObstacle()
    {
        Instantiate(UnityEngine.Random.Range(0f,1f) > 0.3f ? _obstaclePrefab : _scorePrefab
            , new Vector3(0,10,0), Quaternion.identity);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Constants.DATA.MAIN_MENU_SCENE);
    }

    public void ReloadGame()
    {
        int playCount = PlayerPrefs.GetInt("playCount");
        ++playCount;
        PlayerPrefs.SetInt("playCount", playCount);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Rate()
    {
        Debug.Log("go to the game rating page");
        //Application.OpenURL("http://unity3d.com/");
    }

    public void ToggleSound()
    {
        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ? PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND): 1) == 1;
        sound = !sound;
        PlayerPrefs.SetInt(Constants.DATA.SETTINGS_SOUND, sound ? 1 : 0);
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;
        AudioManager.Instance.ToggleSound();
    }

    public void EndGame()
    {
        StartCoroutine(IEndGame());
    }

    private IEnumerator IEndGame()
    {
        GameEnded?.Invoke();
        CancelInvoke(nameof(SpawnObstacle));
        yield return new WaitForSeconds(1.2f);
        _endPanel.SetActive(true);
        if (MainMenuManager.isInterstitialAdEnable)
        {
            int playCount = PlayerPrefs.GetInt("playCount");
            if (playCount != 0 && playCount % 5 == 0)
                onGameoverLoadInterstitialAd?.Invoke();
        }
        _endScoreText.text = score.ToString();

        int highScore = PlayerPrefs.HasKey(Constants.DATA.HIGH_SCORE) ? PlayerPrefs.GetInt(Constants.DATA.HIGH_SCORE) : 0;
        if (score > highScore)
        {
            _highScoreText.text = "NEW BEST";
            highScore = score;
            celebrationEffect.SetActive(true);
            AudioManager.Instance.PlayCelebrationSound();
            LeanTween.scale(scores, new Vector3(1f, 1f, 1f), 2f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.moveLocal(buttonsPanel, Vector3.zero, 2.3f).setDelay(2f).setEase(LeanTweenType.easeOutCirc);
            PlayerPrefs.SetInt(Constants.DATA.HIGH_SCORE, highScore);
        }
        else
        {
            _highScoreText.text = "BEST " + highScore.ToString();
            LeanTween.scale(scores, new Vector3(1f, 1f, 1f), 2f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);
            LeanTween.moveLocal(buttonsPanel, Vector3.zero, 2.3f).setDelay(0.8f).setEase(LeanTweenType.easeOutCirc);
        }

        bool sound = (PlayerPrefs.HasKey(Constants.DATA.SETTINGS_SOUND) ?
          PlayerPrefs.GetInt(Constants.DATA.SETTINGS_SOUND) : 1) == 1;
        _soundImage.sprite = sound ? _activeSoundSprite : _inactiveSoundSprite;

    }

    public void UpdateScore()
    {
        score++;
        _scoreText.text = score.ToString();
        _scoreAnimator.Play(_scoreClip.name, -1, 0f);
        if (score != 0 && score % 10 == 0)
        {
            if(timeToMove > 1.3f)
                timeToMove -= 0.1f;
        }
    }
    //private void GetFPS()
    //{
    //    fps = (int)(1f / Time.unscaledDeltaTime);
    //    fpsText.text = fps + " fps";
    //}

}
