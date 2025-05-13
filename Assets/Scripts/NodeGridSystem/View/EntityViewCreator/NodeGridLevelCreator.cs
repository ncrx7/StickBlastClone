using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;
using Zenject;
using NodeGridSystem.Controllers.EntityScalers;

namespace NodeGridSystem.View
{
    public class NodeGridLevelCreator : EntityLevelCreator<NodeManager>
    {
        #region References
        [SerializeField] private Transform _transformHolder;
        [Inject] private NodeGridBoardManager _nodeGridBoardManager;
        [Inject] private EntityScaler _entityScaler;
        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region Private Methods
        protected override void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> middCellGrid, int nodePoolId)
        {
            if (entityType != EntityType.NodeGrid)
                return;

            base.CreateEntity(entityType, x, y, nodeGrid, middCellGrid, nodePoolId);

            //TODO: PULL NODE OBJECT FROM OBJECT POOLER
            NodeManager node = Instantiate(_entityPrefab, nodeGrid.GetWorldPositionCenter(x, y), Quaternion.identity, _transformHolder);

            node.transform.position = nodeGrid.GetWorldPositionCenter(x, y);

            node.transform.SetParent(_transformHolder);

            node.transform.localScale = _entityScaler.NodeTargetScale;

            var gridObject = new GridNodeObject<NodeManager>(nodeGrid, x, y);
            gridObject.InitNeighbourGridObjects();

            gridObject.SetValue(node);
            nodeGrid.SetValue(x, y, gridObject);

            node.SetGridObjectOnNode(gridObject);
        }

        protected override void HandleEntityScale()
        {
            _entityScaler.CalculateNodeScaleFactor(_entityPrefab, _nodeGridBoardManager);
        }
        #endregion
    }
}
