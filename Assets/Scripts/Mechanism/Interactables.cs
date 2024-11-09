using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public Interactable interactable;
    public QuestionsEnum question;
    private bool isQuestionIsAlreadyAsnswered;
    public delegate void InteractableHandler(Interactable interactable, QuestionsEnum question);
    public static event InteractableHandler interactableInvoke;
    public static event Action<int> OnPlayerInteraction;
    private QuestionsEnum currentQuestion;
    // private bool isPlayerNearby = false;

    private void Update()
    {
      //  if (isPlayerNearby)
      //  {
           // if (Input.GetKeyDown(KeyCode.E))
            //{
           //     OnPlayerInteraction?.Invoke(6);
           //     interactableInvoke.Invoke(interactable, question);
          //  }
      //  }
      
    }
    private void OnEnable()
    {
        GameManager.QuestionIsAlreadyAsnswered += SetisQuestionIsAlreadyAsnswered;
    }

    private void SetisQuestionIsAlreadyAsnswered(bool obj,QuestionsEnum questionsEnum)
    {
        isQuestionIsAlreadyAsnswered = obj;
        currentQuestion = questionsEnum;
    }

    private void OnDisable()
    {
        GameManager.QuestionIsAlreadyAsnswered -= SetisQuestionIsAlreadyAsnswered;
    }
    // When the player enters the interaction zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           // isPlayerNearby = true;
         if(isQuestionIsAlreadyAsnswered == true && currentQuestion == question)
            {
              //  Debug.Log("Question Already Answerd " + question);
                return;
            }
            OnPlayerInteraction?.Invoke(6);
            interactableInvoke.Invoke(interactable, question);
        }
    }

    // While the player is in the interaction zone, check for input
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("Player is inside the lever.");
    //    }
    //    if (isPlayerNearby && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
    //    {
    //        interactableInvoke.Invoke(interactable, question);
    //    }
    //}

    // When the player leaves the interaction zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
         //   isPlayerNearby = false;
           // Debug.Log("Player left the lever.");
        }
    }
}

public enum Interactable
{  
    crank,
    Bridge,
    Npc_Nela,
    Face_1,
    Face_2,
    Face_3,
    Face_4,
    Face_5,
    Face_6,
    Face_7,
    Face_8,
    Face_9,
    Face_10,
}