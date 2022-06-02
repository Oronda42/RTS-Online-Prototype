using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapInteractable {

    #region Implementation

    /// <summary>
    /// Returns the name of the interactable element
    /// </summary>
    /// <returns></returns>
    string GetName();

    /// <summary>
    /// Returns the description
    /// </summary>
    /// <returns></returns>
    string GetDescription();

    /// <summary>
    /// Returns the position on the player map
    /// </summary>
    /// <returns></returns>
    Vector3 GetMapPosition();

    /// <summary>
    /// Returns all availables interactions
    /// </summary>
    /// <returns></returns>
    List<InteractionAction> GetInteractionsAction();

    #endregion
}
