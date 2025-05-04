using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModel;
using Enums;
using NodeGridSystem.Controllers;
using Shapes;
using UnityEngine;
using UnityUtils.Core.VfxSystem;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Inject] GameSettings _gameSettings;

        private Dictionary<ShapeType, ShapePool<ShapeType>> _shapePoolMap = new();
        private Dictionary<VfxType, VfxPool<VfxType>> _vfxPoolMap = new();

        public override void InstallBindings()
        {
            CreateShapePoolsBinding();

            CreateVfxPoolsBinding();

            Container.Bind<ShapeFactory<ShapeType>>().AsSingle();
            Container.Bind<Dictionary<ShapeType, ShapePool<ShapeType>>>().FromInstance(_shapePoolMap).AsSingle();

            Container.Bind<VfxFactory<VfxType>>().AsSingle();
            Container.Bind<Dictionary<VfxType, VfxPool<VfxType>>>().FromInstance(_vfxPoolMap).AsSingle();

            Container.Bind<NodeGridBoardManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
        }

        private void CreateShapePoolsBinding()
        {
            foreach (var shape in _gameSettings.ShapeData)
            {
                Container.BindMemoryPool<ShapeManager, ShapePool<ShapeType>>().WithId(shape.Type).WithInitialSize(10).FromSubContainerResolve().ByNewContextPrefab(shape.ShapePrefab).UnderTransformGroup("Shapes");
            }
        }

        private void CreateVfxPoolsBinding()
        {
            foreach (var vfx in _gameSettings.VfxData)
            {
                Container.BindMemoryPool<BaseVfxController<VfxType>, VfxPool<VfxType>>().WithId(vfx.Type).WithInitialSize(5).FromSubContainerResolve().ByNewContextPrefab(vfx.VfxPrefab).UnderTransformGroup("Vfxs");
            }
        }

        public async override void Start()
        {
            await InitializeShapePoolMap();

            await UniTask.Delay(100);

            await InitializeVfxPoolMap();

            MiniEventSystem.OnCompleteSceneInit?.Invoke();
        }

        private async UniTask InitializeShapePoolMap()
        {
            foreach (var shape in _gameSettings.ShapeData)
            {
                _shapePoolMap[shape.Type] = Container.ResolveId<ShapePool<ShapeType>>(shape.Type);
            }

            await UniTask.Delay(100);
        }

        private async UniTask InitializeVfxPoolMap()
        {
            foreach (var vfx in _gameSettings.VfxData)
            {
                _vfxPoolMap[vfx.Type] = Container.ResolveId<VfxPool<VfxType>>(vfx.Type);
            }

            await UniTask.Delay(100);
        }
    }
}
