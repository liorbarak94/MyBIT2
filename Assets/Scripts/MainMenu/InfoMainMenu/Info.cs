using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Info
{
    public int infoID;
    public string info_name;
    public string info_Image_Path;
    public Texture info_image;
    public string info_Text;

    public Info()
    {
    }

    public Info(string info_name, string info_Image_Path, string info_Text)
    {
        this.info_name = info_name;
        this.info_Image_Path = info_Image_Path;
        this.info_Text = info_Text;
    }

    public Info(string info_name, Texture info_image, string info_Text)
    {
        this.info_name = info_name;
        this.info_image = info_image;
        this.info_Text = info_Text;
    }

    public override string ToString()
    {
        string str = "";
        str += (infoID + " " + info_name + " " 
            + info_Image_Path + " " + info_Text);
        return str;
    }
}
