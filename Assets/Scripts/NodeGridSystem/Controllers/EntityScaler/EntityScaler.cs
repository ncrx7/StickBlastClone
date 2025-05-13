using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGridSystem.Controllers.EntityScalers
{
    public class EntityScaler
    {
        public Vector2 NodeTargetScale {get; set;}

        public Vector2 EdgeTargetScale { get; set; }

        public Vector2 MidCellTargetScaleOnInitial { get; set; }
        public Vector2 MidCellTargetScaleOnFilled { get; set; }


        public void CalculateNodeScaleFactor(NodeManager node, NodeGridBoardManager nodeGridBoardManager)
        {
            float spriteUnitSize = node.GetSpriteRenderer.sprite.bounds.size.x;
            float NodeScaleFactor = (nodeGridBoardManager.AutomaticBoardCellSize / 2) / spriteUnitSize;
            NodeTargetScale = Vector3.one * NodeScaleFactor;
        }

        public void CalculateEdgeScaleFactor(EdgeManager edge, NodeGridBoardManager nodeGridBoardManager)
        {
            float spriteUnitSizeX = edge.GetEdgeSprite.sprite.bounds.size.x;
            float spriteUnitSizeY = edge.GetEdgeSprite.sprite.bounds.size.y;

            float edgeScaleFactorX = nodeGridBoardManager.AutomaticBoardCellSize / spriteUnitSizeX;
            float edgeScaleFactorY = (nodeGridBoardManager.AutomaticBoardCellSize / 4) / spriteUnitSizeY;

            EdgeTargetScale = new Vector2(edgeScaleFactorX, edgeScaleFactorY);
        }

        public void CalculateMidCellScaleFactor(MiddleFillAreaManager midCell, NodeGridBoardManager nodeGridBoardManager)
        {
            float spriteUnitSize = midCell.GetSpriteRenderer.sprite.bounds.size.x;

            float initialScaleFactor = (nodeGridBoardManager.AutomaticBoardCellSize / 4) / spriteUnitSize;
            float scaleFactorOnFilled = (nodeGridBoardManager.AutomaticBoardCellSize) / spriteUnitSize;

            MidCellTargetScaleOnInitial = Vector2.one * initialScaleFactor;
            MidCellTargetScaleOnFilled = Vector2.one * scaleFactorOnFilled; 
        }
    }
}
