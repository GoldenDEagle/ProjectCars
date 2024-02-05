using TMPro;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizeTMPText : MonoBehaviour
    {
        public string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TMP_Text>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}