using System;
using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;

namespace NodeGridSystem.Controllers
{
    public class EdgeManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _blockShapeSprite;
        public NodeManager StartNode;
        public NodeManager EndNode;

        public bool IsEmpty = true;

        public void Setup(GridNodeObject<NodeManager> startGridNodeObject, GridNodeObject<NodeManager> endGridNodeObject, NodeGridSystem2D<GridNodeObject<NodeManager>> gridNodeSystem) //Vector2 start, Vector2 end
        {
            StartNode = startGridNodeObject.GetValue();
            EndNode = endGridNodeObject.GetValue();

            Vector2 startPos = gridNodeSystem.GetWorldPositionCenter(startGridNodeObject.GetX, startGridNodeObject.GetY);
            Vector2 endPos = gridNodeSystem.GetWorldPositionCenter(endGridNodeObject.GetX, endGridNodeObject.GetY);

            transform.position = (Vector2)(startPos + endPos) / 2;

            Vector2 direction = (endPos - startPos);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public SpriteRenderer GetBlockShapeSpriteRenderer => _blockShapeSprite;
    }
}
