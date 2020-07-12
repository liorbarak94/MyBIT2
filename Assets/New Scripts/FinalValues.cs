using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalValues : MonoBehaviour
{
    public enum TypeOfLevel { BUILD, SITUATION };

    public static string COIN_AUDIO = "coin";
    public static string VICTORY_AUDIO = "victory";
    public static string PLAYER = "Player";


    ////////// Scene's Indexs ///////////////////////////////
    public static int REGISTRATION_SCENE_INDEX = 0;
    public static int MAIN_MENU_SCENE_INDEX = 1;

    public static int BUILD_BRIDGE_SCENE_INDEX = 2;
    public static int BUILD_SOFA_SCENE_INDEX = 3;

    public static int SITUATION_CLASS_BULLYING_SCENE_INDEX = 4;
    public static int SITUATION__SCENE_INDEX = 5;


    ////////// Registration ///////////////////////////////

    public static string GIRL_GENDER = "בת";
    public static string BOY_GENDER = "בן";

    ////////// Firebase DB ///////////////////////////////
    public static string FIREBASE_URL = "https://mybit-33396.firebaseio.com/";

    public static string INFORMATIONS_DB_NAME = "Informations";
    public static string INFORMATIONS_COUNTER_DB_NAME = "InformationsCounter";

    public static string USERS_DB_NAME = "Users";
    public static string USERS_COUNTER_DB_NAME = "UsersCounter";

    public static string USER_ID_DB_NAME = "userID";
    public static string USER_INDEX_DB_NAME = "userIndex";
    public static string USER_FIRST_NAME_DB_NAME = "userFirstName";
    public static string USER_LAST_NAME_DB_NAME = "userLastName";
    public static string USER_GENDER_DB_NAME = "userGender";
    public static string USER_AGE_DB_NAME = "userAge";
    public static string USER_NICKNAME_DB_NAME = "userNickname";
    public static string USER_EMAIL_DB_NAME = "userEmail";
    public static string USER_CURRENT_BUILD_LEVEL_DB_NAME = "currentBuildLevelToPlay";
    public static string USER_CURRENT_SITUATION_LEVEL_DB_NAME = "currentSituationLevelToPlay";
    public static string TOTAL_BUILD_LEVELS_PLAYED_DB_NAME = "totalBuildLevelPlayed";
    public static string TOTAL_SITUATION_LEVELS_PLAYED_DB_NAME = "totalSituationLevelPlayed";

    public static string LEVELS_DB_NAME = "Levels";
    public static string BUILD_LEVELS_DB_NAME = "BuildLevels";
    public static string SITUATION_LEVELS_DB_NAME = "SituationLevels";

    public static string BUILD_LEVELS_COUNTER_DB_NAME = "BuildLevelsCounter";
    public static string SITUATION_LEVELS_COUNTER_DB_NAME = "SituationLevelsCounter";

    public static string LEVEL_ID_DB_NAME = "level_ID";
    public static string LEVEL_INDEX_DB_NAME = "level_Index";
    public static string LEVEL_NAME_DB_NAME = "level_Name";
    public static string LEVEL_TYPE_DB_NAME = "level_Type";
    public static string LEVEL_TIMER_DB_NAME = "level_Timer";
    public static string LEVEL_NUMBER_OF_MISTAKES_OR_AVERAGE_NUMBER_OF_TOUCHES_DB_NAME =
        "numberOfMistakesOrAverageNumberOfTouches";
    public static string LEVEL_IS_USER_DID_THE_LEVEL_DB_NAME = "isUserDidTheLevel";
    public static string LEVEL_TOTAL_TIME_DB_NAME = "totalTime";
    public static string TIMES_THE_LEVEL_WAS_PLAYED_DB_NAME = "timesTheLevelWasPlayed";

    public const string SITUATIONS_DB_NAME = "Situations";
    public const string QUESTIONS_DB_NAME = "Questions";
    public const string THE_QUESTION_DB_NAME = "TheQuestion";
    public const string ANSWERS_DB_NAME = "Answers";

    ////////// Main Menu ///////////////////////////////

    ////////// Main Menu - Information ///////////////////////////////
    public static string INFORMATION_TYPE = "information";
    public static int NUM_INFORMATION_ITEMS = 20;
    public static string INFO_TEXT_AREA_IN_INFO_PREFAB = "Info Text";
    public static string INFO_IMAGE_AREA_IN_INFO_PREFAB = "Info Image";


    ////////// Main Menu - Achievements ///////////////////////////////
    public static string LEVEL_TITALE_AREA_IN_ACHIEVEMENTS_PREFAB = "Title";
    public static string LEVEL_TIMER_AREA_IN_ACHIEVEMENTS_PREFAB = "Timer";
    public static string LEVEL_IMAGE_AREA_IN_ACHIEVEMENTS_PREFAB = "Level Image";
    public static string LEVEL_STAR_IMAGE_AREA_IN_ACHIEVEMENTS_PREFAB = "Star Image";

    ////////// My Next Level To Play ///////////////////////////////
    public static int IMPROVEMENT_PRECENT = 10;
    public static int LEVEL_0 = 0;
    public static int LEVEL_1 = 1;
    public static int MAX_MISTAKES_BUILD = 4;
    public static int MAX_MISTAKES_SITUATION = 3;

    ////////// Build ///////////////////////////////
        
    public enum TypeOfArrowAnim {BIG, SMALL};
    public static float WAIT_TIME_FOR_SECOND_TOUCH_OF_PART = 4;
    public static string TAKE_MONEY_TRIGGER_BUILD_SCENE_PIG_ANIMATOR = "TakeMoney";


    ////////// Situation ///////////////////////////////
    public static string SITUATION_TYPE = "situation";
    public static string SITUATIONS_COUNTER = "SituationsCounter";
    public static string THE_RIGHT_ANSWER_TEXT = "TheRightAnswer";
    public static string SITUATION_START_TEXT = "SituationStart";
    public static string SITUATION_INFO1 = "SituationInfo1";
    public static string SITUATION_INFO2 = "SituationInfo2";
    public static string SITUATION_TITLE_TEXT = "Title";
    public static int STORY_SIZE = 3;
    public static int NUM_SITUATION_SCENES = 3;
    public static int NUMBER_OF_QUESTIONS = 3;
    public static int NUMBER_OF_ANSWERS = 4;
    public static int SITUATION_NUMBER_0 = 0;
    public static int SITUATION_NUMBER_1 = 1;
    public static int TOTAL_TIME_ANSWER_QUESTION = 60;
    public static int DELTA_TIME_ANSWER = 5;
   

    ////////// PlayerPrefs ///////////////////////////////

    public static string MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME =
     "MyBIT_Game_User_Index";

    public static string MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME =
      "MyBIT_Game_User_Current_Level_Index";

    public static string MYBIT_GAME_USER_TOTAL_LEVELS_PLAYED_PREFS_NAME =
     "MyBIT_Game_User_Total_Levels_Played";

    public static string CURRENT_TIMER_LEVEL_PLAYER_PREFS_NAME =
        "Current_Timer_Level";

}
