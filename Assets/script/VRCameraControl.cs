using UnityEngine;

using System.Collections;

//���ڿ���VrCamera���

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
        //���ڿ�����Ұ
        //��ȡ���ⰴ��(����������)
        float mouseCenter = Input.GetAxis("Mouse ScrollWheel");

        //��껬���м�����,ʵ��������ľ�ͷ�Ŵ������
        //mouseCenter < 0 = ���� ���󻬶�,���ž�ͷ
        if (mouseCenter < 0)
        {
            //��������
            if (actCamera.fieldOfView <= maxView)
            {
                actCamera.fieldOfView += 10 * slideSpeed * Time.deltaTime;
                if (actCamera.fieldOfView >= maxView)
                {
                    actCamera.fieldOfView = maxView;
                }
            }
        }
        //mouseCenter >0 = ���� ��ǰ����,�Ŵ�ͷ
        else if (mouseCenter > 0)
        {
            //��������
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
