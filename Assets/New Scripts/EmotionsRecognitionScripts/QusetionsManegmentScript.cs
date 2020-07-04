using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetionsManegmentScript : MonoBehaviour
{
    public Text QuestionText;
    public Image Answer1Image, Answer2Image, Answer3Image, Answer4Image;

    private string str;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("QusetionsManegmentScript Starts");

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://mybit-33396.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance.GetReference("situations").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("");
                Debug.Log(snapshot.Child("1").Child("questions").Child("0").GetValue(true).ToString());
                str = snapshot.Child("1").Child("questions").Child("0").GetValue(true).ToString();
                QuestionText.text = str;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
