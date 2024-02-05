using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Codebase.Utils.UI
{
    public class Countdown : MonoBehaviour
    {
        [SerializeField] private RectTransform _circle;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;

        public void Activate(bool isActive)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.gameObject.SetActive(isActive);
            _circle.gameObject.SetActive(isActive);
        }

        public void ShowText(string text)
        {
            DOTween.Kill(_canvasGroup);
            _text.text = text;
            _canvasGroup.alpha = 1f;
            _canvasGroup.DOFade(0f, 1f);
        }
    }
}