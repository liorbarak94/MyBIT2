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

    void Update()
    {
        if (db_Manager.showNextLevelToPlay)
        {
            ShowLevelsToPlay();
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

        int level_Index = db_Manager.me_User.buildLevels_Arr
            [db_Manager.me_User.currentBuildLevelToPlay].level_Index;

        StartCoroutine(LoadNextBuildLevelToPlay(level_Index));
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
}
