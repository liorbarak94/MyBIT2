
public class User
{
    public string userID;
    public int userIndex;
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

    public void InitAllUserParams(string user_ID, int userIndex, string user_First_Name,
        string user_Last_Name, string user_Gender, float user_Age,
        string user_Nickname, string user_Email,
        int currentBuildLevelToPlay, int currentSituationLevelToPlay)
    {
        userID = user_ID;
        this.userIndex = userIndex;

        userFirstName = user_First_Name;
        userLastName = user_Last_Name;
        userGender = user_Gender;
        userAge = user_Age;
        userNickname = user_Nickname;
        userEmail = user_Email;

        this.currentBuildLevelToPlay = currentBuildLevelToPlay;
        this.currentSituationLevelToPlay = currentSituationLevelToPlay;
    }

    public void InitArrOfBuildLevels(int buildCounter)
    {
        buildLevels_Arr = new Level[buildCounter];
    }

    public void InitArrOfSituationLevels(int situationCounter)
    {
        situationLevels_Arr = new Level[situationCounter];
    }

    public override string ToString()
    {
        string str = "";
        str += userID + " " + userIndex +" "+ userFirstName 
            + " " + userLastName + " " + userGender + " " 
            + userAge + " " + userNickname + " " + userEmail
            +" "+ currentBuildLevelToPlay +" "+ currentSituationLevelToPlay;
        return str;
    }
}

