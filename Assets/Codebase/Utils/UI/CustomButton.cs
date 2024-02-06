using Assets.Codebase.Data.Audio;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Codebase.Utils.UI
{
    public class CustomButton : Button
    {
        [SerializeField] private bool _idleAnimation = false;
        [SerializeField] private bool _playSoundOnClick = false;
        [SerializeField] private SoundId _soundId;

        private IAudioService _audioService;

        protected override void Awake()
        {
            base.Awake();

            _audioService = ServiceLocator.Container.Single<IAudioService>();
        }

        protected override void Start()
        {
            base.Start();

            if (_idleAnimation)
            {
                targetGraphic.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f), 2f, 1, 0.1f).OnComplete(RepeatIdleAnimation);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (_playSoundOnClick)
            {
                _audioService.PlaySfxSound(_soundId);
            }
        }

        private void RepeatIdleAnimation()
        {
            targetGraphic.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f), 2f, 1, 0.1f).OnComplete(RepeatIdleAnimation);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!_idleAnimation) return;
            DOTween.Kill(targetGraphic.rectTransform);
        }
    }
}
