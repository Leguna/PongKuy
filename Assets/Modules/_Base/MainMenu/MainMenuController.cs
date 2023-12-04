using MainGame;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MyButton playButton;
        [SerializeField] private MyButton multiplayerButton;
        [SerializeField] private MyButton creditsButton;
        [SerializeField] private MyButton creditsBackButton;
        [SerializeField] private MyButton quitButton;
        [SerializeField] private MyButton linkedInButton;
        [SerializeField] private MyButton githubButton;

        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject mainMenuPanel;

        [SerializeField] private GameManager gameManager;
        private AudioService audioService;

        private void Start()
        {
            AudioService.Instance.Init();
            playButton.onClick.AddListener(PlayGame);
            creditsButton.onClick.AddListener(CreditsButtonListener);
            quitButton.onClick.AddListener(QuitGameListener);
            multiplayerButton.onClick.AddListener(MultiplayerButtonListener);
            creditsBackButton.onClick.AddListener(Home);
            linkedInButton.onClick.AddListener(() => OpenLink("https://www.linkedin.com/in/tuflihun/"));
            githubButton.onClick.AddListener(() => OpenLink("https://github.com/leguna"));
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