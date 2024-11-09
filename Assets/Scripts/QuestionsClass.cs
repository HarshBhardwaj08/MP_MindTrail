using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class Answer
{
    [JsonProperty("option_id")]
    public int OptionId { get; set; }

    [JsonProperty("option")]
    public string Option { get; set; }
}
[System.Serializable]
public class Question
{
    [JsonProperty("question_id")]
    public int QuestionId { get; set; }

    [JsonProperty("question")]
    public string QuestionText { get; set; }

    [JsonProperty("correct_option_id")]
    public int CorrectOptionId { get; set; }

    [JsonProperty("answers")]
    public List<Answer> Answers { get; set; }
}
[System.Serializable]
public class QuestionList
{
    public List<Question> Questions { get; set; }
}