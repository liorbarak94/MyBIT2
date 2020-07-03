using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ScrollerViewScript : MonoBehaviour
{
    public DB_Manager db_Manager;
    public WaitLoadingBarManager waitLoadingBarManager;

    public Texture2D silverImageTexture;
    public Texture2D goldImageTexture;

    public RectTransform prefab;
    public ScrollRect scrollerView;
    public RectTransform content;
    private List<ExampleLevelItemView> views = new List<ExampleLevelItemView>();

    private ExampleLevelItemView view;

    public string myType;

    private void Update()
    {
        if (db_Manager.showBuildLevels && myType == FinalValues.BUILD_TYPE)
        {
            ShowLevelsInAchievements(db_Manager.me_User.buildLevels_Arr,
                db_Manager.all_BuildLevels_Images);
            db_Manager.showBuildLevels = false;
            db_Manager.isBuildShown = true;
        }

        if (db_Manager.showSituationLevels && myType == FinalValues.SITUATION_TYPE)
        {
            ShowLevelsInAchievements(db_Manager.me_User.situationLevels_Arr,
                db_Manager.all_SituationLevels_Images);
            db_Manager.showSituationLevels = false;
            db_Manager.isSituationShown = true;
        }

        if (db_Manager.isBuildShown && db_Manager.isSituationShown)
        {
            waitLoadingBarManager.WaitLoadingBar_Activation(false);
        }
    }

    public void ShowLevelsInAchievements(Level[] levels_Arr, Texture[] images)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        views.Clear();

        for (int i = 0; i < levels_Arr.Length; i++)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);

            view = new ExampleLevelItemView();

            view.titleTxt = instance.transform
                .Find(FinalValues.LEVEL_TITALE_AREA_IN_ACHIEVEMENTS_PREFAB)
                .GetComponent<TextMeshProUGUI>();
            view.titleTxt.text = levels_Arr[i].level_Name;

            view.levelImage = instance.transform
                .Find(FinalValues.LEVEL_IMAGE_AREA_IN_ACHIEVEMENTS_PREFAB)
                .GetComponent<RawImage>();
            view.levelImage.texture = images[i];

            view.starImage = instance.transform
                .Find(FinalValues.LEVEL_STAR_IMAGE_AREA_IN_ACHIEVEMENTS_PREFAB)
                .GetComponent<RawImage>();

            if (levels_Arr[i].isUserDidTheLevel == true)
            {
                view.starImage.texture = goldImageTexture;
            }
            else
            {
                view.starImage.texture = silverImageTexture;
            }
            views.Add(view);
        }
    }

    public class ExampleLevelItemView
    {
        public TextMeshProUGUI titleTxt;
        public RawImage levelImage;
        public RawImage starImage;

        public ExampleLevelItemView()
        {
        }
    }
}
