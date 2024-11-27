using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
   // public static Questions question;
   public static GameManager Instance { get;private set;}
    public GameObject[] questionsArray;
    public List<GameObject>  rewards;
    public  PlayerController playerController;
    public HeroMovementScript heroMovementScript;
    [SerializeField] AudioManager audioManager;
    public Text timerText; // Assign this in the inspector
    public static float timeElapsed;
    public static bool timerStart;
    public static int hours ;
    public static int minutes;
    public static int seconds;
    public static int totalQuestionAttempted = 0;
    public GameObject gameOverPanel;
    public AudioSource musicSound;
    public static event Action<bool,QuestionsEnum> QuestionIsAlreadyAsnswered;
    private QuestionsEnum CurrentQuestion;
    private void Awake()
    {
        OffQuestions();
        

    }
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        totalQuestionAttempted = 0;
        timerStart = true;
        timeElapsed = 0f;
        UpdateTimerText();
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime; // Increment time elapsed by the time since the last frame
        UpdateTimerText();
          }
    void UpdateTimerText()
    {
        if(!timerStart) return;
         hours = Mathf.FloorToInt(timeElapsed / 3600); // Calculate hours
         minutes = Mathf.FloorToInt((timeElapsed % 3600) / 60); // Calculate minutes
         seconds = Mathf.FloorToInt(timeElapsed % 60); // Calculate seconds
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds); // Format the text
    }
    public void ActivatePlayerController()
    {
        playerController.animator.enabled = true;
        playerController.enabled = true;
        heroMovementScript.enabled = true;
    }
    public void DeactivatePlayerController()
    {
        playerController.animator.enabled = false;
       
        playerController.enabled = false;
        heroMovementScript.enabled = false;
       
    }
    private void OffQuestions()
    {
        for (int i = 0; i < questionsArray.Length; i++)
        {
            if (questionsArray[i].activeInHierarchy == false)
            {
                break;
            }
            questionsArray[i].gameObject.SetActive(false);
        }
        ActivatePlayerController();
    }

    private void OnEnable()
    {
        InteractableManager.interactableManagerInvoke += ActivateQuestions;
        QuestionIntilizers.RewardIntiliazer += GiveReward;
        QuestionIntilizers.DisableObject += ActivatePlayerController;
        DialogueManager.DisableObject += ActivatePlayerController;
        DialogueManager.OnDialogue += DeactivatePlayerController;
        DialogueManager.OnQuestionsTriggered += ActivateQuestions;
    }

    public void GiveReward(Reward reward)
    {
        switch (reward)
        { 
            case Reward.Platfomer:
                OffQuestions();
                rewards[0].gameObject.SetActive (false);
                rewards.RemoveAt(0);
                QuestionIsAlreadyAsnswered?.Invoke(true,CurrentQuestion);
                break;
            case Reward.Bridge:
                OffQuestions();
                rewards[0].gameObject.GetComponent<Animator>().SetTrigger("Activate");
                rewards.RemoveAt(0);
                QuestionIsAlreadyAsnswered?.Invoke(true, CurrentQuestion);
                //rewards[0] = rewards[1];
                break;
            case Reward.GateUnlock:
                OffQuestions();
                QuestionIsAlreadyAsnswered?.Invoke(true, CurrentQuestion);
                break;
            case Reward.ClearPasssage:
                OffQuestions();
                rewards[0].GetComponentInChildren<Explosion>().Explode();
                rewards.RemoveAt(0);
                QuestionIsAlreadyAsnswered?.Invoke(true, CurrentQuestion);
                break;
            case Reward.TelePort:
                OffQuestions();
                rewards[0].gameObject.SetActive(true);
                rewards.RemoveAt(0);
                QuestionIsAlreadyAsnswered?.Invoke(true, CurrentQuestion);
                break;
            case Reward.Elevator:
                OffQuestions();
                rewards[0].gameObject.GetComponentInChildren<Animator>().SetTrigger("Open");
                rewards.RemoveAt(0);
                QuestionIsAlreadyAsnswered?.Invoke(true, CurrentQuestion);
                break;
        }
      
        audioManager.PlaySoundEffects(1);
    }
    private void OnDisable()
    {
        InteractableManager.interactableManagerInvoke -= ActivateQuestions;
        QuestionIntilizers.RewardIntiliazer -= GiveReward;
        QuestionIntilizers.DisableObject -= ActivatePlayerController;
        DialogueManager.DisableObject -= ActivatePlayerController;
        DialogueManager.OnDialogue -= DeactivatePlayerController;
        DialogueManager.OnQuestionsTriggered -= ActivateQuestions;
    }
    public void ActivateQuestions(QuestionsEnum questions)
    {   
        CurrentQuestion = questions;
        DeactivatePlayerController();
        switch (questions)
        {
            case QuestionsEnum.Questions1:
                questionsArray[0].SetActive(true);
                break;
            case QuestionsEnum.Questions2:
                questionsArray[1].SetActive(true);
                break;
            case QuestionsEnum.Questions3:
                questionsArray[2].SetActive(true);
                break;
            case QuestionsEnum.Questions4:
                questionsArray[3].SetActive(true);
                break;
            case QuestionsEnum.Questions5:
                questionsArray[4].SetActive(true);
                break;
            case QuestionsEnum.Questions6:
                questionsArray[5].SetActive(true);
                break;
            case QuestionsEnum.Questions7:
                questionsArray[6].SetActive(true);
                break;
            case QuestionsEnum.Questions8:
                questionsArray[7].SetActive(true);
                break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
public enum QuestionsEnum
{
    Questions1, Questions2, Questions3, Questions4 , Questions5 , Questions6 , Questions7 , Questions8 
}