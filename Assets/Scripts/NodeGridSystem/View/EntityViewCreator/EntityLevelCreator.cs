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
        protected virtual void OnEnable()
        {
            MiniEventSystem.OnCreateEntity += CreateEntity;
            MiniEventSystem.OnCompleteGridBoardDimensionCalculating += HandleEntityScale;
        }

        protected virtual void OnDisable()
        {
            MiniEventSystem.OnCreateEntity -= CreateEntity;
            MiniEventSystem.OnCompleteGridBoardDimensionCalculating -= HandleEntityScale;
        }
        #endregion

        #region Protected Methods
        protected virtual void CreateEntity(EntityType entityType, int x, int y, NodeGridSystem2D<GridNodeObject<NodeManager>> nodeGrid, NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> middCellGrid, int entityPoolId)
        {
            //Debug.Log("Entity creating..");
        }

        protected virtual void HandleEntityScale() {} 
  
        #endregion
    }
}
