using System;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string userID;
    public string userFirstName;
    public string userLastName;
    public string userGender;
    public float userAge;
    public string userNickname;
    public string userEmail;

    public Level[] buildLevels_Arr;
    public int currentBuildLevelToPlay;

    public Level[] situationLevels_Arr;
    public int currentSituationLevelToPlay;

    public User()
    {
    }

    public User(string user_First_Name, string user_Last_Name, string user_Gender,
        float user_Age, string user_Nickname, string user_Email)
    {
        this.userFirstName = user_First_Name;
        this.userLastName = user_Last_Name;
        this.userGender = user_Gender;
        this.userAge = user_Age;
        this.userNickname = user_Nickname;
        this.userEmail = user_Email;

        this.currentBuildLevelToPlay = 0;
        this.currentSituationLevelToPlay = 0;
    }

    public User(string user_First_Name, string user_Last_Name, string user_Gender,
        float user_Age, string user_Nickname, string user_Email, 
        int currentBuildLevelToPlay, int currentSituationLevelToPlay)
    {
        this.userFirstName = user_First_Name;
        this.userLastName = user_Last_Name;
        this.userGender = user_Gender;
        this.userAge = user_Age;
        this.userNickname = user_Nickname;
        this.userEmail = user_Email;

        this.currentBuildLevelToPlay = currentBuildLevelToPlay;
        this.currentSituationLevelToPlay = currentSituationLevelToPlay;
    }

    public void InitArrOfBuildLevels(int buildCounter)
    {
        this.buildLevels_Arr = new Level[buildCounter];
    }

    public void InitArrOfSituationLevels(int situationCounter)
    {
        this.situationLevels_Arr = new Level[situationCounter];
    }

    public override string ToString()
    {
        string str = "";
        str += this.userID + " " + this.userFirstName + " " 
            + this.userLastName + " " + this.userGender + " " 
            + this.userAge + " " + this.userNickname + " " + this.userEmail;
        return str;
    }
}

