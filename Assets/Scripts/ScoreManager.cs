using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public int leftScore;
    public int rightScore;

    public int maxScore;
    public ScoreController scoreController;
    public UnityEvent resetBallPositionCallback;

    public void AddLeftScore(int increment)
    {
        leftScore += increment;
        scoreController.UpdateScoreKiri(leftScore.ToString());
        CheckScore();
        resetBallPositionCallback.Invoke();
    }

    public void AddRightScore(int increment)
    {
        rightScore += increment;
        scoreController.UpdateScoreKanan(rightScore.ToString());
        CheckScore();
        resetBallPositionCallback.Invoke();
    }

    public void CheckScore()
    {
        if (leftScore >= maxScore)
        {
            GameOver(1);
        }
        else if (rightScore >= maxScore)
        {
            GameOver(2);
        }
    }

    private void GameOver(int player)
    {
        SceneManager.LoadScene(0);
    }
}
