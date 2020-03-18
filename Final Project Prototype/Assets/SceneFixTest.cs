using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SceneFixTest : MonoBehaviour
{
    StringBuilder builder = new StringBuilder();

    private void Start()
    {
        ReadString();
        WriteString();
    }
    public void WriteString()
    {
        string path = "Assets/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(builder.ToString());
        writer.Close();
    }


    public void ReadString()
    {
        string path = "Assets/New Text Document.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        string[] texts = text.Split(new string[] { "<<<<<<< Updated upstream", "=======" }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < texts.Length; i++)
        {
            if (i % 2 == 0)
            {
                builder.Append(texts[i]);
            }
            if (i==texts.Length-1 && !(i % 2 == 0))
            {
                builder.Append(texts[i]);
            }

        }

        reader.Close();
    }
}
