using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Quit();
        }
    }

    public void Quit()
    {
        //���ʱ����ʹ��
        //UnityEditor.EditorApplication.isPlaying = false;
        //����ʱ����ִ�У���������ִ��
        Application.Quit();
    }
}
