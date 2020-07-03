using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class RegisterNewUser : MonoBehaviour
{
    public enum FeedbackTypeTXTToShow {GOOD, BAD, DEFAULT};
    public FeedbackTypeTXTToShow feedbackTypeTXTToShow;

    DatabaseReference reference;

    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseUser myUser;

    private string displayName;

    public TMP_InputField userNickname_InputField_Login;
    public TMP_InputField userEmailAddress_InputField_Login;

    public TMP_InputField userFirstName_InputField_Register;
    public TMP_InputField userLastName_InputField_Register;
    public TMP_InputField userAge_InputField_Register;
    public TMP_InputField userNickname_InputField_Register;
    public TMP_InputField userEmailAddress_InputField_Register;

    public GameObject waitFeedback;
    public TextMeshProUGUI goodFeedback;
    public TextMeshProUGUI badFeedback;

    private string user_First_Name;
    private string user_Last_Name;
    private float user_Age;
    private string user_Gender;
    private string user_NicknameName;
    private string user_Email;

    public User user;
    private int tmpUserCounter;
    private int tmpLevelsCounter;

    public Level[] buildLevelsFromDB;
    public Level[] situationLevelsFromDB;

    private bool toInitBuildLevelsArr;
    private bool toInitSituationLevelsArr;

    private bool isUserExist_Or_IsMistake_Signin;
    private bool isUserExist_Login;

    private bool loadMainManuScene;


    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        InitializeFirebase();

        loadMainManuScene = false;
        toInitBuildLevelsArr = true;
        toInitSituationLevelsArr = true;
        tmpLevelsCounter = 0;

        feedbackTypeTXTToShow = FeedbackTypeTXTToShow.DEFAULT;

        DownloadLevelsFromDB(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME,
            FinalValues.BUILD_LEVELS_DB_NAME, toInitBuildLevelsArr,
            FinalValues.TypeOfLevel.BUILD);

        DownloadLevelsFromDB(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME,
            FinalValues.SITUATION_LEVELS_DB_NAME, toInitSituationLevelsArr,
            FinalValues.TypeOfLevel.SITUATION);
    }

    private void InitializeFirebase()
    {
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != myUser)
        {
            bool signedIn = myUser != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && myUser != null)
            {
                Debug.Log("Signed out " + myUser.UserId);
            }

            myUser = auth.CurrentUser;

            if (signedIn)
            {
                displayName = myUser.DisplayName ?? "";
                Debug.Log("Signed in " + myUser.UserId);
            }
        }
    }

    public void GirlUserRegistration()
    {
        user_Gender = FinalValues.GIRL_GENDER;
        SetUserParam();
    }

    public void BoyUserRegistration()
    {
        user_Gender = FinalValues.BOY_GENDER;
        SetUserParam();
    }

    public void SetUserParam()
    {
        user_First_Name = userFirstName_InputField_Register.text;
        user_Last_Name = userLastName_InputField_Register.text;
        user_Age = float.Parse(userAge_InputField_Register.text);
        user_NicknameName = userNickname_InputField_Register.text;
        user_Email = userEmailAddress_InputField_Register.text;

        user = new User(user_First_Name, user_Last_Name, user_Gender,
            user_Age, user_NicknameName, user_Email);
    }

    public void DownloadLevelsFromDB(string counter_DB_Name, 
        string type_Levels_DB_Name, bool toInitArr, 
        FinalValues.TypeOfLevel type_Of_Level)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.LEVELS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("DownloadLevelsFromDB IsFaulted");
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    int counter = int.Parse(reference
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(counter_DB_Name)
                        .GetValueAsync().Result.GetValue(true).ToString());

                    DatabaseReference refToLevels = reference
                           .Child(FinalValues.LEVELS_DB_NAME)
                           .Child(type_Levels_DB_Name);

                    for (int i = 0; i < counter; i++)
                    {
                        int levelID = int.Parse(refToLevels
                           .Child(i + "")
                           .Child(FinalValues.LEVEL_ID_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                        int index = int.Parse(refToLevels
                           .Child(i + "")
                           .Child(FinalValues.LEVEL_INDEX_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                        string name = refToLevels
                            .Child(i + "")
                           .Child(FinalValues.LEVEL_NAME_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString();

                        string type = refToLevels
                           .Child(i + "")
                           .Child(FinalValues.LEVEL_TYPE_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString();

                        string path = refToLevels
                           .Child(i + "")
                           .Child(FinalValues.LEVEL_IMAGE_PATH_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString();

                        float timer = float.Parse(refToLevels
                           .Child(i + "")
                           .Child(FinalValues.LEVEL_TIMER_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                        Level level = new Level(levelID, index, name, type, path, timer);

                        if (type_Of_Level == FinalValues.TypeOfLevel.BUILD)
                        {
                            if (toInitArr == true)
                            {
                                toInitArr = false;
                                buildLevelsFromDB = new Level[counter];
                            }
                            buildLevelsFromDB[i] = level;
                        }

                        else if (type_Of_Level == FinalValues.TypeOfLevel.SITUATION)
                        {
                            if (toInitArr == true)
                            {
                                toInitArr = false;
                                situationLevelsFromDB = new Level[counter];
                            }
                            situationLevelsFromDB[i] = level;
                        }
                    }
                    Debug.LogFormat("download Levels successfully");
                }
            });
    }

    public void SetFeedbackTXTGoodOrBadActive_Signin()
    {
        if (isUserExist_Or_IsMistake_Signin) 
        {
            feedbackTypeTXTToShow = FeedbackTypeTXTToShow.BAD;
        }
    }

    public void SetFeedbackTXTGoodOrBadActive_Login()
    {
        if (!isUserExist_Login)
        {
            feedbackTypeTXTToShow = FeedbackTypeTXTToShow.BAD;
        }
    }

    void Update()
    {
        SetFeedbackTXTActive();
        if (loadMainManuScene)
        {
            feedbackTypeTXTToShow = FeedbackTypeTXTToShow.GOOD;
            StartCoroutine(LoadMainManu());
        }
    }

    public IEnumerator LoadMainManu()
    {
        yield return new WaitForSeconds(0.5f);
        loadMainManuScene = false;
        SceneManager.LoadScene(FinalValues.MAIN_MANU_SCENE_INDEX);
    }

    public void SetFeedbackTXTActive()
    {
        switch (feedbackTypeTXTToShow)
        {
            case FeedbackTypeTXTToShow.BAD:
                goodFeedback.gameObject.SetActive(false);
                badFeedback.gameObject.SetActive(true);
                SetWaitFeedbackActivation(false);
                waitFeedback.SetActive(false);
                feedbackTypeTXTToShow = FeedbackTypeTXTToShow.DEFAULT;
                break;

            case FeedbackTypeTXTToShow.GOOD:
                goodFeedback.gameObject.SetActive(true);
                badFeedback.gameObject.SetActive(false);
                SetWaitFeedbackActivation(false);
                waitFeedback.SetActive(false);
                feedbackTypeTXTToShow = FeedbackTypeTXTToShow.DEFAULT;
                break;

            case FeedbackTypeTXTToShow.DEFAULT:
                break;

            default:
                break;
        }
    }

    public string SetCredentialsToRegisterToDB(
        TMP_InputField email, TMP_InputField nickname)
    {
        string email_txt = email.text.Trim();
        string nickname_txt = nickname.text.Trim();

        string[] arr = nickname_txt.Split(' ');
        if (arr.Length > 1)
        {
            nickname_txt = nickname_txt.Replace(" ", "");
        }
        return nickname_txt + email_txt;
    }

    public void SetWaitFeedbackActivation(bool isActive)
    {
        waitFeedback.SetActive(true);

        if (isActive == true)
        {
            goodFeedback.gameObject.SetActive(false);
            badFeedback.gameObject.SetActive(false);
        }
    }

    public void DeactivateFeedbacks()
    {
        goodFeedback.gameObject.SetActive(false);
        badFeedback.gameObject.SetActive(false);
        waitFeedback.SetActive(false);
    }

    public void CreateNewUserInDB()
    {
        string credentialsStr = SetCredentialsToRegisterToDB(
            userEmailAddress_InputField_Register, userNickname_InputField_Register);

        auth.CreateUserWithEmailAndPasswordAsync(credentialsStr, credentialsStr)
            .ContinueWith(task => {

                isUserExist_Or_IsMistake_Signin = false;

                if (task.IsCanceled)
                {
                    isUserExist_Or_IsMistake_Signin = true;
                    Debug.Log("Create New User was canceled");
                    SetFeedbackTXTGoodOrBadActive_Signin();
                }

                if (task.IsFaulted)
                {
                    isUserExist_Or_IsMistake_Signin = true;
                    Debug.Log("Create New User was encounted an error " + task.Exception);
                    SetFeedbackTXTGoodOrBadActive_Signin();
                }

                if (task.IsCompleted)
                {
                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    tmpUserCounter = int.Parse(reference
                           .Child(FinalValues.USERS_COUNTER_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                    for (int i = 0; i < tmpUserCounter && tmpUserCounter > 0; i++)
                    {
                        string nickname = reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(i + "")
                        .Child(FinalValues.USER_NICKNAME_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString();

                        string email = reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(i + "")
                        .Child(FinalValues.USER_EMAIL_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString();

                        if (user.userNickname == nickname &&
                            user.userEmail == email)
                        {
                            isUserExist_Or_IsMistake_Signin = true;
                            Debug.Log("Sign in: User Already Exist: " + isUserExist_Or_IsMistake_Signin);
                        }
                    }

                    if (!isUserExist_Or_IsMistake_Signin)
                    {
                        user.userID = newUser.UserId;
                        string userJson = JsonUtility.ToJson(user);

                        reference
                            .Child(FinalValues.USERS_DB_NAME)
                            .Child(tmpUserCounter + "")
                            .SetRawJsonValueAsync(userJson);

                        reference
                            .Child(FinalValues.USERS_DB_NAME)
                            .Child(tmpUserCounter + "")
                            .Child(FinalValues.LEVELS_DB_NAME)
                            .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                            .SetValueAsync(tmpLevelsCounter);

                        reference
                            .Child(FinalValues.USERS_DB_NAME)
                            .Child(tmpUserCounter + "")
                            .Child(FinalValues.LEVELS_DB_NAME)
                            .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                            .SetValueAsync(tmpLevelsCounter);

                        tmpUserCounter++;
                        reference
                            .Child(FinalValues.USERS_COUNTER_DB_NAME)
                            .SetValueAsync(tmpUserCounter);

                        tmpUserCounter--;


                        DatabaseReference myRef = reference
                           .Child(FinalValues.USERS_DB_NAME)
                           .Child(tmpUserCounter + "")
                           .Child(FinalValues.LEVELS_DB_NAME);

                        tmpLevelsCounter = int.Parse(myRef
                           .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                        if (tmpLevelsCounter < buildLevelsFromDB.Length)
                        {
                            foreach (Level lev in buildLevelsFromDB)
                            {
                                string userLevelsJson = JsonUtility.ToJson(lev);
                                tmpLevelsCounter = lev.level_ID;
                                tmpLevelsCounter++;

                                myRef
                                    .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                                    .Child(lev.level_ID + "")
                                    .SetRawJsonValueAsync(userLevelsJson);

                                myRef
                                    .Child(FinalValues.BUILD_LEVELS_COUNTER_DB_NAME)
                                    .SetValueAsync(tmpLevelsCounter);
                            }
                        }

                        tmpLevelsCounter = int.Parse(myRef
                                .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                                .GetValueAsync().Result.GetValue(true).ToString());

                        if (tmpLevelsCounter < situationLevelsFromDB.Length)
                        {
                            foreach (Level lev in situationLevelsFromDB)
                            {
                                string userLevelsJson = JsonUtility.ToJson(lev);
                                tmpLevelsCounter = lev.level_ID;
                                tmpLevelsCounter++;

                                myRef
                                    .Child(FinalValues.SITUATION_LEVELS_DB_NAME)
                                    .Child(lev.level_ID + "")
                                    .SetRawJsonValueAsync(userLevelsJson);

                                myRef
                                    .Child(FinalValues.SITUATION_LEVELS_COUNTER_DB_NAME)
                                    .SetValueAsync(tmpLevelsCounter);
                            }
                        }

                        if (tmpLevelsCounter == situationLevelsFromDB.Length)
                        {
                            loadMainManuScene = true;
                        }

                        Debug.LogFormat("Firebase user was created successfully: " +
                            "{0} ({1})", newUser.DisplayName, newUser.UserId);
                    }
                }
                SetFeedbackTXTGoodOrBadActive_Signin();
            });
    }

    public void LoginUser()
    {
        string credentialsStr = SetCredentialsToRegisterToDB(
            userEmailAddress_InputField_Login, userNickname_InputField_Login);

        auth.SignInWithEmailAndPasswordAsync(credentialsStr, credentialsStr)
            .ContinueWith(task => {

                isUserExist_Login = false;

                if (task.IsCanceled)
                {
                    isUserExist_Login = false;
                    Debug.Log("Login was canceled");
                    SetFeedbackTXTGoodOrBadActive_Login();
                }

                if (task.IsFaulted)
                {
                    isUserExist_Login = false;
                    Debug.Log("Login was encounted an error " + task.Exception);
                    SetFeedbackTXTGoodOrBadActive_Login();
                }

                if (task.IsCompleted)
                {
                    tmpUserCounter = int.Parse(reference
                           .Child(FinalValues.USERS_COUNTER_DB_NAME)
                           .GetValueAsync().Result.GetValue(true).ToString());

                    for (int i = 0; i < tmpUserCounter && tmpUserCounter > 0; i++)
                    {
                        string nickname = reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(i + "")
                        .Child(FinalValues.USER_NICKNAME_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString();

                        string email = reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(i + "")
                        .Child(FinalValues.USER_EMAIL_DB_NAME)
                        .GetValueAsync().Result.GetValue(true).ToString();

                        if (userNickname_InputField_Login.text == nickname &&
                            userEmailAddress_InputField_Login.text == email)
                        {
                            Firebase.Auth.FirebaseUser newUser = task.Result;
                            Debug.LogFormat("user signed in successfully: {0} ({1})",
                                newUser.DisplayName, newUser.UserId);

                            loadMainManuScene = true;
                            isUserExist_Login = true;
                        }
                    }

                    if (!isUserExist_Login)
                    {
                        Debug.Log("Log in: User Does NOT Exist");
                    }
                }
                SetFeedbackTXTGoodOrBadActive_Login();
            });
    }
}
