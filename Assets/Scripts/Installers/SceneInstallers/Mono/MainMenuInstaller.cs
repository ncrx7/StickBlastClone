using System.Collections;
using System.Collections.Generic;
using Mainpanel;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
