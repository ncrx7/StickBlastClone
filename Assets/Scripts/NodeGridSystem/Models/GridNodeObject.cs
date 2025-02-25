using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using UnityEngine;
using Enums;

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

        private GridNodeObject<T> _rightNodeObject;
        private GridNodeObject<T> _downNodeObject;
        private GridNodeObject<T> _upNodeObject;
        private GridNodeObject<T> _leftNodeObject;
        private Dictionary<NeighbourDirection, GridNodeObject<T>> _neighborNodes = new();

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

            _neighborNodes[NeighbourDirection.Right] = _grid.GetValue(_x + 1, _y);
            _neighborNodes[NeighbourDirection.Left] = _grid.GetValue(_x - 1, _y);
            _neighborNodes[NeighbourDirection.Up] = _downNodeObject = _grid.GetValue(_x, _y + 1);
            _neighborNodes[NeighbourDirection.Down] = _downNodeObject = _grid.GetValue(_x, _y - 1);
        }

        public GridNodeObject<T> GetNeighbourGridObject(NeighbourDirection neighbourDirection)
        {
            return _neighborNodes[neighbourDirection];
        }

        public Dictionary<NeighbourDirection, GridNodeObject<T>> GetAllNeighbourGridObjects => _neighborNodes;
        public int GetX => _x;
        public int GetY => _y;
        public NodeGridSystem2D<GridNodeObject<T>> GetGridSystem => _grid;
    }
}
