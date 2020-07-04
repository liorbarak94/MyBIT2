using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class UploaderInfo : MonoBehaviour
{
    DatabaseReference reference;

    public TextMeshProUGUI info_Name;
    //public TextMeshProUGUI info_Image_Path;
    public TextMeshProUGUI info_Text;

    public int tmpInfoCounter;
    public Info info;

    public TextMeshProUGUI build_Level_Name;
    public TextMeshProUGUI build_Level_Index;
    //public TextMeshProUGUI build_Level_Image_Path;
    public TextMeshProUGUI build_Level_Timer;
    public string build_Level_Type = "build";

    public TextMeshProUGUI situation_Level_Name;
    public TextMeshProUGUI situation_Level_Index;
    //public TextMeshProUGUI situation_Level_Image_Path;
    public TextMeshProUGUI situation_Level_Timer;
    public string situation_Level_Type = "situation";

    public int tmpBuildLevelsCounter;
    public int tmpSituationLevelsCounter;
    public Level level;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void UploadInfo()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.INFORMATIONS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("UploadInfo IsFaulted");
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    string stringInfoCounter = reference
                           .Child(FinalValues.INFORMATIONS_COUNTER_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString();

                    tmpInfoCounter = int.Parse(stringInfoCounter);

                    info = new Info(info_Name.text, info_Text.text)
                    {
                        infoID = tmpInfoCounter
                    };

                    string json = JsonUtility.ToJson(info);

                    reference
                            .Child(FinalValues.INFORMATIONS_DB_NAME)
                            .Child(info.infoID + "")
                            .SetRawJsonValueAsync(json);

                    tmpInfoCounter++;
                    reference
                        .Child(FinalValues.INFORMATIONS_COUNTER_DB_NAME)
                        .SetValueAsync(tmpInfoCounter);

                    Debug.LogFormat("Upload Info successfully");
                }
            });
    }

    public void UploadBuildLevel()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.LEVELS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("UploadBuildLevel IsFaulted");
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    string stringBuildLevelsCounter = reference
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString();
                    
                    tmpBuildLevelsCounter = int.Parse(stringBuildLevelsCounter);

                    level = new Level(int.Parse(build_Level_Index.text),
                        build_Level_Name.text,
                        build_Level_Type,
                        float.Parse(build_Level_Timer.text))
                    {
                        level_ID = tmpBuildLevelsCounter
                    };

                    string json = JsonUtility.ToJson(level);

                    reference
                            .Child(FinalValues.LEVELS_DB_NAME)
                            .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                            .Child(level.level_ID + "")
                            .SetRawJsonValueAsync(json);

                    tmpBuildLevelsCounter++;
                    reference
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                        .SetValueAsync(tmpBuildLevelsCounter);

                    Debug.LogFormat("Upload BUILD level successfully");
                }
            });
    }

    public void UploadSituationLevel()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.LEVELS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("UploadSituationLevel IsFaulted");
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    DataSnapshot ref_Situation_Level_Counter = reference
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                        .GetValueAsync().Result;

                    if (ref_Situation_Level_Counter.Value == null)
                    {
                        reference
                            .Child(FinalValues.LEVELS_DB_NAME)
                            .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                            .SetValueAsync(0);

                        Debug.Log(ref_Situation_Level_Counter);
                    }

                    string stringSituationLevelsCounter =
                        ref_Situation_Level_Counter.GetValue(true).ToString();

                    tmpSituationLevelsCounter = int.Parse(stringSituationLevelsCounter);

                    level = new Level(int.Parse(situation_Level_Index.text),
                        situation_Level_Name.text,
                        situation_Level_Type,
                        float.Parse(situation_Level_Timer.text))
                    {
                        level_ID = tmpSituationLevelsCounter
                    };

                    string json = JsonUtility.ToJson(level);

                    reference
                            .Child(FinalValues.LEVELS_DB_NAME)
                            .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                            .Child(level.level_ID + "")
                            .SetRawJsonValueAsync(json);

                    tmpSituationLevelsCounter++;
                    reference
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                        .SetValueAsync(tmpSituationLevelsCounter);

                    Debug.LogFormat("Upload SITUATION level successfully");
                }
            });
    }
}
