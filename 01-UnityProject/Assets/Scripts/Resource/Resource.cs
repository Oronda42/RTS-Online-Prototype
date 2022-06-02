using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "Game Resource", menuName = "RTS/Resource/01 - New resource", order = 1)]
public class Resource : ScriptableObject
{
    #region Properties

    /// <summary>
    /// Data of the resource
    /// </summary>
    public ResourceModel model;

    /// <summary>
    /// Renderer of the resource
    /// </summary>
    public Sprite sprite;


    #endregion
}
