using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Converters {

    public static string ConvertStringArrayToString(string[] array)
    {
        StringBuilder builder = new StringBuilder();
        foreach (string value in array)
        {
            builder.Append(value);
            builder.Append(',');
        }
        return builder.ToString().Remove(builder.Length - 1); ;
    }
}
