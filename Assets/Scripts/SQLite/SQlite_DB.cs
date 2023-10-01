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
    [Header("HighScore 점수 갱신 관련 변수")]
    public TMP_Text temp;

    [Header("점수 등록을 위한 변수들")]
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
                print("DB연결 성공");
            }
            else
            {
                print("연결실패[ERROR]");
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
        dbConnection.Open(); // DB 열기
        dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query; // 쿼리입력
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
        // 점수를 넣어야 하는데, 등록 이름이 없다면 등록하지 않고 반려시킨다.
        if(UserName_inputField.text.Length == 0)
        {
            return;
        }

        // 점수 등록과정. InsertQueryDataBase() 함수를 이용해 등록 닉네임과 점수를 등록한다.
        // 버틴 초의 합계가 점수가 된다.
        try
        {
            int playerScore = ((int)UImanager.Time_sec) + 60 * UImanager.Time_min;

            InsertQueryDataBase("insert into user_Ranking(ID,SCORE) values(\"" + UserName_inputField.text + "\",\"" + playerScore + "\")");
            DataBaseClose();

            Debug.Log("랭킹 등록 완료.");
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
