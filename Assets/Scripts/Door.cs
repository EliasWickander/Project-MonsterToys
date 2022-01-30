using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : Interactable
{
    [SerializeField] 
    private Scenes m_sceneToLoad;

    public Scenes SceneToLoad => m_sceneToLoad;

    [SerializeField] 
    private Transform m_normal;

    public Vector3 Normal => m_normal.right;
    
    
    public override void OnInteract()
    {
        LevelManager.Instance.ChangeScene(m_sceneToLoad);
    }
}
