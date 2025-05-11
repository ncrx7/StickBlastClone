using System;
using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;
using Data.Model;
using Shapes;

public static class MiniEventSystem 
{
    public static Action<GameData> OnCompleteGameDataLoad;
    public static Action OnCompleteSceneInit;
    public static Action<EntityType, int, int, NodeGridSystem2D<GridNodeObject<NodeManager>>, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>, int> OnCreateEntity;
    public static Action<ShapeManager> OnPlaceShape;
    public static Action<SoundType> PlaySoundClip;
    public static Action<Vector2, VfxType> PlayVfx;
    public static Action OnMidCellFill;
    public static Action OnComboIncrease;
    public static Action OnStartGame;
    public static Action<int> OnEndGame;
    public static Action<int> IncreaseScore;
    public static Action ActivateLoadingUI;
    public static Action DeactivateLoadingUI;
    public static Action<int> OnTimerWork;

    #region BUTTON ACTIONS
    public static Action OnClickHomePanelButton;
    public static Action OnClickLevelPanelButton;
    public static Action OnClickSettingsPanelButton;
    public static Action OnClickStartGameButton;
    #endregion
}
