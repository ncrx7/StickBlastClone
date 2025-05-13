using System;
using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Controllers.EntityScalers;
using NodeGridSystem.Models;
using UnityEngine;
using Zenject;

namespace NodeGridSystem.Controllers
{
    public class EdgeManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private NodeGridBoardManager _nodeGridBoardManager;

        [SerializeField] private SpriteRenderer _edgeSprite;
        [SerializeField] private SpriteRenderer _blockShapeSprite;
        private Color _blockShapeDefaultColor;
        public NodeManager StartNode;
        public NodeManager EndNode;

        [SerializeField] private List<MiddleFillAreaManager> _midCellAreasBelongsTo; 


        public bool IsEmpty = true;

        [Inject]
        private void InitializeDependencies(GameManager gameManager, NodeGridBoardManager nodeGridBoardManager)
        {
            
            _gameManager = gameManager;
            _nodeGridBoardManager = nodeGridBoardManager;
        }

        private void Start()
        {
            _blockShapeDefaultColor = _blockShapeSprite.color;
        }

        public void Setup(GridNodeObject<NodeManager> startGridNodeObject, GridNodeObject<NodeManager> endGridNodeObject, NodeGridSystem2D<GridNodeObject<NodeManager>> gridNodeSystem, EntityScaler entityScaler) //Vector2 start, Vector2 end
        {
            transform.localScale = entityScaler.EdgeTargetScale;

            StartNode = startGridNodeObject.GetValue();
            EndNode = endGridNodeObject.GetValue();

            Vector2 startPos = gridNodeSystem.GetWorldPositionCenter(startGridNodeObject.GetX, startGridNodeObject.GetY);
            Vector2 endPos = gridNodeSystem.GetWorldPositionCenter(endGridNodeObject.GetX, endGridNodeObject.GetY);

            transform.position = (Vector2)(startPos + endPos) / 2;

            Vector2 direction = (endPos - startPos);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void ResetEdge()
        {
            foreach (var midCell in _midCellAreasBelongsTo)
            {
                if(midCell.IsFilled)
                    return;
            }

            _blockShapeSprite.color = _blockShapeDefaultColor;

            IsEmpty = true;

            if (StartNode.AllEdgesEmpty())
                StartNode.ResetNode();

            if (EndNode.AllEdgesEmpty())
                EndNode.ResetNode();
        }

        public void PaintEdge()
        {
            GetBlockShapeSpriteRenderer.color = _gameManager.GetLevelData.LevelColor;
        }

        public void AddMidCellToList(MiddleFillAreaManager midCell)
        {
            _midCellAreasBelongsTo.Add(midCell);
        }

        public SpriteRenderer GetEdgeSprite => _edgeSprite;
        public SpriteRenderer GetBlockShapeSpriteRenderer => _blockShapeSprite;
        public Color GetDefaultColor => _blockShapeDefaultColor;
        public List<MiddleFillAreaManager> GetMidCells => _midCellAreasBelongsTo;
    }
}
