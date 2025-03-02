using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyUtils.Base;
using UnityEngine;

public class CameraManager : SingletonBehavior<CameraManager>
{
    [SerializeField] private Camera _camera;
    private float _defaultSize;

    private void Start()
    {
        _camera = Camera.main;
        _defaultSize = _camera.orthographicSize;
    }

    public void TestButtonEvent()
    {
        ZoomInAndOut(5.2f, 0.45f, 0.2f, 8);
    }

    public void ZoomInAndOut(float zoomSize = 4f, float duration = 1f, float shakeStrength = 0.2f, int vibrato = 10)
    {
       

        _camera.DOOrthoSize(zoomSize, duration)
        .OnStart(() =>
        {
            _camera.transform.DOShakePosition(duration * 0.5f, shakeStrength, vibrato);
        })
            .OnComplete(() =>
            {
    
                _camera.DOOrthoSize(_defaultSize, duration);
            });
    }

}
