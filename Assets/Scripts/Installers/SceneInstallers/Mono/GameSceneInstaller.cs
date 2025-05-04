using System.Collections;
using System.Collections.Generic;
using DataModel;
using Enums;
using Shapes;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Inject] GameSettings _gameSettings;
        private Dictionary<ShapeType, ShapePool<ShapeType>> _shapePoolMap = new();

        public override void InstallBindings()
        {
            CreateShapePoolsBinding();

            Container.Bind<ShapeFactory<ShapeType>>().AsSingle();

            Container.Bind<Dictionary<ShapeType, ShapePool<ShapeType>>>().FromInstance(_shapePoolMap).AsSingle();
        }

        private void CreateShapePoolsBinding()
        {
            foreach (var shape in _gameSettings.ShapeData)
            {
                Container.BindMemoryPool<ShapeManager, ShapePool<ShapeType>>().WithId(shape.Type).WithInitialSize(10).FromSubContainerResolve().ByNewContextPrefab(shape.ShapePrefab).UnderTransformGroup("Shapes");
            }
        }

        public override void Start()
        {
            foreach (var shape in _gameSettings.ShapeData)
            {
                _shapePoolMap[shape.Type] = Container.ResolveId<ShapePool<ShapeType>>(shape.Type);
            }

            MiniEventSystem.OnCompleteSceneInit?.Invoke();
        }
    }
}
