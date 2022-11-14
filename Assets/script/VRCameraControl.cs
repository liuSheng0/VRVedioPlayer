using UnityEngine;

using System.Collections;

//用于控制VrCamera组件

public class VRCameraControl : MonoBehaviour
{
    private Camera actCamera;
    private int maxView = 65;
    private int minView = 10;
    private float slideSpeed = 20;
    // Use this for initialization
    void Start()
    {
        actCamera = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        CameraMove();
#endif
    }
    void CameraMove()
    {
        //用于控制视野
        //获取虚拟按键(鼠标中轴滚轮)
        float mouseCenter = Input.GetAxis("Mouse ScrollWheel");

        //鼠标滑动中键滚轮,实现摄像机的镜头放大和缩放
        //mouseCenter < 0 = 负数 往后滑动,缩放镜头
        if (mouseCenter < 0)
        {
            //滑动限制
            if (actCamera.fieldOfView <= maxView)
            {
                actCamera.fieldOfView += 10 * slideSpeed * Time.deltaTime;
                if (actCamera.fieldOfView >= maxView)
                {
                    actCamera.fieldOfView = maxView;
                }
            }
        }
        //mouseCenter >0 = 正数 往前滑动,放大镜头
        else if (mouseCenter > 0)
        {
            //滑动限制
            if (actCamera.fieldOfView >= minView)
            {
                actCamera.fieldOfView -= 10 * slideSpeed * Time.deltaTime;
                if (actCamera.fieldOfView <= minView)
                {
                    actCamera.fieldOfView = minView;
                }
            }
        }
    }
}
