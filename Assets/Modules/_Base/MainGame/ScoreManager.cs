using System;
using Mirror;
using MirrorMultiplayerPong;

namespace MainGame
{
    public class ScoreManager : NetworkBehaviour
    {
        [SyncVar(hook = nameof(RpcSyncScore))] public ScoreBoard scoreBoard = new();

        public int maxScore;
        public ScorePanel scorePanel;

        private Action<ScoreBoard> onGameOver;

        public void Init(Action<ScoreBoard> onGameOver)
        {
            scorePanel.gameObject.SetActive(true);
            this.onGameOver = onGameOver;
            ResetScore();
        }

        private void OnScoreBoardMessage(ScoreBoardMessage obj)
        {
            scoreBoard = obj.scoreBoard;
            UpdateScore();
        }

        public void AddScore(PlayerType playerType)
        {
            if (!isServer && MyNetworkManager.Sin.isMultiplayer) return;
            if (playerType == PlayerType.Left) scoreBoard.leftScore.score++;
            else scoreBoard.rightScore.score++;
            if (isServer)
                NetworkServer.SendToAll(new ScoreBoardMessage(scoreBoard));
            UpdateScore();
            CheckScore();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<ScoreBoardMessage>(OnScoreBoardMessage);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            NetworkClient.UnregisterHandler<ScoreBoardMessage>();
        }

        public void ResetScore(PlayerType playerType)
        {
            if (playerType == PlayerType.Left) scoreBoard.leftScore.score = 0;
            else scoreBoard.rightScore.score = 0;
            UpdateScore();
        }

        public void ResetScore()
        {
            scoreBoard = new ScoreBoard();
            UpdateScore();
        }

        public void RpcSyncScore(ScoreBoard oldScoreboard, ScoreBoard newScoreboard)
        {
            scoreBoard = newScoreboard;
            UpdateScore();
        }

        public void UpdateScore()
        {
            scorePanel.UpdateScoreKiri(scoreBoard.leftScore.score.ToString());
            scorePanel.UpdateScoreKanan(scoreBoard.rightScore.score.ToString());
        }

        private void CheckScore()
        {
            if (scoreBoard.leftScore.score < maxScore && scoreBoard.rightScore.score < maxScore) return;
            onGameOver?.Invoke(scoreBoard);
        }

        public void HideScore()
        {
            scorePanel.gameObject.SetActive(false);
        }

        public void ShowScore()
        {
            scorePanel.gameObject.SetActive(true);
            UpdateScore();
        }
    }

    public struct ScoreBoardMessage : NetworkMessage
    {
        public readonly ScoreBoard scoreBoard;

        public ScoreBoardMessage(ScoreBoard scoreBoard)
        {
            this.scoreBoard = scoreBoard;
        }
    }

    public struct Score
    {
        [SyncVar] public PlayerType playerType;
        [SyncVar] public int score;

        public Score(PlayerType left, int i)
        {
            playerType = left;
            score = i;
        }
    }

    public class ScoreBoard
    {
        [SyncVar] public Score leftScore = new(PlayerType.Left, 0);
        [SyncVar] public Score rightScore = new(PlayerType.Right, 0);
    }
}