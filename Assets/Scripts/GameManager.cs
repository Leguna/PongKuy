using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GotoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
