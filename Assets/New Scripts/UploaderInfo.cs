using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;

public class UploaderInfo : MonoBehaviour
{
    DatabaseReference reference;

    public int userIndex;
    public int levelIndex;
    public float currentTimer;

    public TMP_InputField averageNumberOfTouches_BUILD;
    public TMP_InputField time_BUILD;

    public TMP_InputField numberOfMistakes_SITUATION;
    public TMP_InputField time_SITUATION;

    public bool showStatuseUpload;
    public int statuseUpload;

    public bool loadMainMenuScene;

    public GameObject waitCircleLoadinBar;

    public TextMeshProUGUI goodTXT;
    public TextMeshProUGUI badTXT;


    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        goodTXT.gameObject.SetActive(false);
        badTXT.gameObject.SetActive(false);

        userIndex = PlayerPrefs.GetInt(
            FinalValues.MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME,
            0);

        levelIndex = PlayerPrefs.GetInt(
            FinalValues.MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME,
            0);

        currentTimer = PlayerPrefs.GetFloat(
            FinalValues.CURRENT_TIMER_LEVEL_PLAYER_PREFS_NAME, 3);

        showStatuseUpload = false;
        statuseUpload = 0;

        loadMainMenuScene = false;
    }

    void Update()
    {
        if (loadMainMenuScene)
        {
            loadMainMenuScene = false;
            SceneManager.LoadScene(FinalValues.MAIN_MENU_SCENE_INDEX);
        }

        if (showStatuseUpload)
        {
            showStatuseUpload = false;

            if (statuseUpload == 1) // GOOD
            {
                waitCircleLoadinBar.SetActive(false);

                goodTXT.gameObject.SetActive(true);
                badTXT.gameObject.SetActive(false);

                loadMainMenuScene = true;

                StartCoroutine(LoadMainManu());

            }
            else if (statuseUpload == 2) // BAD
            {
                goodTXT.gameObject.SetActive(false);
                badTXT.gameObject.SetActive(true);
                
                loadMainMenuScene = false;
            }
        }
    }

    public IEnumerator LoadMainManu()
    {
        yield return new WaitForSeconds(1f);
        loadMainMenuScene = false;
        SceneManager.LoadScene(FinalValues.MAIN_MENU_SCENE_INDEX);
    }

    public void CheckValidationOfBuildInput()
    {
        if (int.Parse(averageNumberOfTouches_BUILD.text) >= 1 
            && float.Parse(time_BUILD.text) >= 0.01f)
        {
            SaveUserDetailsToDBAfterBuildLevel();
        }
        else
        {
            showStatuseUpload = true;
            statuseUpload = 2; // BAD
        }
    }

    public void CheckValidationOfSituationInput()
    {
        if (int.Parse(numberOfMistakes_SITUATION.text) >= 1
            && float.Parse(time_SITUATION.text) >= 0.01f)
        {
            SaveUserDetailsToDBAfterSituationLevel();
        }
        else
        {
            showStatuseUpload = true;
            statuseUpload = 2; // BAD
        }
    }

    public void SaveUserDetailsToDBAfterBuildLevel()
    {
        waitCircleLoadinBar.SetActive(true);

        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task => {

                if (task.IsFaulted)
                {
                    Debug.Log("SaveUserDetailsToDBAfterLevel IsFaulted");
                    statuseUpload = 2; // BAD
                }

                if (task.IsCompleted)
                {
                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_NUMBER_OF_MISTAKES_OR_AVERAGE_NUMBER_OF_TOUCHES_DB_NAME)
                        .SetValueAsync(int.Parse(averageNumberOfTouches_BUILD.text));

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_IS_USER_DID_THE_LEVEL_DB_NAME)
                        .SetValueAsync(true);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_TOTAL_TIME_DB_NAME)
                        .SetValueAsync(float.Parse(time_BUILD.text));

                    levelIndex++;
                    reference
                       .Child(FinalValues.USERS_DB_NAME)
                       .Child(userIndex + "")
                       .Child(FinalValues.USER_CURRENT_BUILD_LEVEL_DB_NAME)
                       .SetValueAsync(levelIndex);

                    showStatuseUpload = true;
                    statuseUpload = 1; // GOOD

                    Debug.LogFormat(
                        "Saved User Details To DB After Finished Build Level Successfully");
                }
            });
    }

    public void SaveUserDetailsToDBAfterSituationLevel()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task => {

                if (task.IsFaulted)
                {
                    Debug.Log("SaveUserDetailsToDBAfterSituationLevel IsFaulted");
                    statuseUpload = 2; // BAD
                }

                if (task.IsCompleted)
                {
                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_NUMBER_OF_MISTAKES_OR_AVERAGE_NUMBER_OF_TOUCHES_DB_NAME)
                        .SetValueAsync(int.Parse(numberOfMistakes_SITUATION.text));

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_IS_USER_DID_THE_LEVEL_DB_NAME)
                        .SetValueAsync(true);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_TOTAL_TIME_DB_NAME)
                        .SetValueAsync(float.Parse(time_SITUATION.text));

                    levelIndex++;
                    reference
                       .Child(FinalValues.USERS_DB_NAME)
                       .Child(userIndex + "")
                       .Child(FinalValues.USER_CURRENT_SITUATION_LEVEL_DB_NAME)
                       .SetValueAsync(levelIndex);

                    Debug.LogFormat(
                        "Saved User Details To DB After Finished Situation Level Successfully");

                    showStatuseUpload = true;
                    waitCircleLoadinBar.SetActive(true);
                    statuseUpload = 1; // GOOD
                }
            });
    }
}
