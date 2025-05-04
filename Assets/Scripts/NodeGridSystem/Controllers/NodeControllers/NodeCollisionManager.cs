using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace NodeGridSystem.Controllers
{
    public class NodeCollisionManager : MonoBehaviour
    {
        #region References
        [SerializeField] private NodeManager _mainNodeManager;
        private NodeManager _currentNodeManager;
         //TODO: This reference will hold in shape manager
        #endregion

        #region MonoBehaviour Callbacks
        private void OnTriggerEnter2D(Collider2D other)
        {
            _currentNodeManager = _mainNodeManager;


            if (other.TryGetComponent<ShapeManager>(out ShapeManager shapeManager))
            {
                shapeManager.GetEdgesMatching.Clear();
                CheckShapePath(shapeManager, shapeManager.GetEdgesMatching, false);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<ShapeManager>(out ShapeManager shapeManager))
            {
                if(shapeManager.IsDragging)
                {
                    HideBlockShapeSlotSign(shapeManager);
                    shapeManager.SetCanPlaceFlag(false);
                }
            }
        }
        #endregion

        #region Private Methods
        public bool CheckShapePath(ShapeManager shapeManager, List<EdgeManager> edges, bool onlyCheck)
        {
            _currentNodeManager = _mainNodeManager;
            foreach (var direction in shapeManager.GetShapeData.ShapeDirections)
            {
                Debug.Log("current node: " + _currentNodeManager.gameObject.name);
                EdgeManager currentEdge = _currentNodeManager.GetNodeEdge(direction);

                if (currentEdge == null)
                {
                    edges.Clear();
                    shapeManager.SetCanPlaceFlag(false);
                    return false;
                }

                if(!onlyCheck) edges.Add(currentEdge);

                if (currentEdge.IsEmpty == false)
                {
                    edges.Clear();
                    //Debug.LogError("Edge is not empty, cant place!!" + currentEdge.gameObject.name);
                    shapeManager.SetCanPlaceFlag(false);
                    return false;
                }

                _currentNodeManager = _currentNodeManager.OnGridNodeObject.GetNeighbourGridObject(direction).GetValue();
            }

            Debug.Log("Can Place Shape!!");

            ShowBlockShapeSlotSign(shapeManager);

            if(!onlyCheck) shapeManager.SetCanPlaceFlag(true);

            return true;
        }

        private void ShowBlockShapeSlotSign(ShapeManager shapeManager)
        {
            if(shapeManager.GetEdgesMatching.Count == 0)
                return;

            foreach (var edge in shapeManager.GetEdgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 0.25f;
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;

                edge.StartNode.IndicatorPaintNode();
                edge.EndNode.IndicatorPaintNode();
            }
        }

        private void HideBlockShapeSlotSign(ShapeManager shapeManager)
        {
            if(shapeManager.GetEdgesMatching.Count == 0)
                return;
                
            foreach (var edge in shapeManager.GetEdgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 0f;
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;

                if(!edge.StartNode.NodePainted) edge.StartNode.ResetNode();
                if(!edge.EndNode.NodePainted) edge.EndNode.ResetNode();
            }
        }
        #endregion
    }
}
