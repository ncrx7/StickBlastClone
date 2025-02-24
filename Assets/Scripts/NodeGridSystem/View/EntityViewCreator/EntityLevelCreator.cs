using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;

namespace NodeGridSystem.View
{
    public class EntityLevelCreator<T> : MonoBehaviour
    {
        #region References
        [SerializeField] protected T _entityPrefab;
        #endregion

        #region MonoBehaviour Callbacks
        private void OnEnable()
        {
            MiniEventSystem.OnCreateNode += CreateEntity;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnCreateNode -= CreateEntity;
        }
        #endregion

        #region Private Methods
        protected virtual void CreateEntity(int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> grid, int entityPoolId)
        {
            Debug.Log("Entity creating..");
        }
        #endregion
    }
}
