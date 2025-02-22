﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum MissionName
{
    CALL_OUT_MY_NAME,
    LETS_HAVE_A_BRAWL,
    MEETING_THE_OLD_BUDS
}

[System.Serializable]
public class LevelInstructionObject
{
    [SerializeField]
    private string instruction;
    [SerializeField]
    private int cashReward;

    public string GetInstruction
    {
        get => this.instruction;
    }

    public int GetReward
    {
        get => this.cashReward;
    }
}

public class LevelData : MonoBehaviour
{
    public Transform playerStartPosition;
    public GameObject startCutSceneObject;
    public float cashEarning = 0f;

    public int currentLevel;

    public bool needToAvoidCops = false;
    public bool callCopsExplicitely = false;
    public Difficulty callingCops;

    public float durationBeforeMissionComplete = 0f;

    public List<LevelInstructionObject> levelInstructions;


    public MissionName missionName;

    [HideInInspector]
    public bool isPedestrianAttackMission = false;
    public bool isTutorialMission = false;

    public UnityEvent eventOnLevelComplete, eventOnLevelFail;


    private void OnEnable()
    {
        this.currentLevel = System.Int32.Parse(this.gameObject.name[this.gameObject.name.Length - 1].ToString());
        //PreferenceManager.ClearedLevels = this.currentLevel;
    }

    public void ActivateLevel()
    {
        this.gameObject.SetActive(true);
    }

    public LevelInstructionObject GetLevelInstruction(int index)
    {
        if(index < this.levelInstructions.Count)
        {
            return this.levelInstructions[index];
        }
        return null;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (this.startCutSceneObject)
            this.startCutSceneObject.SetActive(true);

        GameManager.instance.currentMission = this;
        this.PositionPlayerForMission();
    }

    public void PositionPlayerForMission()
    {

        if (this.playerStartPosition)
        {
            GameManager.instance.playerController.gameObject.transform.position = this.playerStartPosition.position;
            GameManager.instance.playerController.gameObject.transform.rotation = this.playerStartPosition.rotation;

        }
    }

    public void MissionComplete()
    {
        if(this.callCopsExplicitely)
        {
            GameManager.instance.policeManager.CallCops(this.callingCops);
            this.needToAvoidCops = true;
        }
        if(this.needToAvoidCops)
        {
            GameManager.instance.gameplayHUD.TypeInstruction("Avoid Cops");
            GameManager.instance.policeManager.onCopsAvoided += this.MissionCompletion;
            return;
        }
        else
        {
            GameManager.instance.policeManager.EvadeCops();
        }
             Invoke("MissionCompletion", this.durationBeforeMissionComplete);
    }

    void MissionCompletion()
    {
      
    }

    public virtual void OnEnemyNeutralized()
    {

    }

    void CompleteMission()
    {
       // GameManager.instance.MissionComplete(this.cashEarning);
        Destroy(this.gameObject);
    }

    public void MissionFail()
    {
        GameManager.instance.gameplayHUD.MissionFail();
        //if (PreferenceManager.ClearedLevels > 3)
        //{
        //    GameManager.instance.policeManager.CallCops(this.callingCops);
        //    GameManager.instance.gameplayHUD.TypeInstruction("Avoid Cops");
        //}
        //Destroy(this.gameObject);
    }

    private void OnDisable()
    {

    }
}
