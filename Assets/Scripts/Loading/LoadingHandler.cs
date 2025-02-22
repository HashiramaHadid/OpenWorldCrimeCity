using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    public Image fillerImg;
    public Text fillerTxt;

    public AsyncOperation operation;

    private float dummyfill;
    private float speed = 5f;

    public delegate void OnLoadingComplete();
    public static OnLoadingComplete OnLoadingCompleteEvent;
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        dummyfill = 0;
        UpdateUI();

        operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
        StartCoroutine(Loading());
        Constant.UtilityData.isMenuTransition = true;
    }

    IEnumerator Loading()
    {
        Debug.LogError("Start Routine");
        while (dummyfill < 100 & !operation.isDone)
        {
            Debug.LogError("Load Routine");
            yield return new WaitForEndOfFrame();
            dummyfill += speed * Time.deltaTime;
            UpdateUI();
            if (operation.isDone)
            {
                speed = 50;
            }
        }
        dummyfill = 100;
        UpdateUI();

        yield return new WaitForSeconds(2);
        if (OnLoadingCompleteEvent != null)
        {
            OnLoadingCompleteEvent();
        }

        Destroy(gameObject);

    }

    private void UpdateUI()
    {
        fillerImg.fillAmount = dummyfill/100;
        fillerTxt.text = Mathf.FloorToInt(dummyfill)+"%";
    }
}