using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGridSystem.Controllers
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class NodeManager : MonoBehaviour
    {
        #region References
        [SerializeField] private SpriteRenderer _nodeSpriteRenderer;
        #endregion

        //TODO: I CAN ADD NODE TYPE TO ACCEPT SAME TYPE OF BLOCK DRAGGED IN THE FUTURE
        //..type, SetType(set sprite renderer), GetType
        
    }
}
