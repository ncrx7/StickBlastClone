using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;
using System.Linq;

namespace NodeGridSystem.Controllers
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class NodeManager : MonoBehaviour
    {
        #region References
        [SerializeField] private SpriteRenderer _nodeSpriteRenderer;
        public GridNodeObject<NodeManager> OnGridNodeObject { get; private set; }

        public Dictionary<Direction, EdgeManager> _nodeEdges = new();
        public bool NodePainted { get; set; }
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
            if (_nodeEdges.TryGetValue(direction, out EdgeManager edge))
            {
                return edge;
            }

            return null;
        }

        public void PaintNode()
        {
            NodePainted = true;
            GetSpriteRenderer.color = GameManager.Instance.GetLevelData.LevelColor;
        }

        public void IndicatorPaintNode()
        {
            GetSpriteRenderer.color = GameManager.Instance.GetLevelData.LevelColor;
        }

        public void ResetNode()
        {
            NodePainted = false;
            _nodeSpriteRenderer.color = new Color32(65, 58, 154, 255);
        }

        public bool AllEdgesEmpty() => _nodeEdges.Values.All(edge => edge.IsEmpty);

        public Dictionary<Direction, EdgeManager> GetAllNodeEdges => _nodeEdges;
        public SpriteRenderer GetSpriteRenderer => _nodeSpriteRenderer;
        #endregion
    }
}


//TODO: I CAN ADD NODE TYPE TO ACCEPT SAME TYPE OF BLOCK DRAGGED IN THE FUTURE
//..type, SetType(set sprite renderer), GetType