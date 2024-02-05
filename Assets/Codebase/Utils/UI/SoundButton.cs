using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Assets.Codebase.Models.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Utils.UI
{
    [RequireComponent(typeof(Button))]
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Button _switchButton;
        [SerializeField] private Image _soundOnImage;
        [SerializeField] private Image _soundOffImage;

        private IProgressModel _progressModel;

        public Button Button => _switchButton;

        private void Awake()
        {
            _progressModel = ServiceLocator.Container.Single<IModelAccessService>().ProgressModel;
        }

        private void Start()
        {
            SetIcon();
        }

        public void SetIcon()
        {
            if (_progressModel.SessionProgress.SFXVolume.Value > 0.5f)
            {
                _soundOnImage.gameObject.SetActive(true);
                _soundOffImage.gameObject.SetActive(false);
            }
            else
            {
                _soundOnImage.gameObject.SetActive(false);
                _soundOffImage.gameObject.SetActive(true);
            }
        }
    }
}