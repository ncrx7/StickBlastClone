using System.Collections;
using System.Collections.Generic;
using Enums;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using NodeGridSystem.View;
using UnityEngine;

namespace NodeGridSystem.View
{
    public class MidCellLevelCreator : EntityLevelCreator<MiddleFillAreaManager>
    {
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> midCellGrid, int nodePoolId)
        {
            if(entityType != EntityType.MidCell)
                return;
            

            base.CreateEntity(entityType, x, y, nodeGrid, midCellGrid, nodePoolId);
            
            MiddleFillAreaManager middleArea = Instantiate(_entityPrefab, midCellGrid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);

            middleArea.transform.position = midCellGrid.GetWorldPositionCenter(x, y) + new Vector3(NodeGridBoardManager.Instance.GetCellSize / 2, NodeGridBoardManager.Instance.GetCellSize / 2 + 0);
            middleArea.transform.SetParent(transform);

            var gridObject = new GridNodeObject<MiddleFillAreaManager>(midCellGrid, x, y);
            gridObject.InitNeighbourGridObjects();

            gridObject.SetValue(middleArea);
            midCellGrid.SetValue(x, y, gridObject);

            middleArea.SetGridObjectOnMiddleArea(gridObject);

            middleArea.Setup(x, y, nodeGrid);
        }
    }
}
