using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

/*
 * 该脚本用于控制视频的播放，暂停等
 * 
 */
public class videoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private int currentClipIndex;//视频编号
    public List<string> listVideoPath = new List<string>();
    public string videoPath;
    private Vector2 DeltaArea;       //二维向量，滑屏区域

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        currentClipIndex = 0;
        GetFilesAllMp4(videoPath);
        videoPlayer.url = listVideoPath[currentClipIndex];
        //初始化，测试数值
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

    private bool BoolSecondClick;           //是否为第二次点击
    private float FloFirstTime = 0f;          //第一次点击时间
    private float FloSecondTime = 0f;         //第二次点击时间

    void videoControlforAndroid()
    {
        /* 手指离开屏幕 */
        //Input.touchCount是静态整形变量，当一只手指接触到屏幕时返回1，二只手指返回2，以此类推。
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            DeltaArea = Vector2.zero;
            //DoubleClickTips.text = "";          //如果手指离开屏幕，双击效果消失
        }

        /* 识别手指滑屏 */
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            DeltaArea.x += Input.GetTouch(0).deltaPosition.x;           //不断获取手指触屏时x,y轴的变化量并赋值给滑屏区域
            DeltaArea.y += Input.GetTouch(0).deltaPosition.y;
            if (DeltaArea.x > 150)
            {
                //LeftRightTips.text = "右滑屏";
            }
            else if (DeltaArea.x < -150)
            {
                //LeftRightTips.text = "左滑屏";
            }

            if (DeltaArea.y > 150)
            {
                //UpDownTips.text = "上滑屏";

            }
            else if (DeltaArea.y < -150)
            {
                //UpDownTips.text = "下滑屏";
            }
        }


        /* 手指双击识别*/
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            FloSecondTime = Time.time;
            if (FloSecondTime - FloFirstTime > 0.02F && FloSecondTime - FloFirstTime < 0.3F)
            {//当第二次点击与第一次点击的时间间隔在0.02秒至0.3秒之间时
                //DoubleClickTips.text = "双击了屏幕！";
                currentClipIndex++;
                currentClipIndex = currentClipIndex % listVideoPath.Count;
                videoPlayer.url = listVideoPath[currentClipIndex];
            }
            else
            {
                //DoubleClickTips.text = "单击了屏幕！";
                if (videoPlayer.isPlaying == true)
                {
                    videoPlayer.Pause();
                }
                else
                {
                    videoPlayer.Play();
                }
            }
            FloFirstTime = Time.time;       //记录时间
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
            Debug.Log("文件数量" + files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                //忽略关联文件
                if (files[i].Name.EndsWith(".mp4"))
                {
                    //Debug.Log("文件名:" + files[i].Name);
                    Debug.Log("文件绝对路径:" + files[i].FullName);
                    listVideoPath.Add(files[i].FullName);
                    //Debug.Log("文件所在目录:" + files[i].DirectoryName);
                }
            }
        }
        else
        {
            return;
        }
    }
}
