using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

/*
 * �ýű����ڿ�����Ƶ�Ĳ��ţ���ͣ��
 * 
 */
public class videoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private int currentClipIndex;//��Ƶ���
    public List<string> listVideoPath = new List<string>();
    public string videoPath;
    private Vector2 DeltaArea;       //��ά��������������

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        currentClipIndex = 0;
        GetFilesAllMp4(videoPath);
        videoPlayer.url = listVideoPath[currentClipIndex];
        //��ʼ����������ֵ
        DeltaArea = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        videoPlayOrPause();
        videoChangebyurl();
#endif
#if UNITY_ANDROID
        videoControlforAndroid();
#endif
    }

    void videoPlayOrPause()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (videoPlayer.isPlaying == true)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }

    void videoChangebyurl()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentClipIndex++;
            currentClipIndex = currentClipIndex % listVideoPath.Count;
            videoPlayer.url = listVideoPath[currentClipIndex];
            videoPlayer.Pause();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentClipIndex--;
            if (currentClipIndex == -1)
            {
                currentClipIndex = listVideoPath.Count - 1;
            }
            videoPlayer.url = listVideoPath[currentClipIndex];
            videoPlayer.Pause();
        }
    }

    void videoChangebyurlforAndroid()
    {
        if (Input.touchCount==1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                currentClipIndex++;
                currentClipIndex = currentClipIndex % listVideoPath.Count;
                videoPlayer.url = listVideoPath[currentClipIndex];
            }
        }
    }

    private bool BoolSecondClick;           //�Ƿ�Ϊ�ڶ��ε��
    private float FloFirstTime = 0f;          //��һ�ε��ʱ��
    private float FloSecondTime = 0f;         //�ڶ��ε��ʱ��

    void videoControlforAndroid()
    {
        /* ��ָ�뿪��Ļ */
        //Input.touchCount�Ǿ�̬���α�������һֻ��ָ�Ӵ�����Ļʱ����1����ֻ��ָ����2���Դ����ơ�
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            DeltaArea = Vector2.zero;
            //DoubleClickTips.text = "";          //�����ָ�뿪��Ļ��˫��Ч����ʧ
        }

        /* ʶ����ָ���� */
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            DeltaArea.x += Input.GetTouch(0).deltaPosition.x;           //���ϻ�ȡ��ָ����ʱx,y��ı仯������ֵ����������
            DeltaArea.y += Input.GetTouch(0).deltaPosition.y;
            if (DeltaArea.x > 150)
            {
                //LeftRightTips.text = "�һ���";
            }
            else if (DeltaArea.x < -150)
            {
                //LeftRightTips.text = "����";
            }

            if (DeltaArea.y > 150)
            {
                //UpDownTips.text = "�ϻ���";

            }
            else if (DeltaArea.y < -150)
            {
                //UpDownTips.text = "�»���";
            }
        }


        /* ��ָ˫��ʶ��*/
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            FloSecondTime = Time.time;
            if (FloSecondTime - FloFirstTime > 0.02F && FloSecondTime - FloFirstTime < 0.3F)
            {//���ڶ��ε�����һ�ε����ʱ������0.02����0.3��֮��ʱ
                //DoubleClickTips.text = "˫������Ļ��";
                currentClipIndex++;
                currentClipIndex = currentClipIndex % listVideoPath.Count;
                videoPlayer.url = listVideoPath[currentClipIndex];
            }
            else
            {
                //DoubleClickTips.text = "��������Ļ��";
                if (videoPlayer.isPlaying == true)
                {
                    videoPlayer.Pause();
                }
                else
                {
                    videoPlayer.Play();
                }
            }
            FloFirstTime = Time.time;       //��¼ʱ��
        }
    }

    public void GetFilesAllMp4(string path_)
    {
        path_ = Path.Combine(Application.persistentDataPath, path_);
        Debug.Log(path_);
        if (Directory.Exists(path_))
        {
            DirectoryInfo direction = new DirectoryInfo(path_);
            FileInfo[] files = direction.GetFiles("*");
            Debug.Log("�ļ�����" + files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                //���Թ����ļ�
                if (files[i].Name.EndsWith(".mp4"))
                {
                    //Debug.Log("�ļ���:" + files[i].Name);
                    Debug.Log("�ļ�����·��:" + files[i].FullName);
                    listVideoPath.Add(files[i].FullName);
                    //Debug.Log("�ļ�����Ŀ¼:" + files[i].DirectoryName);
                }
            }
        }
        else
        {
            return;
        }
    }
}
