using System.Collections;
using System.Collections.Generic;
using Mainpanel;
using UnityEngine;
using Zenject;

namespace Installers.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
            Debug.Log("project context bindings");
        }
    }
}
