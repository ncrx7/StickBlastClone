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
        if(_shapePoolMap == null)
            Debug.Log("shape pool map is null");
        else
        {
            Debug.Log("shape pool is not null -> " + _shapePoolMap.Count);
        }
        foreach (var kvp in _shapePoolMap)
        {
            Debug.Log($"key: {kvp.Key} value: {kvp.Value}");
        }

        var shapeObject = _shapePoolMap[type].Spawn();

        shapeObject.transform.position = targetPosition;

        return shapeObject;
    }
}
