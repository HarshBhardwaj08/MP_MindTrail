using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public float typingSpeed = 0.05f;
    public static event Action OnDialogue;
    public static event Action<QuestionsEnum> OnQuestionsTriggered;
    public List<string> dialogues;
    public Image character1;
    public Image character2;
    public QuestionsEnum question;
    private int currentDialogueIndex = 0;
    private string currentText = "";
    private bool isTyping = false;
    public static event Action DisableObject;

    [SerializeField] AudioManager audioManager;
   
    private void OnEnable()
    {
        OnDialogue?.Invoke();
    }
    public void DisableGameObject()
    {
        DisableObject?.Invoke();
        gameObject.SetActive(false);
    }
    void Start()
    {
      
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        isTyping = true;
        uiText.text = "";

        if (currentDialogueIndex % 2 == 0) // Character 1's turn
        {
            character1.gameObject.SetActive(true);
            character2.gameObject.SetActive(false);
            uiText.alignment = TextAlignmentOptions.Left;  // Align text to the left for character 1
        }
        else // Character 2's turn
        {
            character1.gameObject.SetActive(false);
            character2.gameObject.SetActive(true);
            uiText.alignment = TextAlignmentOptions.Right; // Align text to the right for character 2
        }

        // Typewriter effect
        for (int i = 0; i <= dialogues[currentDialogueIndex].Length; i++)
        {
            currentText = dialogues[currentDialogueIndex].Substring(0, i);
            uiText.text = currentText;  // Update the UI text


            audioManager.PlaySoundEffects(2);
            
            yield return new WaitForSeconds(typingSpeed);  // Control typing speed
        }

        isTyping = false;  // Typing is done
    }

    public void NextDialogue()
    {
        if (!isTyping && currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;  // Manually increment index
            StartCoroutine(ShowText());  // Show the next line of dialogue
        }
        else
        {   
            if(currentDialogueIndex >= dialogues.Count - 1)
            {
                OnQuestionsTriggered?.Invoke(question);
                currentDialogueIndex = 0;
                this.gameObject.SetActive(false);

            }
           // Debug.Log("Dialogue is Over");
        }
    }
}
