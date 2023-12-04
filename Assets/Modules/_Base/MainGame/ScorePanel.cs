using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class ScorePanel : MonoBehaviour
    {
        public Text scoreKiri;
        public Text scoreKanan;

        public void UpdateScoreKiri(string newScore) => scoreKiri.text = newScore;
        public void UpdateScoreKanan(string newScore) => scoreKanan.text = newScore;
    }
}