using System.Globalization;
using UnityEngine;

public static class StringToVector 
{

    public static string Vec3ToStr(Vector3 pVector)
    {
        string temp = pVector.ToString("F2");
        string[] temp1 = temp.Split('(');
        string[] temp2 = temp1[1].Split(')');

        return temp2[0];
    }

    //public static string Vec2ToStr(Vector2 pVector)
    //{
    //    string temp = pVector.ToString("F2");
    //    string[] temp1 = temp.Split('(');
    //    string[] temp2 = temp1[1].Split(')');

    //    return temp2[0];
    //}



    public static Vector3 StrToVector3(string pVector)
    {
        // split the items
        string[] sArray = pVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0], CultureInfo.InvariantCulture),
            float.Parse(sArray[1], CultureInfo.InvariantCulture),
            float.Parse(sArray[2], CultureInfo.InvariantCulture));

        return result;
    }
    public static Vector2 StrToVector2(string pVector)
    {
        string[] sArray = pVector.Split(',');
        Vector2 result = new Vector2(int.Parse(sArray[0]), int.Parse(sArray[1]));
        return result;
    }

}

