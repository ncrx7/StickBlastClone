using System.Collections;
using System.Collections.Generic;
using Data.Model;
using DG.Tweening;
using Enums;
using Extensions;
using Mainpanel;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using Zenject;

namespace UI.MainMenu.Panels
{
    public class HomePanel : BasePanel<MainPanelType, GameData>
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private StateMachineController _stateMachineController;

        [Header("Buttons")]
        [SerializeField] private Button _startGameButton;

        [Header("Quest")]
        [SerializeField] private Image _questSlider;
        [SerializeField] private TextMeshProUGUI _questText;

        [Header("Gift")]
        [SerializeField] private Image _giftSlider;

        [Header("Tweens")]
        Tween _questSliderTween, _giftSliderTween;

        [Header("Other References")]
        [SerializeField] private RectTransform _collectionButtonIcon;
        [SerializeField] private TextMeshProUGUI _levelButonLabel;
        
        [Header("Settings")]
        [SerializeField] private float _sliderAnimationDuration;
        [SerializeField] private float _collectionIconRotationSpeed;

        private void Start()
        {
            _collectionButtonIcon.RotateYConstantly(_collectionIconRotationSpeed);
        }

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

            _stateMachineController.SwitchState(new HomePanelDisplayingState());

            AnimateSlider(gameData, _questSlider, ref _questSliderTween);

            _questText.text = gameData.UserLevel.ToString() + " /" + " 5";

            AnimateSlider(gameData, _giftSlider, ref _giftSliderTween);

            _levelButonLabel.text = "Level " + (gameData.UserLevel % (_levelManager.GetAllLevelData.Count + 1)).ToString(); 
        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }

        private void AnimateSlider(GameData gameData, Image sliderLine, ref Tween tween)
        {
            if (tween != null)
                tween.Kill();

            sliderLine.fillAmount = 0;

            float sliderValue = gameData.UserLevel / 5f; //QUEST SISTEMI OLUSTURULDUGUNDA 20 DEGERINI GAME DATA QUESTDEN CEK

            tween = sliderLine.DOFillAmount(sliderValue, _sliderAnimationDuration).SetEase(Ease.OutCubic);
        }

        public Button GetStartGameButton => _startGameButton;
    }
}
