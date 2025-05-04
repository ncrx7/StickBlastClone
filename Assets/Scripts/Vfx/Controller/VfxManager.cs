using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityUtils.Core.VfxSystem;
using Vfx.Models;
using Zenject;

namespace Vfx.Controllers
{
    public class VfxManager : MonoBehaviour
    {
        private VfxFactory<VfxType> _vfxFactory;

        [Inject]
        private void InitializeDependencies(VfxFactory<VfxType> vfxFactory)
        {
            _vfxFactory = vfxFactory;
        }

        private void OnEnable()
        {
            MiniEventSystem.PlayVfx += HandlePlayVfx;
        }

        private void OnDisable()
        {
            MiniEventSystem.PlayVfx -= HandlePlayVfx;
        }

        private void HandlePlayVfx(Vector2 targetPosition, VfxType vfxType)
        {
            _vfxFactory.Create(vfxType, targetPosition, 0.75f, 200);
        }
    }
}
