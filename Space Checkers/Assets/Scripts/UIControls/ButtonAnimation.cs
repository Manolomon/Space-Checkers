using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
 
[RequireComponent(typeof(EventTrigger))]
public class ButtonAnimation : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    
    public Image targetImage;
    public Text targetText;
    public AudioSource targetFx;
 
    public Sprite hoverSprite;
    public Color hoverTextColor;
    public AudioClip hoverFx;
    public Sprite pressedSprite;
    public Color pressedTextColor;
    public AudioClip pressedFx;
 
    public UnityEvent OnClick;
    
    Sprite normalSprite;
    Color normalTextColor;
    bool tracking;
    bool inBounds;
    
    void Start() 
    {
        normalSprite = targetImage.sprite;
        normalTextColor = targetText.color;
    }
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        inBounds = true;
        UpdateStyle();
        targetFx.PlayOneShot(hoverFx);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        inBounds = false;
        UpdateStyle();
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        tracking = true;
        inBounds = true;
        UpdateStyle();
        targetFx.PlayOneShot (pressedFx);
    }
   
    public void OnPointerUp(PointerEventData eventData)
    {
        if (tracking && inBounds && OnClick != null)
        {
            OnClick.Invoke();
        }
        tracking = false;
        inBounds = false;
        UpdateStyle();
    }
    
    void Set(Sprite sprite, Color textColor)
    {
        targetImage.sprite = sprite;
        targetText.color = textColor;
    }

    void UpdateStyle()
    {
        if (!inBounds)
        {
            Set(normalSprite, normalTextColor);
        } else if (tracking)
        {
            Set(pressedSprite, pressedTextColor);
        } else
        {
            Set(hoverSprite, hoverTextColor);
        }
    }
    
}