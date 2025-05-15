using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModel;
using EntitiesData.Shapes;
using Enums;
using NodeGridSystem.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Shapes
{
    public class ShapeManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private NodeGridBoardManager _nodeGridBoardManager;
        private GameSettings _gameSettings;

        [SerializeField] private List<SpriteRenderer> _spriteRendererList;
        [SerializeField] private ShapeData _shapeData;
        [SerializeField] private bool _canPlace;
        [SerializeField] private bool _canMove = false;
        [SerializeField] private Transform _headTransform;

        private List<EdgeManager> _edgesMatching = new();
        public bool IsDragging;

        public Action _placeCallBack;

        [Inject]
        private void InitializeDependencies(NodeGridBoardManager nodeGridBoardManager, GameManager gameManager, GameSettings gameSettings)
        {
            _nodeGridBoardManager = nodeGridBoardManager;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
        }

        private void Start()
        {
            foreach (var spriteRenderer in _spriteRendererList)
            {
                spriteRenderer.color = _gameManager.GetLevelData.LevelColor;
            }
        }

        public void Setup(Action placeCallBack, Vector3 targetPosition)
        {
            transform.position = targetPosition;
            transform.localScale = _gameSettings.ShapeDefaultScale;
            _placeCallBack = placeCallBack;
        }

        public void SetCanPlaceFlag(bool flag)
        {
            _canPlace = flag;
        }

        public void SetCanMoveFlag(bool flag)
        {
            _canMove = flag;
        }

        public void PlaceShape()
        {
            if (GetEdgesMatching.Count == 0)
                return;

            EdgeManager firstEdge = GetEdgesMatching[0];

            foreach (var edge in GetEdgesMatching)
            {
                edge.PaintEdge();
                edge.IsEmpty = false;
                edge.StartNode.PaintNode();
                edge.EndNode.PaintNode();

                MiniEventSystem.PlayVfx?.Invoke(edge.transform.position, VfxType.Place);
            }
            
            _nodeGridBoardManager.CheckMidCellFullnessOnBoard(_edgesMatching);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.PlaceShape);

            _placeCallBack?.Invoke();
            //Destroy(this.gameObject);
        }

        public bool CheckRelativeMatchExist()
        {
            for (int x = 0; x < _nodeGridBoardManager.GetWidth; x++)
            {
                for (int y = 0; y < _nodeGridBoardManager.GetHeight; y++)
                {
                    var gridNodeObject = _nodeGridBoardManager.GetNodeGridSystem2D.GetValue(x, y);

                    NodeManager nodeManager = gridNodeObject.GetValue();

                    if (PathChecker.CheckPathFromANode(this, nodeManager, GetEdgesMatching, true)) //nodeManager.GetNodeCollisionManager.CheckShapePath(this, GetEdgesMatching, true)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public void Reset()
        {
            SetCanMoveFlag(false);
            SetCanPlaceFlag(false);
            _placeCallBack = null;
            _edgesMatching.Clear();
        }

        public ShapeData GetShapeData => _shapeData;
        public List<EdgeManager> GetEdgesMatching => _edgesMatching;
        public bool GetCanPlaceFlag => _canPlace;
        public bool GetCanMoveFlag => _canMove;
        public Transform GetHead => _headTransform;
        public List<SpriteRenderer> GetAllSprites => _spriteRendererList;
    }
}
