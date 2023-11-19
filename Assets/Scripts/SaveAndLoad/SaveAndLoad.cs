using System.IO;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData;

    private void Start()
    {
        LoadFile();
    }

    private void OnApplicationQuit()
    {
        SaveToFile();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveToFile();
    }

    public void SaveToFile()
    {
        saveData.Save();
        string json = JsonUtility.ToJson(saveData);
        if (!File.Exists(Application.persistentDataPath + "/data.save"))
        {
            File.Create(Application.persistentDataPath + "/data.save").Dispose();
        }

        File.WriteAllText(Application.persistentDataPath + "/data.save", json);
    }

    public void LoadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/data.save"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/data.save");
            saveData = JsonUtility.FromJson<SaveData>(json);
            saveData.Load();
        }
        else
        {
            saveData = new SaveData();
        }
    }

    /*public void LoadGame()
    {
        StartCoroutine(LoadingScreen());
    }

    private IEnumerator LoadingScreen()
    {
        loadingScreen.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync("World");
        while (!async.isDone)
        {
            progressBar.fillAmount = async.progress;
            if (async.progress >= 0.95f)
            {
                progressText.text = "Press any Key to continue";
            }
            yield return null;
        }

        if (Input.anyKey) async.allowSceneActivation = true;
        {
            loadingScreen.SetActive(false);
            yield return null;
        }
    }*/
}
