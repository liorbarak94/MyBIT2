using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DB_Manager : MonoBehaviour
{
    public DatabaseReference reference;
    public DataSnapshot snapshot;

    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser authUser;

    public User me_User;

    [HideInInspector]
    public bool showNextLevelToPlay;

    [HideInInspector]
    public bool showProfile;

    [HideInInspector]
    public bool showInfo;

    [HideInInspector]
    public bool showBuildLevels;

    [HideInInspector]
    public bool showSituationLevels;

    [HideInInspector]
    public bool isBuildShown;

    [HideInInspector]
    public bool isSituationShown;

    public Texture[] all_Informations_Images;
    public string[] all_Informations_Texts;

    public List<Info> informations_Arr = new List<Info>();

    public Texture[] all_BuildLevels_Images;
    public Texture[] all_SituationLevels_Images;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        authUser = auth.CurrentUser;

        me_User = new User();

        showNextLevelToPlay = false;
        showProfile = false;
        showInfo = false;
        showBuildLevels = false;
        showSituationLevels = false;
        isBuildShown = false;
        isSituationShown = false;

        TakeProfileDataFromDB();
        InitInfornationArr();
    }

    public void InitInfornationArr()
    {
        int i = 0;
        foreach (Texture image in all_Informations_Images)
        {
            Info info = new Info(i + "", image, all_Informations_Texts[i]);
            informations_Arr.Add(info);
            i++;
        }
    }

    public Firebase.Auth.FirebaseAuth GetAuth()
    {
        return auth;
    }
    public Firebase.Auth.FirebaseUser GetAuthUser()
    {
        return authUser;
    }

    public void TakeProfileDataFromDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {

                if (task.IsFaulted)
                {
                    Debug.Log("TakeProfileDataFromDB IsFaulted");
                }

                if (task.IsCompleted)
                {
                    string authUserId = authUser.UserId;

                    snapshot = task.Result;

                    int intUserCounter = int.Parse(reference
                        .Child(FinalValues.USERS_COUNTER_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString());

                    for (int i = 0; i < intUserCounter; i++)
                    {
                        string tmpID = snapshot
                        .Child(i + "")
                        .Child(FinalValues.USER_ID_DB_NAME)
                        .GetValue(true).ToString();

                        if (authUserId == tmpID)
                        {
                            int user_Index = int.Parse(snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_INDEX_DB_NAME)
                                .GetValue(true).ToString());

                            string user_First_Name = snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_FIRST_NAME_DB_NAME)
                                .GetValue(true).ToString();

                            string user_Last_Name = snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_LAST_NAME_DB_NAME)
                                .GetValue(true).ToString();

                            string user_Gender = snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_GENDER_DB_NAME)
                                .GetValue(true).ToString();

                            float user_Age = float.Parse(snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_AGE_DB_NAME)
                                .GetValue(true).ToString());

                            string user_Nickname = snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_NICKNAME_DB_NAME)
                                .GetValue(true).ToString();

                            string user_Email = snapshot
                                .Child(i + "")
                                .Child(FinalValues.USER_EMAIL_DB_NAME)
                                .GetValue(true).ToString();

                            TakeBothCurrentLevels();

                            int buildLevelCounter = int.Parse(snapshot
                                .Child(i + "")
                                .Child(FinalValues.LEVELS_DB_NAME)
                                .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                                .GetValue(true).ToString());

                            int situationLevelCounter = int.Parse(snapshot
                                .Child(i + "")
                                .Child(FinalValues.LEVELS_DB_NAME)
                                .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                                .GetValue(true).ToString());

                            me_User.InitAllUserParams(tmpID, user_Index,
                                user_First_Name, user_Last_Name, user_Gender,
                                user_Age, user_Nickname, user_Email,
                                buildLevelCounter, situationLevelCounter);

                            me_User.InitArrOfBuildLevels(buildLevelCounter);
                            me_User.InitArrOfSituationLevels(situationLevelCounter);

                            TakeBothLevels();
                        }
                    }
                    showProfile = true;
                    Debug.Log("download Profile and Achievements successfully");
                }
            });
    }

    public void TakeBothCurrentLevels()
    {
        TakeCurrentLevelsFromUserDB(FinalValues.TypeOfLevel.BUILD);
        TakeCurrentLevelsFromUserDB(FinalValues.TypeOfLevel.SITUATION);
    }

    public void TakeCurrentLevelsFromUserDB(FinalValues.TypeOfLevel typeOfLevel)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {

                if (task.IsFaulted)
                {
                    Debug.Log("TakeCurrentLevelsFromUserDB IsFaulted");
                }

                if (task.IsCompleted)
                {
                    if (typeOfLevel == FinalValues.TypeOfLevel.BUILD)
                    {
                        me_User.currentBuildLevelToPlay =
                        int.Parse(reference
                            .Child(FinalValues.USERS_DB_NAME)
                            .Child(me_User.userIndex + "")
                            .Child(FinalValues.USER_CURRENT_BUILD_LEVEL_DB_NAME)
                            .GetValueAsync().Result.GetRawJsonValue());

                        if (!showNextLevelToPlay)
                        {
                            showNextLevelToPlay = true;
                        }
                    }

                    else if (typeOfLevel == FinalValues.TypeOfLevel.SITUATION)
                    {
                        me_User.currentSituationLevelToPlay =
                        int.Parse(reference
                            .Child(FinalValues.USERS_DB_NAME)
                            .Child(me_User.userIndex + "")
                            .Child(FinalValues.USER_CURRENT_SITUATION_LEVEL_DB_NAME)
                            .GetValueAsync().Result.GetRawJsonValue());

                        if (!showNextLevelToPlay)
                        {
                            showNextLevelToPlay = true;
                        }
                    }
                }
            });
    }

    public void TakeBothLevels()
    {
        if (me_User.buildLevels_Arr != null && me_User.situationLevels_Arr != null)
        {
            TakeLevelsFromUserDB(me_User.buildLevels_Arr.Length, FinalValues.TypeOfLevel.BUILD);
            TakeLevelsFromUserDB(me_User.situationLevels_Arr.Length, FinalValues.TypeOfLevel.SITUATION);
        }
        else
        {
            Debug.Log("TakeBothLevels is NOT working!");
        }
    }

    public void TakeLevelsFromUserDB(int counter, FinalValues.TypeOfLevel typeOfLevel)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {

                if (task.IsFaulted)
                {
                    Debug.Log("TakeLevelsFromUserDB IsFaulted");
                }

                if (task.IsCompleted)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        if (typeOfLevel == FinalValues.TypeOfLevel.BUILD)
                        {
                            me_User.buildLevels_Arr[i] =
                            JsonUtility.FromJson<Level>(reference
                                .Child(FinalValues.USERS_DB_NAME)
                                .Child(me_User.userIndex + "")
                                .Child(FinalValues.LEVELS_DB_NAME)
                                .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                                .Child(i + "")
                                .GetValueAsync().Result.GetRawJsonValue());

                            if (!showBuildLevels)
                            {
                                showBuildLevels = true;
                            }
                        }

                        else if (typeOfLevel == FinalValues.TypeOfLevel.SITUATION)
                        {
                            me_User.situationLevels_Arr[i] =
                            JsonUtility.FromJson<Level>(reference
                                .Child(FinalValues.USERS_DB_NAME)
                                .Child(me_User.userIndex + "")
                                .Child(FinalValues.LEVELS_DB_NAME)
                                .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                                .Child(i + "")
                                .GetValueAsync().Result.GetRawJsonValue());

                            if (!showSituationLevels)
                            {
                                showSituationLevels = true;
                            }
                        }
                    }
                }
            });
    }

    public void SetShowProfileActivation(bool toShow)
    {
        showProfile = toShow;
    }

    public void SetMyNextLevelToPlayAtivation(bool toShow)
    {
        showNextLevelToPlay = toShow;
    }

    public void SetShowInformationAtivation(bool toShow)
    {
        showInfo = toShow;
    }
}