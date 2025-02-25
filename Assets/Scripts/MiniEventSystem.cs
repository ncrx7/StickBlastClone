using System;
using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

public static class MiniEventSystem 
{
    public static Action<EntityType, int, int, NodeGridSystem2D<GridNodeObject<NodeManager>>, int> OnCreateNode;
}
