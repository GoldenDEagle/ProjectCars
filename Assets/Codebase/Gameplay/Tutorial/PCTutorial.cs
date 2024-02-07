using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Gameplay.Tutorial
{
    public class PCTutorial : MonoBehaviour
    {
        [SerializeField] private Image _tutorialBackground;
        [SerializeField] private Button _okButton;

        public event Action OnTutorialClosed;

        private void OnEnable()
        {
            _okButton.onClick.AddListener(OkWasClicked);
        }

        private void OnDisable()
        {
            _okButton.onClick.RemoveListener(OkWasClicked);
        }

        private void OkWasClicked()
        {
            OnTutorialClosed?.Invoke();
            Destroy(gameObject);
        }
    }
}