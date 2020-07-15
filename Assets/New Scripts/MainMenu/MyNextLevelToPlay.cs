using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyNextLevelToPlay : MonoBehaviour
{
    public DB_Manager db_Manager;
    public WaitLoadingBarManager waitLoadingBarManager;

    public Button build_Btn_Level;
    public RawImage build_Image_Level;

    public Button situation_Btn_Level;
    public RawImage situation_Image_Level;

    public RawImage recommendationImage;

    public Texture buildIconImage;
    public Texture situationIconImage;

    void Update()
    {
        if (db_Manager.showNextLevelToPlay)
        {
            PlayerPrefs.SetInt(
                FinalValues.MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME,
                db_Manager.me_User.userIndex);

            AI_CalculateTheTypeOfLevelToPlay();

            db_Manager.showNextLevelToPlay = false;
        }
    }

    public IEnumerator ShowLevelsToPlay()
    {
        yield return new WaitForSeconds(0.5f);

        if (build_Image_Level != null)
        {
            int buildIndexImage = db_Manager.me_User.currentBuildLevelToPlay;
            build_Image_Level.texture =
                db_Manager.all_BuildLevels_Images[buildIndexImage];
        }

        if (situation_Image_Level != null)
        {
            int situationIndexImage = db_Manager.me_User.currentSituationLevelToPlay;
            situation_Image_Level.texture =
                db_Manager.all_SituationLevels_Images[situationIndexImage];
        }
        waitLoadingBarManager.WaitLoadingBar_Activation(false);
    }

    public void BuildLevelWasPressed()
    {
        Debug.Log("BuildLevelWasPressed");

        int level_Index_InUnity = db_Manager.me_User.buildLevels_Arr
            [db_Manager.me_User.currentBuildLevelToPlay].level_Index;

        SaveDitailsToPlayerPrefs(level_Index_InUnity,
            db_Manager.me_User.currentBuildLevelToPlay,
            db_Manager.me_User.totalBuildLevelPlayed,
            db_Manager.me_User.buildLevels_Arr
            [db_Manager.me_User.currentBuildLevelToPlay].level_Timer);
    }

    public void SituationLevelWasPressed()
    {
        Debug.Log("SituationLevelWasPressed");

        int level_Index_InUnity = db_Manager.me_User.situationLevels_Arr
            [db_Manager.me_User.currentSituationLevelToPlay].level_Index;

        SaveDitailsToPlayerPrefs(level_Index_InUnity,
            db_Manager.me_User.currentSituationLevelToPlay,
            db_Manager.me_User.totalSituationLevelPlayed,
            db_Manager.me_User.situationLevels_Arr
            [db_Manager.me_User.currentSituationLevelToPlay].level_Timer);
    }

    public void SaveDitailsToPlayerPrefs(int level_Index_InUnity, int currentLevelToPlay,
        int totalLevelsPlayed, float timer)
    {
        waitLoadingBarManager.WaitLoadingBar_Activation(true);

        PlayerPrefs.SetInt(
            FinalValues.MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME,
            currentLevelToPlay);
        
        PlayerPrefs.SetInt(
            FinalValues.MYBIT_GAME_USER_TOTAL_LEVELS_PLAYED_PREFS_NAME,
            totalLevelsPlayed);

        PlayerPrefs.SetFloat(
            FinalValues.CURRENT_TIMER_LEVEL_PLAYER_PREFS_NAME, timer);

        StartCoroutine(LoadNextLevelToPlay(level_Index_InUnity));
    }

    public IEnumerator LoadNextLevelToPlay(int level_Index)
    {
        yield return new WaitForSeconds(0.5f);

        if (level_Index >= 0 && level_Index < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("level Index Does Exist: " + level_Index);
            SceneManager.LoadScene(level_Index);
        }
        else
        {
            Debug.Log("level Index Does NOT Exist");
        }
    }

    public void AI_CalculateTheTypeOfLevelToPlay()
    {
        // Play in more build levels then situatuion levels
        if (db_Manager.me_User.totalBuildLevelPlayed > db_Manager.me_User.totalSituationLevelPlayed)
        {
            // Check the numberOfMistakesOrAverageNumberOfTouches in the last played level
            if (db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1]
                .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_BUILD)
            {
                if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                    db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                recommendationImage.texture = buildIconImage;
            }

            // Check if there is improvement in total time between two levels in build
            else if (db_Manager.me_User.totalBuildLevelPlayed > FinalValues.LEVEL_1)
            {
                float improvementBuild =
                        db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                        / FinalValues.IMPROVEMENT_PRECENT;
                float deltaTimeBuild =
                    db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                    - db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1].totalTime;
                Debug.Log("improvementBuild: " + improvementBuild);
                Debug.Log("deltaTimeBuild: " + deltaTimeBuild);

                if (deltaTimeBuild < improvementBuild)
                {
                    if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                        db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                    recommendationImage.texture = buildIconImage;
                }
                else
                    recommendationImage.texture = situationIconImage;
            }
            else
                recommendationImage.texture = situationIconImage;

            if (db_Manager.me_User.totalSituationLevelPlayed > FinalValues.LEVEL_0)
            {
                // Check the numberOfMistakesOrAverageNumberOfTouches in the last played level
                if (db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1]
                    .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_SITUATION)
                {
                    if (db_Manager.me_User.currentSituationLevelToPlay != db_Manager.me_User.totalSituationLevelPlayed - 1)
                        db_Manager.UpdateCurrentSituationLevelToPlayInDatabase();
                }

                // Check if there is improvement in total time between two levels in situations
                else if (db_Manager.me_User.totalSituationLevelPlayed > FinalValues.LEVEL_1)
                {
                    float improvementSituation =
                        db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                        / FinalValues.IMPROVEMENT_PRECENT;
                    float deltaTimeSituation =
                        db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                        - db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1].totalTime;
                    Debug.Log("improvementSituation: " + improvementSituation);
                    Debug.Log("deltaTimeSituation: " + deltaTimeSituation);

                    if (deltaTimeSituation < improvementSituation)
                    {
                        if (db_Manager.me_User.currentSituationLevelToPlay != db_Manager.me_User.totalSituationLevelPlayed - 1)
                            db_Manager.UpdateCurrentSituationLevelToPlayInDatabase();
                    }
                }
            }
        }

        // Play in more situation levels then build levels
        else if (db_Manager.me_User.totalBuildLevelPlayed < db_Manager.me_User.totalSituationLevelPlayed)
        {
            // Check the numberOfMistakesOrAverageNumberOfTouches in the last played level
            if (db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1]
                .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_SITUATION)
            {
                if (db_Manager.me_User.currentSituationLevelToPlay != db_Manager.me_User.totalSituationLevelPlayed - 1)
                    db_Manager.UpdateCurrentSituationLevelToPlayInDatabase();
                recommendationImage.texture = situationIconImage;
            }

            // Check if there is improvement in total time between two levels in build
            else if (db_Manager.me_User.totalSituationLevelPlayed > FinalValues.LEVEL_1)
            {
                float improvementSituation =
                        db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                        / FinalValues.IMPROVEMENT_PRECENT;
                float deltaTimeSituation =
                    db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                    - db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1].totalTime;
                Debug.Log("improvementSituation: " + improvementSituation);
                Debug.Log("deltaTimeSituation: " + deltaTimeSituation);

                if (deltaTimeSituation < improvementSituation)
                {
                    if (db_Manager.me_User.currentSituationLevelToPlay != db_Manager.me_User.totalSituationLevelPlayed - 1)
                        db_Manager.UpdateCurrentSituationLevelToPlayInDatabase();
                    recommendationImage.texture = situationIconImage;
                }
                else
                    recommendationImage.texture = buildIconImage;
            }
            else
                recommendationImage.texture = buildIconImage;

            if (db_Manager.me_User.totalBuildLevelPlayed > FinalValues.LEVEL_0)
            {
                // Check the numberOfMistakesOrAverageNumberOfTouches in the last played level
                if (db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1]
                    .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_BUILD)
                {
                    if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                        db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                }

                // Check if there is improvement in total time between two levels in situations
                else if (db_Manager.me_User.totalBuildLevelPlayed > FinalValues.LEVEL_1)
                {
                    float improvementBuild =
                        db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                        / FinalValues.IMPROVEMENT_PRECENT;
                    float deltaTimeBuild =
                        db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                        - db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1].totalTime;
                    Debug.Log("improvementBuild: " + improvementBuild);
                    Debug.Log("deltaTimeBuild: " + deltaTimeBuild);

                    if (deltaTimeBuild < improvementBuild)
                    {
                        if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                            db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                    }
                }
            }
        }

        // Play in the same count of levels
        else if (db_Manager.me_User.totalBuildLevelPlayed == db_Manager.me_User.totalSituationLevelPlayed)
        {
            if (db_Manager.me_User.totalBuildLevelPlayed == FinalValues.LEVEL_0
                && db_Manager.me_User.totalSituationLevelPlayed == FinalValues.LEVEL_0)
                recommendationImage.texture = buildIconImage;

            else if (db_Manager.me_User.totalBuildLevelPlayed > FinalValues.LEVEL_0
                && db_Manager.me_User.totalSituationLevelPlayed > FinalValues.LEVEL_0)
            {
                // Check the numberOfMistakesOrAverageNumberOfTouches in the last played level
                if (db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1]
                    .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_BUILD)
                {
                    if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                        db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                    recommendationImage.texture = buildIconImage;
                }

                else if (db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1]
                    .numberOfMistakesOrAverageNumberOfTouches > FinalValues.MAX_MISTAKES_SITUATION)
                {
                    if (db_Manager.me_User.currentSituationLevelToPlay != db_Manager.me_User.totalSituationLevelPlayed - 1)
                        db_Manager.UpdateCurrentSituationLevelToPlayInDatabase();
                    recommendationImage.texture = situationIconImage;
                }

                // Check if there is improvement in total time between two levels in build
                else if (db_Manager.me_User.totalBuildLevelPlayed > FinalValues.LEVEL_1)
                {
                    float improvementBuild =
                            db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                            / FinalValues.IMPROVEMENT_PRECENT;
                    float deltaTimeBuild =
                        db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 2].totalTime
                        - db_Manager.me_User.buildLevels_Arr[db_Manager.me_User.totalBuildLevelPlayed - 1].totalTime;
                    Debug.Log("improvementBuild: " + improvementBuild);
                    Debug.Log("deltaTimeBuild: " + deltaTimeBuild);

                    float improvementSituation =
                        db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                        / FinalValues.IMPROVEMENT_PRECENT;
                    float deltaTimeSituation =
                        db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 2].totalTime
                        - db_Manager.me_User.situationLevels_Arr[db_Manager.me_User.totalSituationLevelPlayed - 1].totalTime;
                    Debug.Log("improvementSituation: " + improvementSituation);
                    Debug.Log("deltaTimeSituation: " + deltaTimeSituation);

                    if (deltaTimeBuild < improvementBuild)
                    {
                        if (db_Manager.me_User.currentBuildLevelToPlay != db_Manager.me_User.totalBuildLevelPlayed - 1)
                            db_Manager.UpdateCurrentBuildLevelToPlayInDatabase();
                        recommendationImage.texture = buildIconImage;
                    }
                    else if (deltaTimeSituation < improvementSituation)
                        recommendationImage.texture = situationIconImage;
                    else
                        recommendationImage.texture = buildIconImage;
                }
                else
                    recommendationImage.texture = buildIconImage;
            }
        }

        // Shows the next levels
        StartCoroutine(ShowLevelsToPlay());
    }
}

