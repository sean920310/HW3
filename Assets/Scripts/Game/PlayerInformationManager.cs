using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationManager : MonoBehaviour
{
    [Serializable]
    public struct PlayerInfo
    {
        public string name;
        public int score;
        public int smashCount;
        public int defenceCount;
        public int overhandCount;
        public int underhandCount;

        public PlayerInfo(string Name)
        {
            this.name = Name;
            this.score = 0;
            this.smashCount = 0;
            this.defenceCount = 0;
            this.overhandCount = 0;
            this.underhandCount = 0;
        }
    }

    public PlayerInfo Info = new PlayerInfo("Player");

    public void SetPlayerName(string Name)
    {
        Info.name = Name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
