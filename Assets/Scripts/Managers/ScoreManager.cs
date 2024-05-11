using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private Player player;

    [SerializeField] private int highesetScore;
    [SerializeField] private int totalScore;

    public UnityEvent<int> OnTotalScoreChanged = new UnityEvent<int>();
    public UnityEvent<int> OnHighestScoreChanged = new UnityEvent<int>();

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

    public void IncreaseScore()
    {
        totalScore++;
        OnTotalScoreChanged.Invoke(totalScore);
    }

    public void RegisterHighestScore()
    {
        if (totalScore > highesetScore)
        {
            highesetScore = totalScore;
            OnHighestScoreChanged.Invoke(highesetScore);
            PlayerPrefs.SetInt("HSCORE", highesetScore);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        highesetScore = PlayerPrefs.GetInt("HSCORE");
        OnHighestScoreChanged.Invoke(highesetScore);

        player = FindObjectOfType<Player>();
        player.OnPlayerDied.AddListener(RegisterHighestScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
