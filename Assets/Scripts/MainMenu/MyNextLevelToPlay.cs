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
            ShowLevelsToPlay();

            AI_CalculateTheTypeOfLevelToPlay();

            db_Manager.showNextLevelToPlay = false;
        }
    }

    public void ShowLevelsToPlay()
    {
        int buildIndexImage = db_Manager.me_User.currentBuildLevelToPlay;
        build_Image_Level.texture = 
            db_Manager.all_BuildLevels_Images[buildIndexImage];
                
        int situationIndexImage = db_Manager.me_User.currentSituationLevelToPlay;
        situation_Image_Level.texture =
            db_Manager.all_SituationLevels_Images[situationIndexImage];

        waitLoadingBarManager.WaitLoadingBar_Activation(false);
    }

    public void BuildLevelWasPressed()
    {
        waitLoadingBarManager.WaitLoadingBar_Activation(true);

        int level_Index_InUnity = db_Manager.me_User.buildLevels_Arr
            [db_Manager.me_User.currentBuildLevelToPlay].level_Index;

        PlayerPrefs.SetInt(
            FinalValues.MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME,
            db_Manager.me_User.userIndex);

        PlayerPrefs.SetInt(
            FinalValues.MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME,
            db_Manager.me_User.currentBuildLevelToPlay);

        PlayerPrefs.SetFloat(
            FinalValues.CURRENT_TIMER_BUILD_LEVEL_PLAYER_PREFS_NAME,
            db_Manager.me_User.buildLevels_Arr
            [db_Manager.me_User.currentBuildLevelToPlay].level_Timer);

        StartCoroutine(LoadNextBuildLevelToPlay(level_Index_InUnity));
    }

    public IEnumerator LoadNextBuildLevelToPlay(int level_Index)
    {
        yield return new WaitForSeconds(0.5f);

        if (level_Index >= 0 && level_Index < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("level Index Does Exist");
            SceneManager.LoadScene(level_Index);
        }
        else
        {
            Debug.Log("level Index Does NOT Exist");
        } 
    }

    public void SituationLevelWasPressed()
    {
        Debug.Log("SituationLevelWasPressed");
    }


    public void AI_CalculateTheTypeOfLevelToPlay()
    {
        if (db_Manager.me_User.currentBuildLevelToPlay == 0 
            && db_Manager.me_User.currentSituationLevelToPlay == 0
            || db_Manager.me_User.currentBuildLevelToPlay == 0
            && db_Manager.me_User.currentSituationLevelToPlay >= 1)
        {
            recommendationImage.texture = buildIconImage;
        }

        if (db_Manager.me_User.currentBuildLevelToPlay >= 1
            && db_Manager.me_User.currentSituationLevelToPlay == 0)
        {
            recommendationImage.texture = situationIconImage;
        }



    }
}
