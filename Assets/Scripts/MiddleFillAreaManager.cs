using System.Collections;
using System.Collections.Generic;
using Enums;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiddleFillAreaManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rectangleSprite;
    public GridNodeObject<MiddleFillAreaManager> OnGridNodeObject { get; private set; }
    public List<EdgeManager> edges = new();

    public void AddEdgeToList(EdgeManager edgeManager)
    {
        edges.Add(edgeManager);
    }

    public void SetGridObjectOnMiddleArea(GridNodeObject<MiddleFillAreaManager> gridNodeObject)
    {
        OnGridNodeObject = gridNodeObject;
    }

    public void Setup(int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid)
    {
        var nodeObject = nodeGrid.GetValue(x, y);
        NodeManager currentNode = nodeObject.GetValue();

        /*         AddEdgeToList(currentNode.GetNodeEdge(Direction.Right));
                AddEdgeToList(currentNode.GetNodeEdge(Direction.Up));

                var rightNodeObject = nodeObject.GetNeighbourGridObject(Direction.Right);
                NodeManager rightNode = rightNodeObject.GetValue();
                AddEdgeToList(rightNode.GetNodeEdge(Direction.Up));

                var topNodeObject = nodeObject.GetNeighbourGridObject(Direction.Up);
                NodeManager topNode = topNodeObject.GetValue();
                AddEdgeToList(topNode.GetNodeEdge(Direction.Right)); */


/*         EdgeManager firstEdge = currentNode.GetNodeEdge(Direction.Right);
        EdgeManager secondEdge = firstEdge.EndNode.GetNodeEdge(Direction.Up);
        EdgeManager thirdNode = secondEdge.EndNode.GetNodeEdge(Direction.Left);
        EdgeManager fourthEdge = thirdNode.EndNode.GetNodeEdge(Direction.Down);

        AddEdgeToList(firstEdge);
        AddEdgeToList(secondEdge);
        AddEdgeToList(thirdNode);
        AddEdgeToList(fourthEdge); */

        AddEdgeToList(currentNode.GetNodeEdge(Direction.Right));
        AddEdgeToList(edges[^1].EndNode.GetNodeEdge(Direction.Up));
        AddEdgeToList(edges[^1].StartNode.GetNodeEdge(Direction.Left));
        AddEdgeToList(edges[^1].StartNode.GetNodeEdge(Direction.Down));
    }
}
