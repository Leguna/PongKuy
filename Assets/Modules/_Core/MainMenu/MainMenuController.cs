using LobbyMultiplayer;
using ToC;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        [SerializeField] private LobbyController multiplayerController;

        private void Start()
        {
            playButton.onClick.AddListener(PlayGame);
            creditsButton.onClick.AddListener(CreditsButtonListener);
            quitButton.onClick.AddListener(QuitGameListener);
            multiplayerButton.onClick.AddListener(MultiplayerButtonListener);
        }

        private void MultiplayerButtonListener()
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            multiplayerController.Init(HomeButtonListener);
        }

        private void HomeButtonListener()
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
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