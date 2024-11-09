using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class DataRetriver : MonoBehaviour
{
    private List<Question> sortedQuestions;
    private void Start()
    {
        string path = Application.persistentDataPath + "/SortedQuestionsData.json";

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            var questionList = JsonConvert.DeserializeObject<QuestionList>(jsonString);
            sortedQuestions = questionList.Questions
                .OrderBy(q => q.QuestionId)
                .ToList();


            foreach (var question in sortedQuestions)
            {
                question.Answers = question.Answers
                    .OrderBy(a => a.OptionId)
                    .ToList();
            }


            foreach (var question in sortedQuestions)
            {
                // Debug.Log($"Question ID: {question.QuestionId}, Question: {question.QuestionText}");
                foreach (var answer in question.Answers)
                {
                    //   Debug.Log($"  Option ID: {answer.OptionId}, Answer: {answer.Option}");
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }

    public List<Question> GetSortedQuestions()
    {
        return sortedQuestions;
    }
}
    
       

