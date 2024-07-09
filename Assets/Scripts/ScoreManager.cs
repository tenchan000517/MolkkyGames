using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[] playerScores;
    private const int NUMBER_OF_PLAYERS = 2;

    private void Awake()
    {
        InitializeScores();
    }

    private void InitializeScores()
    {
        playerScores = new int[NUMBER_OF_PLAYERS];
    }

    public void AddScore(int playerIndex, int points)
    {
        if (playerIndex >= 0 && playerIndex < NUMBER_OF_PLAYERS)
        {
            playerScores[playerIndex] += points;
        }
    }

    public int GetPlayerScore(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < NUMBER_OF_PLAYERS)
        {
            return playerScores[playerIndex];
        }
        return 0;
    }

    public void ResetScores()
    {
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            playerScores[i] = 0;
        }
    }

    public int GetWinner()
    {
        int maxScore = int.MinValue;
        int winnerIndex = -1;
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            if (playerScores[i] > maxScore)
            {
                maxScore = playerScores[i];
                winnerIndex = i;
            }
        }
        return winnerIndex;
    }
}