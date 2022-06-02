using RTS.Configuration;
using RTS.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// The text which is display if exist
    /// </summary>
    public TextMeshProUGUI genericText;

    /// <summary>
    /// The sprite which is display by Indicator
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The animator which is use by Indicator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Save the type of indicator if needed for retrievying
    /// </summary>
    private int typeOfIndicator;

    public int TypeOfIndicator { get => typeOfIndicator; set => typeOfIndicator = value; }

    /// <summary>
    /// Set stay to true if you need the indicator stay on screen
    /// </summary>
    private bool stay;
    public bool Stay
    {
        get => stay;
        set
        {
            stay = value;
            if (value)
                animator.SetBool("Destroy", false);
            else
                animator.SetBool("Destroy", true);
        }
    }


    #endregion

    #region Unity Callback

    #endregion

    #region Implementation

    public void Init()
    {
        genericText.text = "";
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Create a Indicator with the given sprite and animation
    /// </summary>
    /// <param name="pSprite"></param>
    /// <param name="pAnimationTriggerName"></param>
    public void AnimateSprite(Sprite pSprite, string pAnimationTriggerName)
    {
        if (pSprite != null)
            spriteRenderer.sprite = pSprite;
        animator.SetTrigger(pAnimationTriggerName);
    }

    /// <summary>
    /// Display a text with the given color
    /// </summary>
    /// <param name="pText"></param>
    /// <param name="pColor"></param>
    public void SetText(string pText, Color pColor)
    {
        genericText.text = pText;
        genericText.faceColor = pColor;
    }

    /// <summary>
    /// Triggered by animator for destroy popup
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion
}