using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace NodeGridSystem.Controllers
{
    public class NodeCollisionManager : MonoBehaviour
    {
        [SerializeField] private NodeManager _mainNodeManager;
        private NodeManager _currentNodeManager;
        public bool CanPlace = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("COLLISION WORKED!!!!!");
            _currentNodeManager = _mainNodeManager;
            List<EdgeManager> edges = new();

            if (other.TryGetComponent<ShapeManager>(out ShapeManager shapeManager))
            {
                CheckShapePath(shapeManager, edges);
            }
        }

        private void CheckShapePath(ShapeManager shapeManager, List<EdgeManager> edges)
        {
            foreach (var direction in shapeManager.GetShapeData.ShapeDirections)
            {
                EdgeManager currentEdge = _currentNodeManager.GetNodeEdge(direction);

                if (currentEdge == null)
                    return;

                edges.Add(currentEdge);

                if (currentEdge.IsEmpty == false)
                {
                    Debug.LogError("Edge is not empty, cant place!!" + currentEdge.gameObject.name);
                    CanPlace = false;
                    return;
                }

                _currentNodeManager = _currentNodeManager.OnGridNodeObject.GetNeighbourGridObject(direction).GetValue();
            }

            Debug.Log("Can Place Shape!!");
            CanPlace = true;
        }
    }
}
