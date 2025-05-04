using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Zenject;

public class ShapePool<Ttype> : MonoMemoryPool<ShapeManager> where Ttype : Enum
{
    protected override void OnCreated(ShapeManager item)
    {
        item.gameObject.SetActive(false);
    }

    protected override void OnSpawned(ShapeManager item)
    {
        item.gameObject.SetActive(true);
    }

    protected override void OnDespawned(ShapeManager item)
    {
        item.gameObject.SetActive(false);

        item.Reset();
    }

}
