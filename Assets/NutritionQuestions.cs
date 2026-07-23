using System.Collections.Generic;
using UnityEngine;

public class NutritionQuestions : MonoBehaviour
{
    public class Question
    {
        public string questionText;
        public List<string> options;

        public Question(string questionText, List<string> options)
        {
            this.questionText = questionText;
            this.options = options;
        }
    }

    

    public List<Question> categoryOneQuestions = new List<Question>
    {
        new Question(
            "How often do you consume animal products (meat, dairy, eggs)?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you include plant-based protein sources (e.g., beans, lentils, tofu) in your diet?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you eat whole grains (e.g., brown rice, whole wheat bread)?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you consume fruits and vegetables?",
            new List<string> { "A) Every meal", "B) Daily", "C) Few times a week", "D) Rarely" }
        ),
        new Question(
            "How often do you consume sugary foods and drinks?",
            new List<string> { "A) Rarely", "B) Occasionally", "C) Daily", "D) Multiple times a day" }
        ),
        new Question(
            "Do you follow any specific diet (e.g., keto, vegan, low-carb)?",
            new List<string> { "A) Yes, strictly", "B) Yes, loosely", "C) No, but I try to eat healthy", "D) No, I eat whatever I want" }
        ),
        new Question(
            "How often do you eat fast food or processed meals?",
            new List<string> { "A) Rarely", "B) Once a week", "C) Few times a week", "D) Daily" }
        ),
        new Question(
            "How often do you eat organic foods?",
            new List<string> { "A) Regularly", "B) Occasionally", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you consume nuts and seeds?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you cook meals at home?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Occasionally", "D) Rarely" }
        )
    };

    public List<Question> categoryTwoQuestions = new List<Question>
    {
        new Question(
            "What is your primary health goal?",
            new List<string> { "A) Weight loss", "B) Muscle gain", "C) Maintain current weight", "D) Improve overall health" }
        ),
        new Question(
            "How often do you engage in physical activity?",
            new List<string> { "A) Daily", "B) 3-5 times a week", "C) 1-2 times a week", "D) Rarely or never" }
        ),
        new Question(
            "How much water do you typically drink per day?",
            new List<string> { "A) More than 8 glasses", "B) 5-7 glasses", "C) 3-4 glasses", "D) Less than 3 glasses" }
        ),
        new Question(
            "How many hours of sleep do you get per night?",
            new List<string> { "A) 7-8 hours", "B) 5-6 hours", "C) Less than 5 hours", "D) More than 8 hours" }
        ),
        new Question(
            "How do you rate your stress levels?",
            new List<string> { "A) Low", "B) Moderate", "C) High", "D) Very high" }
        ),
        new Question(
            "How often do you monitor your calorie intake?",
            new List<string> { "A) Daily", "B) Occasionally", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you eat home-cooked meals?",
            new List<string> { "A) Daily", "B) Several times a week", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you get a health check-up or medical examination?",
            new List<string> { "A) Regularly", "B) Occasionally", "C) Rarely", "D) Only when necessary" }
        ),
        new Question(
            "How often do you eat meals with a focus on balanced nutrition (protein, carbs, fats)?",
            new List<string> { "A) Every meal", "B) Most meals", "C) Sometimes", "D) Rarely" }
        ),
        new Question(
            "How would you rate your overall commitment to achieving your health goals?",
            new List<string> { "A) Very committed", "B) Moderately committed", "C) Somewhat committed", "D) Not committed" }
        )
    };

   

    public List<Question> categoryFourQuestions = new List<Question>
    {
        new Question(
            "How often do you engage in cardiovascular exercise (e.g., running, cycling)?",
            new List<string> { "A) 5+ times a week", "B) 3-4 times a week", "C) 1-2 times a week", "D) Rarely or never" }
        ),
        new Question(
            "How often do you engage in strength training (e.g., weightlifting)?",
            new List<string> { "A) 3+ times a week", "B) 1-2 times a week", "C) Occasionally", "D) Rarely or never" }
        ),
        new Question(
            "How often do you engage in flexibility exercises (e.g., stretching, yoga)?",
            new List<string> { "A) Daily", "B) Few times a week", "C) Occasionally", "D) Rarely or never" }
        ),
        new Question(
            "How active are you during your work or school day?",
            new List<string> { "A) Very active", "B) Moderately active", "C) Sedentary with occasional movement", "D) Mostly sedentary" }
        ),
        new Question(
            "How often do you incorporate physical activity into your daily routine (e.g., walking, taking stairs)?",
            new List<string> { "A) Daily", "B) Few times a week", "C) Occasionally", "D) Rarely" }
        ),
        new Question(
            "How do you rate your motivation to exercise regularly?",
            new List<string> { "A) Very high", "B) Moderate", "C) Low", "D) Very low" }
        ),
        new Question(
            "How often do you try new forms of physical activity?",
            new List<string> { "A) Frequently", "B) Occasionally", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you participate in group sports or activities?",
            new List<string> { "A) Regularly", "B) Occasionally", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How often do you set and track fitness goals?",
            new List<string> { "A) Regularly", "B) Occasionally", "C) Rarely", "D) Never" }
        ),
        new Question(
            "How satisfied are you with your current physical activity level?",
            new List<string> { "A) Very satisfied", "B) Somewhat satisfied", "C) Neutral", "D) Unsatisfied" }
        )
    };

    

    public List<Question> GetQuestions()
    {
        List<Question> questions = new List<Question>();

        // Adding 5 questions from each category
        questions.AddRange(GetRandomQuestions(categoryOneQuestions, 4));
        questions.AddRange(GetRandomQuestions(categoryTwoQuestions, 3));
        questions.AddRange(GetRandomQuestions(categoryFourQuestions, 3));

        return questions;
    }

    private List<Question> GetRandomQuestions(List<Question> questions, int count)
    {
        List<Question> selectedQuestions = new List<Question>();
        List<Question> shuffledQuestions = new List<Question>(questions);
        Shuffle(shuffledQuestions);

        for (int i = 0; i < Mathf.Min(count, shuffledQuestions.Count); i++)
        {
            selectedQuestions.Add(shuffledQuestions[i]);
        }

        return selectedQuestions;
    }

    private void Shuffle(List<Question> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Question value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
