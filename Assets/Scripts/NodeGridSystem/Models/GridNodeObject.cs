using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using UnityEngine;

namespace NodeGridSystem.Models
{
    public class GridNodeObject<T> where T : NodeManager
    {
        /// <summary>
        /// EN: We made this class generic so that it would be more dynamic and convenient for us to put another object instead of a node or another type of node.
        /// TR: Bu sınıfı node'larımın daha dinamik olması için generic yaptım.
        /// </summary>
        private NodeGridSystem2D<GridNodeObject<T>> _grid;
        private int _x;
        private int _y;
        private T _nodeManager;

        private GridNodeObject<T> _rightNodeObject;
        private GridNodeObject<T> _downNodeObject;

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

        public void InitNeighbourGridObjects()
        {
            _rightNodeObject = _grid.GetValue(_x + 1, _y);
            _downNodeObject = _grid.GetValue(_x, _y - 1);
        }
    }
}