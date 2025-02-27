using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

namespace NodeGridSystem.View
{
    public class NodeGridLevelCreator : EntityLevelCreator<NodeManager>
    {
        #region References
        
        #endregion

        #region MonoBehaviour Callbacks
 
        #endregion

        #region Private Methods
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> middCellGrid, int nodePoolId)
        {
            if(entityType != EntityType.NodeGrid)
                return;

            base.CreateEntity(entityType, x, y, nodeGrid, middCellGrid, nodePoolId);

            //TODO: PULL NODE OBJECT FROM OBJECT POOLER
            NodeManager node = Instantiate(_entityPrefab, nodeGrid.GetWorldPositionCenter(x, y), Quaternion.identity, transform); 

            node.transform.position = nodeGrid.GetWorldPositionCenter(x, y);
            node.transform.SetParent(transform);

            var gridObject = new GridNodeObject<NodeManager>(nodeGrid, x, y);
            gridObject.InitNeighbourGridObjects();

            gridObject.SetValue(node); 
            nodeGrid.SetValue(x, y, gridObject); 

            node.SetGridObjectOnNode(gridObject);
        }
        #endregion
    }
}
