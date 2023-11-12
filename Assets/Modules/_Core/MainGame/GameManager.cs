using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainGame
{
    public class GameManager : MonoBehaviour
    {
        public void GotoMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
