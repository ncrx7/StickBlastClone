using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using NodeGridSystem.Controllers;
using NodeGridSystem.Controllers.EntityScalers;
using NodeGridSystem.Models;
using UnityEngine;
using Zenject;

namespace NodeGridSystem.Controllers
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MiddleFillAreaManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _rectangleSprite;
        [SerializeField] private float _scaleAnimationTime;
        private GameManager _gameManager;
        private NodeGridBoardManager _nodeGridBoardManager;

        public GridNodeObject<MiddleFillAreaManager> OnGridNodeObject { get; private set; }
        public List<EdgeManager> edges = new();

        public bool IsFilled = false;

        private Vector2 _initialLocalScale;
        private Vector2 _localScaleOnFilled;

        [Inject]
        private void InitializeDependencies(GameManager gameManager, NodeGridBoardManager nodeGridBoardManager)
        {
            _gameManager = gameManager;
            _nodeGridBoardManager = nodeGridBoardManager;
        }

        public void AddEdgeToList(EdgeManager edgeManager)
        {
            edges.Add(edgeManager);
        }

        public void SetGridObjectOnMiddleArea(GridNodeObject<MiddleFillAreaManager> gridNodeObject)
        {
            OnGridNodeObject = gridNodeObject;
        }

        public void Setup(int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, EntityScaler entityScaler)
        {
            var nodeObject = nodeGrid.GetValue(x, y);
            NodeManager currentNode = nodeObject.GetValue();

            _initialLocalScale = entityScaler.MidCellTargetScaleOnInitial;
            _localScaleOnFilled = entityScaler.MidCellTargetScaleOnFilled;

            AddEdgeToList(currentNode.GetNodeEdge(Direction.Right));
            AddEdgeToList(edges[^1].EndNode.GetNodeEdge(Direction.Up));
            AddEdgeToList(edges[^1].StartNode.GetNodeEdge(Direction.Left));
            AddEdgeToList(edges[^1].StartNode.GetNodeEdge(Direction.Down));

            foreach (EdgeManager edge in edges)
            {
                edge.AddMidCellToList(this);
            }
        }

        public void OnAllEdgeFull()
        {
            transform.localScale = _initialLocalScale;

            GetSpriteRenderer.enabled = true;

            transform.DOScale(_localScaleOnFilled, _scaleAnimationTime);
            
            IsFilled = true;

            PaintMidCell();

            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.CellFilling);

            MiniEventSystem.PlayVfx?.Invoke(transform.position, VfxType.CellFilling);

            MiniEventSystem.OnMidCellFill?.Invoke();
        }

        public bool CheckEdges()
        {
            foreach (var edge in edges)
            {
                if (edge.IsEmpty == true)
                {
                    return false;
                }
            }

            return true;
        }

        public void ResetEdges()
        {
            foreach (EdgeManager edge in edges)
            {
                edge.ResetEdge();
            }
        }

        public void PaintMidCell()
        {
            GetSpriteRenderer.color = _gameManager.GetLevelData.LevelColor;
        }

        public SpriteRenderer GetSpriteRenderer => _rectangleSprite;
    }
}
