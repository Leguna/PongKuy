using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.ToastModal
{
    public class Toast : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;

        private Sequence tween;

        public void Show(string message, float showDuration = .5f, float fadeDuration = 0.2f)
        {
            tween.Kill();
            text.text = message;
            gameObject.SetActive(true);
            text.alpha = 0f;
            text.rectTransform.anchoredPosition = new Vector2(0f, -200f);
            background.rectTransform.anchoredPosition = new Vector2(0f, -200f);
            background.color = new Color(0f, 0f, 0f, 0f);
            var alphaFade = text.DOFade(1f, fadeDuration);
            var bgFade = background.DOFade(0.5f, fadeDuration);
            var moveUp = text.rectTransform.DOAnchorPosY(-250f, fadeDuration);
            var bgMoveUp = background.rectTransform.DOAnchorPosY(-250f, fadeDuration);
            tween = DOTween.Sequence().Append(alphaFade).Join(bgFade).Join(moveUp).Join(bgMoveUp)
                .AppendInterval(showDuration)
                .OnComplete(() => Hide());
        }

        private void Hide(float duration = 0.2f)
        {
            tween.Kill();
            var alphaFade = text.DOFade(0f, duration);
            var bgFade = background.DOFade(0f, duration);
            var moveDown = text.rectTransform.DOAnchorPosY(-200f, duration);
            var bgMoveDown = background.rectTransform.DOAnchorPosY(-200f, duration);
            tween = DOTween.Sequence().Append(alphaFade).Join(bgFade).Join(moveDown).Join(bgMoveDown).OnComplete(() =>
            {
                gameObject.SetActive(false);
                text.text = "";
            });
        }
    }
}