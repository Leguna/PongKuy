using System;
using SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace ToC
{
    public class ToCController : MonoBehaviour
    {
        [SerializeField] private Button agreeButton;
        [SerializeField] private Button disagreeButton;

        private Action<bool> onButtonClick;
        private readonly ToCAgreement toCAgreement = new();

        public void Init(Action<bool> newOnButtonClick)
        {
            SaveLoadSystem.Load(toCAgreement);
            if (toCAgreement.isAgreed)
            {
                newOnButtonClick?.Invoke(true);
                gameObject.SetActive(false);
                return;
            }

            Open();
            onButtonClick = newOnButtonClick;
            agreeButton.onClick.AddListener(() =>
            {
                onButtonClick?.Invoke(true);
                toCAgreement.isAgreed = true;
                SaveLoadSystem.Save(toCAgreement);
                Close();
            });
            disagreeButton.onClick.AddListener(() =>
            {
                onButtonClick?.Invoke(false);
                toCAgreement.isAgreed = false;
                SaveLoadSystem.Save(toCAgreement);
                Close();
            });
        }

        private void Open() => gameObject.SetActive(true);
        private void Close() => gameObject.SetActive(false);
    }


    [Serializable]
    public class ToCAgreement : ISaveable
    {
        public bool isAgreed;

        public string GetUniqueIdentifier()
        {
            var type = GetType();
            return $"{type}";
        }

        public object CaptureState()
        {
            return JsonUtility.ToJson(this);
        }

        public void RestoreState(object state)
        {
            JsonUtility.FromJsonOverwrite((string)state, this);
        }
    }
}