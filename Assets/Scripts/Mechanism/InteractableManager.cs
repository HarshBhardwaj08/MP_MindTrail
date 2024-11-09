using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public delegate void InteractableMangerHandler(QuestionsEnum question);
    public static event InteractableMangerHandler interactableManagerInvoke;
    public delegate void InteractingObjects(Interactable name);
    public static event InteractingObjects interactingObjects;
    private void OnEnable()
    {
        Interactables.interactableInvoke += InvokeInteractable;
    }
    private void OnDisable()
    {
        Interactables.interactableInvoke -= InvokeInteractable;
    }
    public void InvokeInteractable(Interactable interactable, QuestionsEnum question)
    {
        switch (interactable)
        {
            case Interactable.crank:
                interactableManagerInvoke.Invoke(question);
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Npc_Nela:
               // interactableManagerInvoke.Invoke(question);
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Bridge:
               // interactableManagerInvoke.Invoke(question);
                break;
            case Interactable.Face_1:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_2:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_3:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_4:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_5:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_6:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_7:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_8:
                interactingObjects.Invoke(interactable);
                break;
            case Interactable.Face_9:
                break;
            case Interactable.Face_10:
                break;
                default:
                return;
        }
    }
}
