using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Mainpanel;
using StateMachine;
using UnityEngine;
using Zenject;

namespace Installers.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameDataHandler>().FromComponentInHierarchy().AsSingle();

            Debug.Log("project context bindings have completed");
        }
    }
}
