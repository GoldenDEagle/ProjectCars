using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Codebase.Gameplay.Input
{
    public class ClickHandler : MonoBehaviour, IPointerDownHandler
    {
        public event Action OnClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}
