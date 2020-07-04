using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollerInfoItems : MonoBehaviour
{
    public DB_Manager db_manager;
    public WaitLoadingBarManager waitLoadingBarManager;

    private ExampleInfoItemView view;

    public RectTransform prefab;
    public ScrollRect scrollerView;
    public RectTransform content;
    private List<ExampleInfoItemView> views = new List<ExampleInfoItemView>();

    private void Update()
    {
        if (db_manager.showInfo)
        {
            ShowInformations();
            db_manager.showInfo = false;
        }
    }

    public void ShowInformations()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        views.Clear();

        for (int i = 0; i < db_manager.informations_Arr.Count; i++)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);

            view = new ExampleInfoItemView();

            view.info_Text = instance.transform
                .Find(FinalValues.INFO_TEXT_AREA_IN_INFO_PREFAB).GetComponent<TextMeshProUGUI>();
            view.info_Text.text = db_manager.informations_Arr[i].info_Text;
            
            view.info_Image = instance.transform
                .Find(FinalValues.INFO_IMAGE_AREA_IN_INFO_PREFAB).GetComponent<RawImage>();
            view.info_Image.texture = db_manager.informations_Arr[i].info_image;

            views.Add(view);
        }
        waitLoadingBarManager.WaitLoadingBar_Activation(false);
    }

    public class ExampleInfoItemView
    {
        public RawImage info_Image;
        public TextMeshProUGUI info_Text;

        public ExampleInfoItemView()
        {
        }
    }
}
