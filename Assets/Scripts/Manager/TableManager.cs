using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

public class TableManager
{
    private static TableManager instance = null;

    public static TableManager Instance
    {
        get
        {
            if(null == instance)
            {
                instance = new TableManager();

                instance.init();
            }

            return instance;
        }
    }

    void init()
    {
    }

    private Dictionary<string, List<Dictionary<string, object>>> table = new Dictionary<string, List<Dictionary<string, object>>>();

    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public List<Dictionary<string, object>> GetTable(string file)
    {
        if (table.ContainsKey(file))
        {
            return table[file];
        }
        else
        {
            return Read(file);
        }
    }

    public List<Dictionary<string, object>> Read(string file)
    {
        if(table.ContainsKey(file))
        {
            return table[file];
        }

        var list = new List<Dictionary<string, object>>();
        //TextAsset data = Resources.Load (file) as TextAsset;

#if (UNITY_EDITOR || UNITY_STANDALONE)

        // 경로에 파일 존재하는지 확인
        string dir = Application.dataPath + "/Data/" + file + ".csv";
        if (!File.Exists(dir))
        {
            Debug.Log(file + "파일이 존재하지않습니다.");
            return list;
        }

        string source;
        StreamReader sr = new StreamReader(Application.dataPath + "/Data/" + file + ".csv");
        source = sr.ReadToEnd();
        sr.Close();
        var lines = Regex.Split(source, LINE_SPLIT_RE);
#else
        TextAsset data = Resources.Load (file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
#endif

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }

        table.Add(file, list);
        return list;
    }
}
