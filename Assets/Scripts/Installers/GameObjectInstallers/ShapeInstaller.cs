using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Zenject;

namespace Installers.GameObject
{
    public class ShapeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ShapeManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShapeLocomotionManager>().FromComponentsInHierarchy().AsTransient();
        }
    }
}
