using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyProfileShow : MonoBehaviour
{
    public DB_Manager db_Manager;
    public WaitLoadingBarManager waitLoadingBarManager;

    public TextMeshProUGUI userName_Profile;
    public TextMeshProUGUI userNickname_Profile;
    public TextMeshProUGUI userAge_Profile;
    public TextMeshProUGUI userEmailAddress_Profile;

    void Update()
    {
        if (db_Manager.showProfile)
        {
            ShowDataInProfile();
            db_Manager.showProfile = false;
        }  
    }

    public void ShowDataInProfile()
    {
        if (db_Manager.me_User != null)
        {
            userName_Profile.text = db_Manager.me_User.userFirstName + " "
                + db_Manager.me_User.userLastName;
            userNickname_Profile.text = db_Manager.me_User.userNickname;
            userAge_Profile.text = db_Manager.me_User.userAge + "";
            userEmailAddress_Profile.text = db_Manager.me_User.userEmail;

            waitLoadingBarManager.WaitLoadingBar_Activation(false);
        }
    }
}
