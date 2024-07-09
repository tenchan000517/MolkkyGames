using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text[] playerScoreTexts;
    public Text roundText;
    public Text currentPlayerText;
    public GameObject gameOverPanel;
    public Text winnerText;
    public Button restartButton;

    public Slider powerGaugeSlider;
    public GameObject powerGaugeObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.Log("Restart functionality not yet implemented.");
        }
        
        HidePowerGauge();
    }

    public void UpdateUI()
    {
        if (GameManager.Instance == null || GameManager.Instance.scoreManager == null)
        {
            Debug.LogError("GameManager or ScoreManager is not initialized");
            return;
        }

        for (int i = 0; i < playerScoreTexts.Length; i++)
        {
            playerScoreTexts[i].text = "Player " + (i + 1) + ": " + GameManager.Instance.scoreManager.GetPlayerScore(i);
        }

        if (roundText != null)
            roundText.text = "Round: " + GameManager.Instance.roundNumber.ToString();
        if (currentPlayerText != null)
            currentPlayerText.text = "Player " + (GameManager.Instance.currentPlayerIndex + 1).ToString() + "'s Turn";
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        if (GameManager.Instance != null && GameManager.Instance.scoreManager != null)
        {
            int winner = GameManager.Instance.scoreManager.GetWinner();
            if (winnerText != null)
                winnerText.text = "Player " + (winner + 1).ToString() + " Wins!";
        }
        else
        {
            Debug.LogError("GameManager or ScoreManager is not initialized");
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPowerGauge()
    {
        if (powerGaugeObject != null)
            powerGaugeObject.SetActive(true);
    }

    public void HidePowerGauge()
    {
        if (powerGaugeObject != null)
            powerGaugeObject.SetActive(false);
    }

    public void UpdatePowerGauge(float value)
    {
        if (powerGaugeSlider != null)
            powerGaugeSlider.value = value;
    }

    public void UpdatePlayerScore(int playerIndex, int score)
    {
        if (playerIndex >= 0 && playerIndex < playerScoreTexts.Length)
        {
            playerScoreTexts[playerIndex].text = "Player " + (playerIndex + 1) + ": " + score;
        }
    }

    public void SetActivePlayer(int playerIndex)
    {
        if (currentPlayerText != null)
            currentPlayerText.text = "Player " + (playerIndex + 1) + "'s Turn";
    }
}