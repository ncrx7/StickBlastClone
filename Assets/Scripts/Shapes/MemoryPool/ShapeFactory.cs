using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Zenject;

public class ShapeFactory<Ttype> : IFactory<Ttype, Vector3, ShapeManager> where Ttype : Enum
{
    [Inject] private readonly Dictionary<Ttype, ShapePool<Ttype>> _shapePoolMap;

    public ShapeManager Create(Ttype type, Vector3 targetPosition)
    {
        var pool = _shapePoolMap[type];

        var shapeObject = pool.Spawn();

        shapeObject.Setup(() => pool.Despawn(shapeObject), targetPosition);

        return shapeObject;
    }
}
