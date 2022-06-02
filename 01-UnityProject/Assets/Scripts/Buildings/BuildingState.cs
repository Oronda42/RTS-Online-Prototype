using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "BuildingState", menuName = "RTS/Buildings/00 - New Building State", order = 0)]
public class BuildingState : ScriptableObject
{
    #region Properties

    public BuildingStateModel model;

    public Sprite icon;

    public Color color;

    #endregion
}
