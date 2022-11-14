using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

//���ڿ��ƾ�ͷ��
public class cameraControl : MonoBehaviour
{

    public Transform rig_Transform;
    public Transform VrCamera;
    private float RotateSpeed = 300;
    private float x_AngleLimit = 45;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        CameraRotate();
        CameraReset();
#endif
#if UNITY_ANDROID
        CameraGyroController();
#endif
    }


    public Vector3 angles;

 
    void CameraRotate()
    {
        //�����ƽǶ�
        if (rig_Transform == null || VrCamera == null) return;

        if (Input.GetMouseButton(0))
        {
            float x_Offset = Input.GetAxis("Mouse X");//����ˮƽ����ת����rig_Transform����y����ת
            float y_Offset = Input.GetAxis("Mouse Y");//���ƴ�ֱ����ת����VrCamera����x����ת

            rig_Transform.Rotate(x_Offset * Vector3.up * RotateSpeed * Time.fixedDeltaTime, Space.World);
            VrCamera.Rotate(y_Offset * Vector3.right * RotateSpeed * Time.fixedDeltaTime, Space.Self);
        }

        //�Ƕ�����
        angles = VrCamera.localEulerAngles;
        if(angles.x < 180)
        {
            if (angles.x > x_AngleLimit)
            {
                VrCamera.localRotation = Quaternion.Euler(new Vector3(x_AngleLimit, 0, 0));
            }

        }
        else
        {
            if (angles.x < 360 - x_AngleLimit)
            {
                VrCamera.localRotation = Quaternion.Euler(new Vector3(360-x_AngleLimit, 0, 0));
            }
        }

    }

    void CameraReset()
    {
        //��������Ƕ�
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            rig_Transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            VrCamera.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public Vector3 _rotationRotate;
    void CameraGyroController()
    {
        //�����ǿ��������ת
        if (rig_Transform == null || VrCamera == null) return;

        //��ȡ�����ǲ�����
        Gyroscope _gyroscope = Input.gyro;
        _gyroscope.enabled = true;

        _rotationRotate = _gyroscope.rotationRateUnbiased;

        this.transform.Rotate(-_rotationRotate.y * Vector3.down * RotateSpeed * Time.fixedDeltaTime, Space.World);
        this.transform.Rotate(-_rotationRotate.x * Vector3.right * RotateSpeed * Time.fixedDeltaTime, Space.Self);

    }
}