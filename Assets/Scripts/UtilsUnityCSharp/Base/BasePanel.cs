using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UnityUtils.BaseClasses
{
    public abstract class BasePanel<TType, TData> : MonoBehaviour where TType : Enum
    {
        public TType PanelType;
        public PanelPositionType positionType;
        [SerializeField] private RectTransform _rectTransform;

        protected void SetType(TType panelType)
        {
            PanelType = panelType;
        }

        public virtual void OnOpenPanel(TData data) { }

        public virtual void OnClosePanel(TData data) { }

        public RectTransform GetRectTransform => _rectTransform;
    }
        public enum PanelPositionType { Left, Mid, Right }
}
