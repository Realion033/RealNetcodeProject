using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NoNameGun
{
    public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region EVT_FUNC
        public event Action<PointerEventData> OnClickHandler = null;
        public event Action<PointerEventData> OnPointerDownHandler = null;
        public event Action<PointerEventData> OnPointerUpHandler = null;
        public event Action<PointerEventData> OnDragHandler = null;
        #endregion

        #region MAIN_FUNC
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            OnDragHandler?.Invoke(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownHandler?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpHandler?.Invoke(eventData);
        }
        #endregion
    }
}
