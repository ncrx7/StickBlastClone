using System.Collections;
using System.Collections.Generic;
using NodeGridSystem.Controllers;
using NodeGridSystem.Models;
using UnityEngine;
using Enums;

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
            MiniEventSystem.OnCreateEntity += CreateEntity;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnCreateEntity -= CreateEntity;
        }
        #endregion

        #region Private Methods
        protected virtual void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> middCellGrid, int entityPoolId)
        {
            Debug.Log("Entity creating..");
        }
        #endregion
    }
}
