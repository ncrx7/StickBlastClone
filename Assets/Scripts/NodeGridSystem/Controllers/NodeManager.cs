using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

namespace NodeGridSystem.Controllers
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class NodeManager : MonoBehaviour
    {
        #region References
        [SerializeField] private SpriteRenderer _nodeSpriteRenderer;
        public GridNodeObject<NodeManager> OnGridNodeObject {get; private set;}

        public Dictionary<Direction, EdgeManager> _nodeEdges = new();
        #endregion
        
        #region Public Methods
        public void SetGridObjectOnNode(GridNodeObject<NodeManager> gridNodeObject)
        {
            OnGridNodeObject = gridNodeObject;
        }

        public void SetEdge(Direction direction, EdgeManager edgeManager)
        {
            _nodeEdges[direction] = edgeManager;
        }

        public EdgeManager GetNodeEdge(Direction direction)
        {
            return _nodeEdges[direction];
        }

        public Dictionary<Direction, EdgeManager> GetAllNodeEdges => _nodeEdges;
        #endregion
    }
}


//TODO: I CAN ADD NODE TYPE TO ACCEPT SAME TYPE OF BLOCK DRAGGED IN THE FUTURE
        //..type, SetType(set sprite renderer), GetType