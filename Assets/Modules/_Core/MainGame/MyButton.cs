using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainGame
{
    public class MyButton : Button
    {
        protected override void Start()
        {
            base.Awake();
            onClick.AddListener(() => AudioService.Instance.PlayAudio(AudioService.AudioType.Click));
        }
    }
}