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
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> grid, int nodePoolId)
        {
            if(entityType != EntityType.NodeGrid)
                return;

            base.CreateEntity(entityType, x, y, grid, nodePoolId);

            //TODO: PULL NODE OBJECT FROM OBJECT POOLER
            NodeManager node = Instantiate(_entityPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform); 

            node.transform.position = grid.GetWorldPositionCenter(x, y);
            node.transform.SetParent(transform);

            var gridObject = new GridNodeObject<NodeManager>(grid, x, y);
            gridObject.InitNeighbourGridObjects();

            gridObject.SetValue(node); 
            grid.SetValue(x, y, gridObject); 

            node.SetGridObjectOnNode(gridObject);
        }
        #endregion
    }
}
