using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Mono.Data.SqliteClient;
using TMPro;


public class SQlite_DB : MonoBehaviour
{
    [Header("HighScore ���� ���� ���� ����")]
    public TMP_Text temp;

    [Header("���� ����� ���� ������")]
    public UImanager_SinglePlay UImanager;
    public TMP_InputField UserName_inputField;


    static IDbConnection dbConnection;
    static IDbCommand dbCommand;
    public static IDataReader dataReader;

    void Start()
    {
        DBConnectionCheck();
    }

    public string GetDBFilePath()
    {
        string str = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            str = "URI=file:" + Application.persistentDataPath + "/MyDB.db";
        }
        else
        {
            str = "URI=file:" + Application.streamingAssetsPath + "/MyDB.db";
        }
        return str;
    }

    public void DBConnectionCheck()
    {
        try
        {
            dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open)
            {
                print("DB���� ����");
            }
            else
            {
                print("�������[ERROR]");
            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }

    public void InsertQueryDataBase(string query)
    {
        dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open(); // DB ����
        dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query; // �����Է�
        dataReader = dbCommand.ExecuteReader();
    }

    public void DataBaseClose()
    {
        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void RecordRanking()
    {
        // ������ �־�� �ϴµ�, ��� �̸��� ���ٸ� ������� �ʰ� �ݷ���Ų��.
        if(UserName_inputField.text.Length == 0)
        {
            return;
        }

        // ���� ��ϰ���. InsertQueryDataBase() �Լ��� �̿��� ��� �г��Ӱ� ������ ����Ѵ�.
        // ��ƾ ���� �հ谡 ������ �ȴ�.
        try
        {
            int playerScore = ((int)UImanager.Time_sec) + 60 * UImanager.Time_min;

            InsertQueryDataBase("insert into user_Ranking(ID,SCORE) values(\"" + UserName_inputField.text + "\",\"" + playerScore + "\")");
            DataBaseClose();

            Debug.Log("��ŷ ��� �Ϸ�.");
        }
        catch (SqliteExecutionException e)
        {
            print(e);
            return;
        }
    }

    public void ShowRanking()
    {
        string query = "SELECT * FROM user_Ranking ORDER BY SCORE DESC";
        int i = 0;
        InsertQueryDataBase(query);
        while (dataReader.Read())
        {
            if (i >= 10) break;
            UImanager.RankingText[i].text = "<" + i + "> [NickName : " + dataReader.GetString(0) + "] & [Score : " + dataReader.GetInt32(1) + "]";
            i++;
        }
        DataBaseClose();
        UImanager.Ranking();
    }

}
