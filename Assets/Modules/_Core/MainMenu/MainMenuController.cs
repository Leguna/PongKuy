using MainGame;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button multiplayerButton;

        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject mainMenuPanel;

        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            playButton.onClick.AddListener(PlayGame);
            creditsButton.onClick.AddListener(CreditsButtonListener);
            quitButton.onClick.AddListener(QuitGameListener);
            multiplayerButton.onClick.AddListener(MultiplayerButtonListener);
            Home();
        }

        private void MultiplayerButtonListener()
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            gameManager.Init(true, Home);
        }

        private void Home()
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        public void PlayGame()
        {
            mainMenuPanel.SetActive(false);
            creditsPanel.SetActive(false);
            gameManager.Init(false, Home);
        }

        private void CreditsButtonListener()
        {
            creditsPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }

        public void QuitGameListener() => Application.Quit();
        public void OpenLink(string url) => Application.OpenURL(url);
    }
}