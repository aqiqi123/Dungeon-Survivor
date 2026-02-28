using System.IO;
using UnityEngine;

public static class SaveManager
{
    //存档文件路径
    private static readonly string SavePath=Path.Combine(Application.persistentDataPath, "save.json");

    /// <summary>
    /// 保存数据到JSON文件
    /// </summary>
    /// <param name="data"></param>
    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"存档已保存至：{SavePath}");
    }

    /// <summary>
    /// 从JSON文件加载数据，如果文件不存在则返回默认数据
    /// </summary>
    /// <returns></returns>
    public static SaveData Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning($"未找到存档文件：{SavePath}，将返回默认数据");
            return new SaveData(); //返回默认数据
        }

        string json = File.ReadAllText(SavePath);
        SaveData date=JsonUtility.FromJson<SaveData>(json);
        Debug.Log($"存档加载成功");

        return date;
    }

    /// <summary>
    /// 删除存档文件
    /// </summary>
    public static void DeleteSave()
    {
        if(File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log($"存档文件已删除：{SavePath}");
        }
    }

    /// <summary>
    /// 是否存在存档文件
    /// </summary>
    /// <returns></returns>
    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }
}
