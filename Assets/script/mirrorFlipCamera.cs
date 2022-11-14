using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorFlipCamera : MonoBehaviour
{
    //用于控制镜头镜像翻转
    new Camera camera;
    public bool flipHorizontal;//打勾即进行翻转
    void Awake()
    {
        camera = GetComponent<Camera>();
    }
    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(flipHorizontal ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        GL.invertCulling = flipHorizontal;
    }


    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
