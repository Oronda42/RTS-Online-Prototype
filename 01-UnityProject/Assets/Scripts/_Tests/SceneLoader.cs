using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public Image fallofImage, progressBar;
    public TextMeshProUGUI loadingText;

    float fallofDuration = 0.02f;


    public void Init(string pScene)
    {

        if (SceneManager.GetActiveScene().name == Constants.Scenes.PlayerMap.FIRST_CONNECTION)
            SceneManager.UnloadSceneAsync(Constants.Scenes.PlayerMap.FIRST_CONNECTION);

        StartCoroutine(LoadLevelAsync(pScene));
    }

    public IEnumerator LoadLevelAsync(string pScene)
    {

        progressBar.fillAmount = 0;
        loadingText.text = "0 %";


        AsyncOperation op = SceneManager.LoadSceneAsync(pScene);
        op.allowSceneActivation = false;
        op.completed += OnFirstSceneLoaded;

        //REAL LOADING
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = 1;
                loadingText.text = "Press any key to continue";

                if (Input.anyKey)
                {
                    op.allowSceneActivation = true;
                    break;
                }
            }

            //FAKE LOADING
            while (fallofImage.color.a < 1)
            {
                progressBar.fillAmount += fallofDuration;
                loadingText.text = Mathf.RoundToInt((progressBar.fillAmount * 100)).ToString() + " %";

                float alpha = fallofImage.color.a;
                alpha += fallofDuration;
                fallofImage.color = new Color(1, 1, 1, alpha);
                yield return new WaitForSeconds(0.01f);
            }

            yield return null;
        }

    }

    public void OnFirstSceneLoaded(AsyncOperation op)
    {
        Debug.Log("OnFirstSceneLoaded");
    }

}

