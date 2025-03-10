using System;
using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

public static class MiniEventSystem 
{
    public static Action<EntityType, int, int, NodeGridSystem2D<GridNodeObject<NodeManager>>, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>, int> OnCreateEntity;
    public static Action OnPlaceShape;
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
}
