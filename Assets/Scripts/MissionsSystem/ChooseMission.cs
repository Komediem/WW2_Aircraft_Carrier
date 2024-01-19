using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMission : MonoBehaviour
{
    public string sceneToLaunchName;

    public void ChooseMissionFunction(MissionCreator mission)
    {
        MissionData.Instance.currentMission = mission;   
        SceneManager.LoadScene(sceneToLaunchName);
    }
}
