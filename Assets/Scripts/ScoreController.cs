using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    public Text scoreKiri;
    public Text scoreKanan;

    public void UpdateScoreKiri(string newScore) => this.scoreKiri.text = newScore;

    public void UpdateScoreKanan(string newScore) => this.scoreKanan.text = newScore;
}
