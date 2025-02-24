using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGridSystem.Models
{
    public class GridNodeObject<T>
    {
        /// <summary>
        /// EN: We made this class generic so that it would be more dynamic and convenient for us to put another object instead of a node or another type of node.
        /// TR: Bu sınıfı node'larımın daha dinamik olması için generic yaptım.
        /// </summary>
        private NodeGridSystem2D<GridNodeObject<T>> _grid;
        private int _x;
        private int _y;
        private T _nodeManager;

        public GridNodeObject(NodeGridSystem2D<GridNodeObject<T>> grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void SetValue(T nodeManager)
        {
            _nodeManager = nodeManager;
        }

        public T GetValue()
        {
            return _nodeManager;
        }
    }
}