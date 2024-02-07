using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Gameplay.Tutorial
{
    public class MobileTutorial : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _tutorialBubbles;
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

        public void ActivateTutorial()
        {
            _tutorialBackground.gameObject.SetActive(true);
            foreach (var element in _tutorialBubbles)
            {
                element.gameObject.SetActive(true);
            }
        }

        public void DeactivateTutorial()
        {
            _tutorialBackground.gameObject.SetActive(false);
            foreach (var element in _tutorialBubbles)
            {
                element.gameObject.SetActive(false);
            }
        }

        private void OkWasClicked()
        {
            DeactivateTutorial();
            OnTutorialClosed?.Invoke();
            Destroy(gameObject);
        }
    }
}