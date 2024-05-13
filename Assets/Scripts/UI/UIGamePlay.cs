using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    public static UIGamePlay Instance;

    private Player player;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    [SerializeField] private TextMeshProUGUI nukeText;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Image timerImage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Reset()
    {
        UpdateHealthText();
        UpdateNukeText();
        HideTimerImage();
        HideGameOver();
        UpdateScoreText(0);
        UpdateHighestScoreText(PlayerPrefs.GetInt("HSCORE"));
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);

        player = FindObjectOfType<Player>();
        player.OnPlayerDied.AddListener(ShowGameOver);
        player.OnHealthChanged.AddListener(UpdateHealthText);
        player.OnNukesChanged.AddListener(UpdateNukeText);
        player.OnHighSpeedShootingEnabled.AddListener(ShowTimerImage);
        player.OnHighSpeedShootingDisabled.AddListener(HideTimerImage);
        player.OnPlayerDied.AddListener(HideTimerImage);

        player.gameObject.GetComponent<PlayerInput>().OnContinuousShooting.AddListener(UpdateShootingTimer);

        ScoreManager.Instance.OnTotalScoreChanged.AddListener(UpdateScoreText);
        ScoreManager.Instance.OnHighestScoreChanged.AddListener(UpdateHighestScoreText);

        UpdateScoreText(0);
        UpdateHealthText();
        HideTimerImage();
        UpdateHighestScoreText(PlayerPrefs.GetInt("HSCORE"));
    }

    // Update is called once per frame
    void Update()
    {
        if (timerImage.fillAmount > 0)
        {
            timerImage.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
        }
    }

    private void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    private void HideGameOver()
    {
        gameOverScreen.SetActive(false);
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health point: " + player.GetCurrentHealth().ToString();
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = "Current score: " + score.ToString();
    }

    private void UpdateHighestScoreText(int score)
    {
        highestScoreText.text = "Highest score: " + score.ToString();
    }

    private void UpdateNukeText()
    {
        nukeText.text = "# of nuke: " + player.GetNumOfNukes().ToString();
    }

    public void ReturnToMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TryAgain()
    {
        GameManager.Instance.Reset();
        ScoreManager.Instance.Reset();
        Reset();
    }

    private void UpdateShootingTimer(float t)
    {
        timerImage.fillAmount = t;
        timerImage.transform.position = Camera.main.WorldToScreenPoint(player.transform.position);
    }

    private void ShowTimerImage()
    {
        timerImage.fillAmount = 1;
    }

    private void HideTimerImage()
    {
        timerImage.fillAmount = 0;
    }
}
