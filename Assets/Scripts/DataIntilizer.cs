using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataIntilizer : MonoBehaviour
{
    private void Awake()
    {
        // Create example data
        var questions = new List<Question>
        {
            new Question
            {
                QuestionId = 2,
                QuestionText = "Which planet is the red planet?",
                CorrectOptionId = 2,
                Answers = new List<Answer>
                {
                    new Answer { OptionId = 4, Option = "D) Venus" },
                    new Answer { OptionId = 1, Option = "A) Jupiter" },
                    new Answer { OptionId = 3, Option = "C) Earth" },
                    new Answer { OptionId = 2, Option = "B) Mars" }
                }
            },
            new Question
            {
                QuestionId = 1,
                QuestionText = "What is the capital of France?",
                CorrectOptionId = 3,
                Answers = new List<Answer>
                {
                    new Answer { OptionId = 3, Option = "C) Paris" },
                    new Answer { OptionId = 1, Option = "A) Berlin" },
                    new Answer { OptionId = 2, Option = "B) Madrid" },
                    new Answer { OptionId = 4, Option = "D) Rome" }
                }
            },
            new Question
            {
                QuestionId = 4,
                QuestionText = "Who wrote the play 'Romeo and Juliet'?",
                CorrectOptionId = 4,
                Answers = new List<Answer>
                {
                    new Answer{ OptionId = 3, Option =  "C) Charles Dickens"},
                    new Answer { OptionId = 1, Option = "A) Jane Austen" },
                    new Answer { OptionId = 2, Option = "B) Mark Twain" },
                    new Answer { OptionId = 4, Option = "D) William Shakespeare" }

                }
            },
            new Question
            {
                QuestionId = 3,
                QuestionText = "What is the largest mammal on Earth?",
                CorrectOptionId = 1,
                Answers = new List<Answer>
                {
                    new Answer{ OptionId = 4, Option =  "D) Giraffe"},
                    new Answer { OptionId = 2, Option = "B) Elephant" },
                    new Answer { OptionId = 1, Option = "A) Blue Whale" },
                    new Answer { OptionId = 3, Option = "C) Hippopotamus" }

                }
            },
             new Question
            {
                QuestionId = 5,
                QuestionText = "Which element has the chemical symbol 'O'?",
                CorrectOptionId = 1,
                Answers = new List<Answer>
                {
                    new Answer { OptionId = 4, Option = "D) Hydrogen" },
                    new Answer { OptionId = 1, Option = "A) Oxygen" },
                    new Answer { OptionId = 3, Option = "C) Silver" },
                    new Answer { OptionId = 2, Option = "B) Gold" }
                }
            },
            new Question
            {
                QuestionId = 6,
                QuestionText = "In which year did the Titanic sink?",
                CorrectOptionId = 2,
                Answers = new List<Answer>
                {
                    new Answer { OptionId = 3, Option = "C) 1920" },
                    new Answer { OptionId = 1, Option = "A) 1905" },
                    new Answer { OptionId = 2, Option = "B) 1912" },
                    new Answer { OptionId = 4, Option = "D) 1935" }
                }
            },
            new Question
            {
                QuestionId = 7,
                QuestionText = "What is the smallest prime number?",
                CorrectOptionId = 4,
                Answers = new List<Answer>
                {
                    new Answer{ OptionId = 3, Option =  "C) 1"},
                    new Answer { OptionId = 1, Option = "A) 3" },
                    new Answer { OptionId = 2, Option = "B) 5" },
                    new Answer { OptionId = 4, Option = "D) 2 "}

                }
            },
            new Question
            {
                QuestionId = 8,
                QuestionText = "Which of the following is a primary color?",
                CorrectOptionId = 3,
                Answers = new List<Answer>
                {
                    new Answer{ OptionId = 4, Option =  "D) Purple"},
                    new Answer { OptionId = 2, Option = "B) Green" },
                    new Answer { OptionId = 1, Option = "A) Yellow" },
                    new Answer { OptionId = 3, Option = "C) Red" }

                }
            }
        };

        // Sort the questions by their QuestionId in ascending order
        var sortedQuestions = questions.OrderBy(q => q.QuestionId).ToList();

        // Sort the answers within each question by their OptionId in ascending order
        foreach (var question in sortedQuestions)
        {
            question.Answers = question.Answers.OrderBy(a => a.OptionId).ToList();
        }

        var questionList = new QuestionList
        {
            Questions = sortedQuestions
        };

        // Serialize the sorted object to a JSON string
        string jsonString = JsonConvert.SerializeObject(questionList, Formatting.Indented);

        // Path to save the JSON file in the Assets folder
        string path = Application.persistentDataPath + "/SortedQuestionsData.json";

        // Write the JSON string to a file
        File.WriteAllText(path, jsonString);

        // Log success message
    //    Debug.Log("Sorted JSON file saved to: " + path);
    }
}
   
