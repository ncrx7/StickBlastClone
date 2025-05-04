using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Inject] GameSettings _gameSettings;
        
        public override void InstallBindings()
        {
            
        }
    }
}
