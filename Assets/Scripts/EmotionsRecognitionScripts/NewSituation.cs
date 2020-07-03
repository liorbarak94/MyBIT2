using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NewSituation : MonoBehaviour
{
    public DatabaseReference reference;

    public TMP_InputField situationStartTextInputField, situationInfo1TextInputField, situationInfo2TextInputField;
    public TMP_InputField titleInputField;
    public TMP_InputField questionTextField, rightAnsTextField;
    public TMP_InputField answer1TextField, answer2TextField, answer3TextField, answer4TextField;
    public TMP_Text answer1Text, answer2Text, answer3Text, answer4Text, rightAnswerText;
    public Button addSituationButton, addQuestionButton;
    public GameObject situationObetects, questionObjects;
    public static string title, questionText, rightAnsText;
    private string[] storyParts = new string[FinalValues.STORY_SIZE];
    public int currentIndex;
    public Situation situation;
    private int situationsCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

       GetSituationCounter();
    }

    private void GetSituationCounter()
    {
        reference.Child(FinalValues.SITUATIONS_COUNTER)
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsFaulted)
                 {
                     // Handle the error...
                 }
                 else if (task.IsCompleted)
                 {
                     DataSnapshot snapshot = task.Result;
                     situationsCounter = int.Parse(snapshot.GetValue(true).ToString());
                 }
             });
    }

    public void WriteNewSituation()
    {
        storyParts[0] = situationStartTextInputField.text.ToString();
        storyParts[1] = situationInfo1TextInputField.text.ToString();
        storyParts[2] = situationInfo2TextInputField.text.ToString();
        title = titleInputField.text.ToString();
        situation = new Situation(situationsCounter, title, storyParts);

        // Add situation details to firebase
        reference.Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situationsCounter + "").Child("Title").SetValueAsync(situation.GetTitle());

        reference.Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situationsCounter + "").Child("SituationStart").SetValueAsync(situation.GetPartsOfTheStory()[0]);

        reference.Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situationsCounter + "").Child("SituationInfo1").SetValueAsync(situation.GetPartsOfTheStory()[1]);

        reference.Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situationsCounter + "").Child("SituationInfo2").SetValueAsync(situation.GetPartsOfTheStory()[2]);

        situationsCounter++;
        reference.Child(FinalValues.SITUATIONS_COUNTER + "").SetValueAsync(situationsCounter);

        situationObetects.SetActive(false);
        questionObjects.SetActive(true);
    }

    public void AddQuestion()
    {
        questionText = questionTextField.text.ToString();

        string[] answers = new string[4];
        if (answer1TextField.text.ToString() != null)
            answers[0] = answer1TextField.text.ToString();
        if (answer2TextField.text.ToString() != null)
            answers[1] = answer2TextField.text.ToString();
        if (answer3TextField.text.ToString() != null)
            answers[2] = answer3TextField.text.ToString();
        if (answer4TextField.text.ToString() != null)
            answers[3] = answer4TextField.text.ToString();

        rightAnsText = rightAnsTextField.text.ToString();
        Question question = new Question(questionText, answers, rightAnsText);

        currentIndex = situation.AddQuestion(question);
        AddQuestToFirebase(question, currentIndex);
        ClearTextFields();
    }

    private void ClearTextFields()
    {
        questionTextField.text = "";
        answer1TextField.text = "";
        answer2TextField.text = "";
        answer3TextField.text = "";
        answer4TextField.text = "";
        rightAnsTextField.text = "";
    }

    private void AddQuestToFirebase(Question question, int currentIndex)
    {
        // Write the question
        reference
            .Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situation.GetID() + "")
            .Child(FinalValues.QUESTIONS_DB_NAME)
            .Child(currentIndex + "")
            .Child(FinalValues.THE_QUESTION_DB_NAME)
            .SetValueAsync(question.GetQuestText());

        // Write the answers
        for (int i = 0; i < question.GetAnswers().Length; i++)
        {
            reference
                .Child(FinalValues.SITUATIONS_DB_NAME)
                .Child(situation.GetID() + "")
                .Child(FinalValues.QUESTIONS_DB_NAME)
                .Child(currentIndex + "")
                .Child(FinalValues.ANSWERS_DB_NAME)
                .Child(i + "").SetValueAsync(question.GetAnswers()[i]);
        }

        // Write the right answer
        reference
            .Child(FinalValues.SITUATIONS_DB_NAME)
            .Child(situation.GetID() + "")
            .Child(FinalValues.QUESTIONS_DB_NAME)
            .Child(currentIndex + "")
            .Child(FinalValues.THE_RIGHT_ANSWER_TEXT).SetValueAsync(question.GetRightAnswer());
    }
}
