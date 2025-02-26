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
        private List<EdgeManager> _edgesMatching = new(); //TODO: This reference will hold in shape manager
        #endregion

        #region MonoBehaviour Callbacks
        private void OnTriggerEnter2D(Collider2D other)
        {
            _edgesMatching.Clear();

            _currentNodeManager = _mainNodeManager;


            if (other.TryGetComponent<ShapeManager>(out ShapeManager shapeManager))
            {
                CheckShapePath(shapeManager, _edgesMatching);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<ShapeManager>(out ShapeManager shapeManager))
            {
                HideBlockShapeSlotSign();
            }
        }
        #endregion

        #region Private Methods
        private void CheckShapePath(ShapeManager shapeManager, List<EdgeManager> edges)
        {
            foreach (var direction in shapeManager.GetShapeData.ShapeDirections)
            {
                EdgeManager currentEdge = _currentNodeManager.GetNodeEdge(direction);

                if (currentEdge == null)
                {
                    shapeManager.SetCanPlaceFlag(false);
                    return;
                }

                edges.Add(currentEdge);

                if (currentEdge.IsEmpty == false)
                {
                    Debug.LogError("Edge is not empty, cant place!!" + currentEdge.gameObject.name);
                    shapeManager.SetCanPlaceFlag(false);
                    return;
                }

                _currentNodeManager = _currentNodeManager.OnGridNodeObject.GetNeighbourGridObject(direction).GetValue();
            }

            Debug.Log("Can Place Shape!!");

            ShowBlockShapeSlotSign();

            shapeManager.SetCanPlaceFlag(true);
        }

        private void ShowBlockShapeSlotSign()
        {
            foreach (var edge in _edgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 0.33f;
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;
            }
        }

        private void HideBlockShapeSlotSign()
        {
            foreach (var edge in _edgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 0f;
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;
            }
        }
        #endregion
    }
}
