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
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> midCellGrid, int entityPoolId)
        {
            if (entityType != EntityType.Edge)
                return;

            base.CreateEntity(entityType, x, y, nodeGrid, midCellGrid, entityPoolId);

            var startNodeObject = nodeGrid.GetValue(x, y);
            NodeManager startNode = startNodeObject.GetValue();

            if (startNodeObject.GetNeighbourGridObject(Enums.Direction.Right) != null)
            {
                var rightGridNodeObject = startNodeObject.GetNeighbourGridObject(Direction.Right);
                NodeManager rightNode = rightGridNodeObject.GetValue();
                
                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity);

                startNode.SetEdge(Direction.Right, edgeManager);
                rightNode.SetEdge(Direction.Left, edgeManager);

                if (edgeManager != null)
                    edgeManager.Setup(startNodeObject, rightGridNodeObject, nodeGrid);
            }

            if (startNodeObject.GetNeighbourGridObject(Enums.Direction.Down) != null)
            {
                var downGridNodeObject = startNodeObject.GetNeighbourGridObject(Enums.Direction.Down);
                NodeManager downNode = downGridNodeObject.GetValue();

                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity, transform);

                startNode.SetEdge(Direction.Down, edgeManager);
                downNode.SetEdge(Direction.Up, edgeManager);

                if (edgeManager != null)
                    edgeManager.Setup(startNodeObject, downGridNodeObject, nodeGrid);
            }

            //await UniTask.DelayFrame(1);
        }
    }
}
