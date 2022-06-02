using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void ButtonClick()
    {
        StartCoroutine(ApplicationManager.instance.WaitingSceneLoading(Constants.Scenes.PlayerMap.PLAYERMAP));
    }
}
