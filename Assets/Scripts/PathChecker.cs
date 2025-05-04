using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using Shapes;
using UnityEngine;

public static class PathChecker
{
    private static NodeManager _currentNodeManager;
    

    public static bool CheckPath(ShapeManager shapeManager, NodeManager mainNodeManager, List<EdgeManager> edgeList, bool onlyCheck)
    {
        /* if(_currentNodeManager != null && _currentNodeManager != mainNodeManager)
        {
            HideBlockShapeSlotSign(shapeManager);
            Debug.LogWarning("Exited from old node");
            
        } */
        
        HideBlockShapeSlotSign(shapeManager);
        shapeManager.GetEdgesMatching.Clear();

        _currentNodeManager = mainNodeManager;
        


        foreach (var direction in shapeManager.GetShapeData.ShapeDirections)
        {
            Debug.Log("current node: " + _currentNodeManager.gameObject.name);
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

        Debug.Log("Can Place Shape!!");
        
        ShowBlockShapeSlotSign(shapeManager);

        if (!onlyCheck) shapeManager.SetCanPlaceFlag(true);

        return true;
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
            Debug.Log("NO EDGE TO HİDE");
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
}
