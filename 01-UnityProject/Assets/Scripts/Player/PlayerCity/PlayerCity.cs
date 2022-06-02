using RTS.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCity : MonoBehaviour 
{
   public PlayerCityModel playerCityModel;

    public PlayerCityUI playerCityUI;

   
    public void Init()
    {
        playerCityUI = GetComponent<PlayerCityUI>();
        playerCityUI.Init(this);
    }

   
    private void OnMouseDown()
    {
        Debug.Log("City clicked, ONMOUSEDOWN");
    }
}
   
    
    

