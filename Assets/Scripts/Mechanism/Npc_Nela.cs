using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Nela : MonoBehaviour
{
    public GameObject dialogueManger;
    public Interactable interactable;
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
            dialogueManger.SetActive(true);
        }
    }
    private void Awake()
    {
        dialogueManger.SetActive(false);
    }
}
