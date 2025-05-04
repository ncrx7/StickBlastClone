using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityUtils.Core.VfxSystem;
using Zenject;

namespace Installers.GameObject
{
    public class VfxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BaseVfxController<VfxType>>().FromComponentInHierarchy().AsSingle();
        }
    }
}
