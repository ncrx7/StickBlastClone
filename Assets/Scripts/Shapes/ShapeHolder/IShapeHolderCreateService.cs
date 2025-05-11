using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Shapes;
using UnityEngine;

public interface IShapeHolderCreateService
{
    public void InitializeShapeHolder(Transform startpoint, Transform endPoint);
    public UniTask HandleCreateShapes();
    public UniTask RelocationShapes();
    public void OnPlaceCallBack(ShapeManager shapeManager);


    public Transform QueueStartingPoint { get; set; }
    public Transform QueueEndPoint { get; set; }
}
