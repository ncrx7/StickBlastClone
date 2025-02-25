using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

namespace NodeGridSystem.View
{
    public class EdgeGridLevelCreator : EntityLevelCreator<EdgeManager>
    {
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> grid, int entityPoolId)
        {
            if (entityType != EntityType.Edge)
                return;

            base.CreateEntity(entityType, x, y, grid, entityPoolId);

            var gridNodeObject = grid.GetValue(x, y);

            if (gridNodeObject.GetNeighbourGridObject(Enums.NeighbourDirection.Right) != null)
            {
                var rightGridNodeObject = gridNodeObject.GetNeighbourGridObject(Enums.NeighbourDirection.Right);
                
                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity);

                if (edgeManager != null)
                    edgeManager.Setup(gridNodeObject, rightGridNodeObject, grid);
            }

            if (gridNodeObject.GetNeighbourGridObject(Enums.NeighbourDirection.Down) != null)
            {
                var downGridNodeObject = gridNodeObject.GetNeighbourGridObject(Enums.NeighbourDirection.Down);

                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity);

                if (edgeManager != null)
                    edgeManager.Setup(gridNodeObject, downGridNodeObject, grid);
            }

            //await UniTask.DelayFrame(1);
        }
    }
}
