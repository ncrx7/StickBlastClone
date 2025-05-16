using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;
using Zenject;
using NodeGridSystem.Controllers.EntityScalers;

namespace NodeGridSystem.View
{
    public class EdgeGridLevelCreator : EntityLevelCreator<EdgeManager>
    {
        [SerializeField] private Transform _transformHolder;

        [Inject] private NodeGridBoardManager _nodeGridBoardManager;
        [Inject] private EntityScaler _entityScaler;

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

                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity, _transformHolder);

                _nodeGridBoardManager.GetAllEdgesOnBoard.Add(edgeManager);

                startNode.SetEdge(Direction.Right, edgeManager);
                rightNode.SetEdge(Direction.Left, edgeManager);

                if (edgeManager != null)
                    edgeManager.Setup(startNodeObject, rightGridNodeObject, nodeGrid, _entityScaler);
            }

            if (startNodeObject.GetNeighbourGridObject(Enums.Direction.Down) != null)
            {
                var downGridNodeObject = startNodeObject.GetNeighbourGridObject(Enums.Direction.Down);
                NodeManager downNode = downGridNodeObject.GetValue();

                EdgeManager edgeManager = Instantiate(_entityPrefab, Vector3.zero, Quaternion.identity, _transformHolder);

                _nodeGridBoardManager.GetAllEdgesOnBoard.Add(edgeManager);

                startNode.SetEdge(Direction.Down, edgeManager);
                downNode.SetEdge(Direction.Up, edgeManager);

                if (edgeManager != null)
                    edgeManager.Setup(startNodeObject, downGridNodeObject, nodeGrid, _entityScaler);
            }

            //await UniTask.DelayFrame(1);
        }

        protected override void HandleEntityScale()
        {
            _entityScaler.CalculateEdgeScaleFactor(_entityPrefab, _nodeGridBoardManager);
        }
    }
}
