using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level
{
    public int level_ID;
    public int level_Index;
    public string level_Name;
    public string level_Type;
    public float level_Timer;
    public int numberOfMistakesOrAverageNumberOfTouches;
    public int timesTheLevelWasPlayed;

    public bool isUserDidTheLevel;
    public float totalTime;

    public Level()
    {
    }

    public Level(int level_Index, string level_Name, string level_Type, 
        float level_Timer)
    {
        this.level_Index = level_Index;
        this.level_Name = level_Name;
        this.level_Type = level_Type;
        this.level_Timer = level_Timer;

        this.numberOfMistakesOrAverageNumberOfTouches = -1;
        this.isUserDidTheLevel = false;
        this.totalTime = 0;
        this.timesTheLevelWasPlayed = 0;
    }

    public Level(int level_ID, int level_Index, string level_Name,
        string level_Type, float level_Timer)
    {
        this.level_ID = level_ID;
        this.level_Index = level_Index;
        this.level_Name = level_Name;
        this.level_Type = level_Type;
        this.level_Timer = level_Timer;

        this.numberOfMistakesOrAverageNumberOfTouches = -1;
        this.isUserDidTheLevel = false;
        this.totalTime = 0;
        this.timesTheLevelWasPlayed = 0;
    }

    public string GetTheTotalTimeUserDidTheLevel()
    {
        return ("0" + level_Timer + ":00");
    }

    public override string ToString()
    {
        string str = "";
        str += level_ID + " " + level_Index + " " + level_Name + " "
            + level_Type + " " + /*level_Image_Path + " " +*/ level_Timer;
        return str;
    }
}
