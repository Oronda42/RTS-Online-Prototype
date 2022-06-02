using System.Collections.Generic;
using UnityEngine;

#region Delegates

/// <summary>
/// Delegate for the action to be performed
/// </summary>
/// <param name="pGameObject"></param>
public delegate void InteractionActionDelegate(params object[] pParameters);

/// <summary>
/// Delegate for the condition to be performed
/// </summary>
/// <param name="pParameters"></param>
/// <returns></returns>
public delegate bool InteractionConditionDelegate(params object[] pParameters);


#endregion

public class InteractionAction
{
    #region Properties

    /// <summary>
    /// Name of the action
    /// </summary>
    private string name;
   
    /// <summary>
    /// Position of the interaction
    /// </summary>
    private int position;

    /// <summary>
    /// Delegate to the action
    /// </summary>
    private InteractionActionDelegate actionToPerform;

    /// <summary>
    /// Delegate for conditions
    /// </summary>
    private InteractionConditionDelegate condition;


    /// <summary>
    /// Parameters of the interaction
    /// </summary>
    private object[] parameters;

    /// <summary>
    /// List of sub interactions
    /// </summary>
    private List<InteractionAction> interactionSubList;

    /// <summary>
    /// Path to the icon into the asset bundle
    /// </summary>
    private string iconAssetPath;

    /// <summary>
    /// sprite to be displayed as icon
    /// </summary>
    private Sprite spriteReference;

    #endregion

    #region Constructor

    public InteractionAction(string pName, int pPosition, string pIcon,InteractionConditionDelegate pCondition, InteractionActionDelegate pAction, params object[] pParameters)
    {
        name = pName;
        
        position = pPosition;
        condition = pCondition;
        actionToPerform = pAction;
        iconAssetPath = pIcon;
        parameters = pParameters;
    }

    public InteractionAction(string pName, int pPosition, Sprite pSprite, InteractionConditionDelegate pCondition, InteractionActionDelegate pAction, params object[] pParameters)
    {
        name = pName;
        position = pPosition;
        condition = pCondition;
        actionToPerform = pAction;
        spriteReference = pSprite;
        parameters = pParameters;
    }

    public InteractionAction(string pName, int pPosition, string pIcon, InteractionConditionDelegate pCondition, List<InteractionAction> pInteractionSubList, object incrementLevelInteraction)
    {
        name = pName;
        position = pPosition;
        iconAssetPath = pIcon;
        condition = pCondition;
        interactionSubList = pInteractionSubList;
    }

    #endregion

    #region Implementation

    /// <summary>
    /// Interact with the current action
    /// </summary>
    public void Interact()
    { 
        if (actionToPerform != null)
        {
            actionToPerform(parameters);
        }
    }

    /// <summary>
    /// Interact with the current action
    /// </summary>
    public bool CheckAvailabilty()
    {
        if (condition != null)
            return condition(parameters);
        else return false;

    }

    /// <summary>
    /// Returns the interaction list
    /// </summary>
    public List<InteractionAction> GetSubInteractions()
    {
        return interactionSubList;
    }

    /// <summary>
    /// Returns the position of the action
    /// </summary>
    /// <returns></returns>
    public int GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Returns the name of the interaction
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return name;
    }


    /// <summary>
    /// Returns the icon asset path
    /// </summary>
    /// <returns></returns>
    public string GetIconAssetPath()
    {
        return iconAssetPath;
    }

    /// <summary>
    /// Returns the sprite of the interaction
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {
        return spriteReference;
    }

    public static bool AllowInterraction(params object[] pParameters)
    {
        return true;
    }

    #endregion
}
