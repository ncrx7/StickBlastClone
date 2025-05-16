using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NodeGridSystem.Controllers;
using Shapes;
using UnityEngine;
using Zenject;

public static class PathChecker
{
    private static NodeManager _currentNodeManager;

    public static void HandleCheckShapePathByPointer(NodeGridBoardManager nodeGridBoardManager, ShapeManager shapeManager, ref Vector2Int lastMousePositionOnBoard)
    {
        var closestNode = FindClosestNeighbourByShapeHead(nodeGridBoardManager, shapeManager, ref lastMousePositionOnBoard);

        if (closestNode == null)
            return;

        PathChecker.CheckPathFromANode(shapeManager, closestNode, shapeManager.GetEdgesMatching, false);
    }

    private static NodeManager FindClosestNeighbourByShapeHead(NodeGridBoardManager nodeGridBoardManager, ShapeManager shapeManager, ref Vector2Int lastMousePositionOnBoard)
    {
        // Vector2 pointerWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);

        Vector2Int closestNodeCordinate = nodeGridBoardManager.GetNodeGridSystem2D.GetXY(shapeManager.GetHead.transform.position);


        if (closestNodeCordinate == lastMousePositionOnBoard)
        {
            return null;
        }

        //Debug.Log("closest cordinate -> " + closestNodeCordinate.x + " - " + closestNodeCordinate.y);

        NodeManager closestNode = nodeGridBoardManager.GetNodeGridSystem2D.GetValue(closestNodeCordinate.x, closestNodeCordinate.y)?.GetValue();

        if (closestNode == null)
            return null;

        //_shapeManager.GetEdgesMatching.Clear();

        lastMousePositionOnBoard = closestNodeCordinate;
        return closestNode;
    }

    public static bool CheckPathFromANode(ShapeManager shapeManager, NodeManager mainNodeManager, List<EdgeManager> edgeList, bool onlyCheck)
    {
        /* if(_currentNodeManager != null && _currentNodeManager != mainNodeManager)
        {
            HideBlockShapeSlotSign(shapeManager);
            Debug.LogWarning("Exited from old node");
            
        } */

        if(!onlyCheck) HideBlockShapeSlotSign(shapeManager);
        shapeManager.GetEdgesMatching.Clear();

        _currentNodeManager = mainNodeManager;



        foreach (var direction in shapeManager.GetShapeData.ShapeDirections)
        {
            //Debug.Log("current node: " + _currentNodeManager.gameObject.name);
            EdgeManager currentEdge = _currentNodeManager.GetNodeEdge(direction);

            if (currentEdge == null)
            {
                edgeList.Clear();
                shapeManager.SetCanPlaceFlag(false);
                return false;
            }

            if (!onlyCheck) edgeList.Add(currentEdge);

            if (currentEdge.IsEmpty == false)
            {
                edgeList.Clear();
                //Debug.LogError("Edge is not empty, cant place!!" + currentEdge.gameObject.name);
                shapeManager.SetCanPlaceFlag(false);
                return false;
            }

            _currentNodeManager = _currentNodeManager.OnGridNodeObject.GetNeighbourGridObject(direction).GetValue();
        }

        //Debug.Log("Can Place Shape!!");

        if (!onlyCheck)
        {
            ShowBlockShapeSlotSign(shapeManager);
            shapeManager.SetCanPlaceFlag(true);
        }

        return true;
    }

    public async static UniTask<EmptyDirectionPathResponse> EmptyDirectionPathOnBoardChecker(NodeGridBoardManager nodeGridBoardManager, ShapeManager shapeManager)
    {
        bool isThereEmptySlot = false;
        int matchCount = 0;

        for (int x = 0; x < nodeGridBoardManager.GetWidth; x++)
        {
            for (int y = 0; y < nodeGridBoardManager.GetHeight; y++)
            {
                var gridNodeObject = nodeGridBoardManager.GetNodeGridSystem2D.GetValue(x, y);

                NodeManager nodeManager = gridNodeObject.GetValue();

                if (CheckPathFromANode(shapeManager, nodeManager, shapeManager.GetEdgesMatching, true)) 
                {
                    matchCount++;

                    if (matchCount == 1) 
                    {
                        isThereEmptySlot =  true;
                        continue;
                    }
                    else
                    {
                        await UniTask.DelayFrame(1);
                        return new EmptyDirectionPathResponse(isThereEmptySlot, true); //Eger birden fazla eşleşme olursa  2. PARAMETRE true doner
                    }
                }

            }
        }

        await UniTask.DelayFrame(1);

        return new EmptyDirectionPathResponse(isThereEmptySlot, false);;
    }

    private static void ShowBlockShapeSlotSign(ShapeManager shapeManager)
    {
        if (shapeManager.GetEdgesMatching.Count == 0)
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

    private static void HideBlockShapeSlotSign(ShapeManager shapeManager)
    {
        if (shapeManager.GetEdgesMatching.Count == 0)
        {
            //Debug.Log("NO EDGE TO HIDE");
            return;
        }

        foreach (var edge in shapeManager.GetEdgesMatching)
        {
            Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
            targetDisplayColor.a = 0f;
            edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;

            if (!edge.StartNode.NodePainted) edge.StartNode.ResetNode();
            if (!edge.EndNode.NodePainted) edge.EndNode.ResetNode();
        }
    }

    public class EmptyDirectionPathResponse
    {
        public bool IsThereEmptySlot;
        public bool IsSlotCountUpperThanOne;

        public EmptyDirectionPathResponse(bool isThereEmptySlot, bool isSlotCountUpperOne) 
        {
            IsThereEmptySlot = isThereEmptySlot;
            IsSlotCountUpperThanOne = isSlotCountUpperOne;
        }
    }
}
