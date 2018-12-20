using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
 
 /// <summary>Clase de animación de botón a partir de eventos de mouse</summary>
[RequireComponent(typeof(EventTrigger))]
public class ButtonAnimation : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    
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
    
    /// <summary>Inicialización del animador del botón con sus atributos en estado normal</summary>
    void Start() 
    {
        normalSprite = targetImage.sprite;
        normalTextColor = targetText.color;
    }
 
     /// <summary>Evento en caso de que el mouse haga hover sobre el botón</summary>
    /// <param name="eventData">Información del evento de mouse</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        inBounds = true;
        UpdateStyle();
        targetFx.PlayOneShot(hoverFx);
    }

    /// <summary>Evento en que el mouse deje de hacer hover sobre el botón</summary>
    /// <param name="eventData">Información del evento de mouse</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        inBounds = false;
        UpdateStyle();
    }
   
    /// <summary>Evento de click en el botón</summary>
    /// <param name="eventData">Información del evento de mouse</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        tracking = true;
        inBounds = true;
        UpdateStyle();
        targetFx.PlayOneShot (pressedFx);
    }
   
    /// <summary>Evento de salida del click en el botón</summary>
    /// <param name="eventData">Información del evento de mouse</param>
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
    
    /// <summary>Gestión del cambio de sprite y color de texto</summary>
    /// <param name="sprite">Sprite definida para el cambio de estado</param>
    /// <param name="textColor">Color definido para el cambio de estado</param>
    void Set(Sprite sprite, Color textColor)
    {
        targetImage.sprite = sprite;
        targetText.color = textColor;
    }

    /// <summary>Definición del cambio de estado del sprite y color de texto</summary>
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