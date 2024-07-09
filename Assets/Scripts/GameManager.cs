using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CameraController cameraController;
    public List<SkittleController> skittles;
    public ScoreManager scoreManager;
    public UIManager uiManager;

    public int currentPlayerIndex { get; private set; } = 0;
    public int roundNumber { get; private set; } = 1;
    private const int MAX_ROUNDS = 6;
    private const int NUMBER_OF_PLAYERS = 2;

    private List<SkittleController> knockedDownSkittles = new List<SkittleController>();

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
            return;
        }
        InitializeComponents();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeComponents()
    {
        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();
        if (scoreManager == null)
            scoreManager = GetComponent<ScoreManager>();
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
        if (skittles == null || skittles.Count == 0)
            skittles = new List<SkittleController>(FindObjectsOfType<SkittleController>());
    }

    private void InitializeGame()
    {
        SetupSkittles();
        ResetSkittles();
        scoreManager?.ResetScores();
        uiManager?.UpdateUI();
    }

    private void SetupSkittles()
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-0.5f, 1f, 10f),
            new Vector3(0.5f, 1f, 10f),
            new Vector3(-1f, 1f, 10.9f),
            new Vector3(1f, 1f, 10.9f),
            new Vector3(-1.5f, 1f, 11.75f),
            new Vector3(1.5f, 1f, 11.75f),
            new Vector3(-1f, 1f, 12.6f),
            new Vector3(1f, 1f, 12.6f),
            new Vector3(0f, 1f, 12.6f),
            new Vector3(0f, 1f, 10.9f),
            new Vector3(-0.5f, 1f, 11.75f),
            new Vector3(0.5f, 1f, 11.75f)
        };

        for (int i = 0; i < skittles.Count; i++)
        {
            if (i < positions.Length)
            {
                skittles[i].SetInitialPosition(positions[i]);
            }
        }
    }

    public void ResetSkittles()
    {
        foreach (var skittle in skittles)
        {
            skittle?.ResetPosition();
        }
        knockedDownSkittles.Clear();
    }

    public void NextTurn()
    {
        UpdateScore();
        currentPlayerIndex = (currentPlayerIndex + 1) % NUMBER_OF_PLAYERS;
        if (currentPlayerIndex == 0)
        {
            roundNumber++;
        }
        if (roundNumber > MAX_ROUNDS)
        {
            EndGame();
        }
        else
        {
            PrepareNextTurn();
        }
    }

    private void PrepareNextTurn()
    {
        cameraController?.ResetAim();
        ResetSkittles();
        uiManager?.UpdateUI();
    }

    private void EndGame()
    {
        uiManager?.ShowGameOverScreen();
    }

    public void RegisterKnockedDownSkittle(SkittleController skittle)
    {
        if (!knockedDownSkittles.Contains(skittle))
        {
            knockedDownSkittles.Add(skittle);
        }
    }

    private void UpdateScore()
    {
        int score = CalculateScore(knockedDownSkittles);
        scoreManager.AddScore(currentPlayerIndex, score);
        uiManager.UpdateUI();
        knockedDownSkittles.Clear();
    }

    private int CalculateScore(List<SkittleController> knockedSkittles)
    {
        if (knockedSkittles.Count == 0) return 0;
        if (knockedSkittles.Count == 1) return knockedSkittles[0].pointValue;
        return knockedSkittles.Count;
    }
}