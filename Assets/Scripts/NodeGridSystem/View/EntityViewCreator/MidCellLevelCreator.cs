using System.Collections;
using System.Collections.Generic;
using Enums;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using NodeGridSystem.View;
using UnityEngine;
using Zenject;

namespace NodeGridSystem.View
{
    public class MidCellLevelCreator : EntityLevelCreator<MiddleFillAreaManager>
    {
        [SerializeField] private Transform _transformHolder;
        private NodeGridBoardManager _nodeGridBoardManager;

        [Inject]
        private void InitializeDependencies(NodeGridBoardManager nodeGridBoardManager)
        {
            _nodeGridBoardManager = nodeGridBoardManager;
        }

        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> midCellGrid, int nodePoolId)
        {
            if(entityType != EntityType.MidCell)
                return;
            

            base.CreateEntity(entityType, x, y, nodeGrid, midCellGrid, nodePoolId);
            
            MiddleFillAreaManager middleArea = Instantiate(_entityPrefab, midCellGrid.GetWorldPositionCenter(x, y), Quaternion.identity, _transformHolder);

            middleArea.transform.position = midCellGrid.GetWorldPositionCenter(x, y) +
                             new Vector3(_nodeGridBoardManager.AutomaticBoardCellSize / 2, _nodeGridBoardManager.AutomaticBoardCellSize / 2);

            middleArea.transform.SetParent(_transformHolder);

            var gridObject = new GridNodeObject<MiddleFillAreaManager>(midCellGrid, x, y);
            gridObject.InitNeighbourGridObjects();

            gridObject.SetValue(middleArea);
            midCellGrid.SetValue(x, y, gridObject);

            middleArea.SetGridObjectOnMiddleArea(gridObject);

            middleArea.Setup(x, y, nodeGrid);
        }
    }
}
