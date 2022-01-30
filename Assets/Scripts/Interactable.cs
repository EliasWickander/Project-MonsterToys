using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject m_arrowIndicator;

    [SerializeField] 
    private Transform m_interactionPoint;

    
    private RectTransform m_rectTransform;
    public Vector3 CenterPoint
    {
        get
        {
            return m_rectTransform.position;
        }
        set
        {
            m_rectTransform.position = value;
        }
    }
    public Vector3 InteractionPoint => m_interactionPoint.position;
    
    public event Action<Interactable> OnClick; 
    
    public abstract void OnInteract();

    protected void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    protected void OnHoverEnter()
    {
        m_arrowIndicator.SetActive(true);
    }

    protected void OnHoverExit()
    {
        m_arrowIndicator.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.IsPaused)
            return;
        
        OnClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}
