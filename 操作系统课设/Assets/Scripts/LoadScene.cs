using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject loadPanel;
    public Text InputName;
    public Text proText;
    public Scrollbar process;
    public GameObject butEnter;

    void Start()
    {
        loadPanel.SetActive(false);
    }

    void Update()
    {
        if (InputName.text.Length > 0)
        {
            butEnter.SetActive(true);
        }
        else
        {
            butEnter.SetActive(false);
        }
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    LoadNewScene();
        //}
      
    }

    public void LoadNewScene()
    {
        PlayerPrefs.SetString("人物名字",InputName.text);
        StartCoroutine(StartLoading("Test"));//调用协程
        loadPanel.SetActive(true);
    }

    private void SetLoadingPercentage(int displayProgress)
    {
        process.size = displayProgress * 0.01f;
        proText.text = displayProgress.ToString() + "%";
    }

    private IEnumerator StartLoading(string sceneName)
    {
        int displayProcess = 0;
        int toProcess = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);//异步加载

        op.allowSceneActivation = false;//禁止加载完毕后自动切换场景
        while (op.progress < 0.9f)
        {
            toProcess = (int) op.progress * 100;
            while (displayProcess < toProcess)
            {
                ++displayProcess;
                SetLoadingPercentage(displayProcess);
                yield return  new WaitForEndOfFrame();
            }
        }

        toProcess = 100;
        while (displayProcess < toProcess)
        {
            ++displayProcess;
            SetLoadingPercentage(displayProcess);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;

    }

}
