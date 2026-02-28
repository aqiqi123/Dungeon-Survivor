using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Data;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string dbPath;
    private SqliteConnection connection;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            InitDatabase();
        } else {
            Destroy(gameObject);
        }
    }

    private void InitDatabase()
    {
        dbPath=Path.Combine(Application.persistentDataPath,"database.db");
        connection=new SqliteConnection($"Data Source={dbPath};Version=3;");

        Debug.Log($"Database path: {dbPath}");
    }

    private void ExecuteNonQuery(string sql)
    {
        try
        {
            connection.Open();
            SqliteCommand cmd=connection.CreateCommand();
            cmd.CommandText=sql;
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            Debug.LogError($"Database error: {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    private void OnApplicationQuit()
    {
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
