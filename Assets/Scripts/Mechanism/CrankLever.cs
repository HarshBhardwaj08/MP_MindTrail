using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrankLever : MonoBehaviour
{
    public GameObject leverDown;
    public Interactable interactable;
    private void Awake()
    {
        leverDown.SetActive(false);
    }
    private void OnEnable()
    {
        InteractableManager.interactingObjects += InvokeInteractable;
    }
    private void OnDisable()
    {
        InteractableManager.interactingObjects -= InvokeInteractable;
    }
    private void InvokeInteractable(Interactable name)
    {
        if (name == interactable)
        {
            leverDown.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
       
    }

}
