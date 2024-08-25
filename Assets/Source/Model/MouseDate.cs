using UnityEngine;

public class MouseDate : IMousePosition
{
    private readonly Camera _mainCamera;

    public Vector3 mouseToWorldPosition => _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));
    
    public MouseDate(Camera camera)
    {
        _mainCamera = camera;
    }
}