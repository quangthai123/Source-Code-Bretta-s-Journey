using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TempGameData", menuName = "ScriptableObjects/GameDatas")]
public class GameDatas : ScriptableObject
{
    [Header("Current Save Slot")]
    public int saveSlot;
    public int deadCount;
    public int playedTime;
    public int preShowInterstitialTime;
    [Header("Scenario Select Lv")]
    public bool isScenario;
    [Header("Player Pos")]
    public string currentScene;
    public string tempCurrentScene;
    public Vector2 initializePos;
    public Vector2 revivalCheckPointPos;
    public int facingDir;
    [Header("Player Stats")]
    public int currency;
    public float currentHealth;
    public Stat maxHealth;
    public int currentMana;
    public Stat maxMana;
    public int currentSwordLv;
    public bool haveDied;
    //public bool breakResistMana;
    [Header("Flask Infor")]
    public int flaskQuantity;
    public int fullHealFlaskQuantity;
    public int flaskLv;
    [Header("Soul Infor")]
    public string soulScene;
    public Vector2 soulPos;
    public int currencySoul;

    [Header("Items Infor")]
    public List<int> newArmorialItems;
    public List<int> newImportantItems;
    public List<int> newSwordPieceItems;
    public List<int> newMagicGemItems;

    public List<int> amorialHadItems;
    public List<int> amorialEquippedItems;
    public int currenArmorialSlot;

    public List<int> importantHadItems;
    public List<int> usedImportantItems;

    public List<int> swordPieceHadItems;
    public List<int> swordPieceEquippedItems;
    public List<int> swordPieceMergedItems;

    public List<int> perfectSwordsHad;
    public List<int> perfectSwordsEquipped;
    public int currentSwordPieceSlot;

    public List<int> swordPairsActivated;
    
    public List<int> magicGemHadItems;
    public List<int> magicGemEquippedItems;


    [Header("Boss Infor")]
    public bool winBoss1;

    [Header("Inventory Infor")]
    public int selectingTab;

    [Header("Sword Skill Infor")]
    public List<bool> learnedSkill;

    public bool finishLv1;
    public List<bool> upgradedSwordStatues;
    public int[] upgradedSwordLv; // luu index tuong nao da nang kiem lv nao, index cua tuong tuong ung voi vi tri trong mang
    public void SetNewGame(int _saveSlot)
    {
        saveSlot = _saveSlot;
        deadCount = 0;
        playedTime = 0;
        preShowInterstitialTime = 0;

        isScenario = false;

        currentScene = "StartGameCutScene";
        tempCurrentScene = currentScene;
        initializePos = new Vector2(99f, 850f); // dau game cai nay se xac dinh bang voi vi tri nhan vat nhay tu tren nui xuong giao dien select lv sau cutscene//
                                                // sau do se trigger va cham va xac dinh lai bang vi tri checkpoint cua select lv

        revivalCheckPointPos = new Vector2(-32f, 16f); // dau game cai nay se xac dinh o giao dien select lv bang voi vi tri checkpoint
        facingDir = 1;

        currency = 0;
        maxHealth = new Stat(100f);
        currentHealth = 100f;
        maxMana = new Stat(100f);
        currentMana = 0;
        currentSwordLv = 0;
        haveDied = false;
        //breakResistMana = true;

        flaskQuantity = 2;
        fullHealFlaskQuantity = 2;
        flaskLv = 0;

        soulScene = string.Empty;
        soulPos = Vector2.zero;
        currencySoul = 0;

        newArmorialItems = null;
        newImportantItems = null;
        newSwordPieceItems = null;
        newMagicGemItems = null;

        amorialHadItems = null;
        amorialEquippedItems = null;
        currenArmorialSlot = 2;

        importantHadItems = null;
        usedImportantItems = null;

        swordPieceHadItems = null;
        swordPieceEquippedItems = null;
        currentSwordPieceSlot = 4;
        swordPieceMergedItems = new List<int>();

        perfectSwordsHad = new List<int>();

        perfectSwordsEquipped = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            perfectSwordsEquipped.Add(-1);
        }
        swordPairsActivated = new List<int>();

        magicGemHadItems = new List<int>();
        magicGemEquippedItems = new List<int>();
        for (int i = 0; i < 2; i++)
        {
            magicGemEquippedItems.Add(-1);
        }

        winBoss1 = false;

        selectingTab = 0;

        learnedSkill = new List<bool>();
        for (int i = 0; i < 18; i++)
        {
            learnedSkill.Add(false);
        }

        finishLv1 = false;
        upgradedSwordStatues = new List<bool>();
        for (int i=0; i<7; i++) 
        {
            upgradedSwordStatues.Add(false);
        }
        upgradedSwordLv = new int[7] { -1, -1, -1, -1, -1, -1, -1 };
    }
    public void GetDataFrom(GameDatas _gameData)
    {
        this.saveSlot = _gameData.saveSlot;
        this.deadCount = _gameData.deadCount;
        this.playedTime = _gameData.playedTime;
        preShowInterstitialTime = _gameData.preShowInterstitialTime;

        this.isScenario = _gameData.isScenario;

        this.currentScene = _gameData.currentScene;
        this.tempCurrentScene = _gameData.currentScene;
        this.initializePos = _gameData.initializePos;
        this.revivalCheckPointPos = _gameData.revivalCheckPointPos;
        this.facingDir = _gameData.facingDir;

        this.currency = _gameData.currency;
        this.maxHealth = _gameData.maxHealth;
        this.currentHealth = _gameData.currentHealth;
        this.maxMana = _gameData.maxMana;
        this.currentMana = _gameData.currentMana;
        this.currentSwordLv = _gameData.currentSwordLv;
        this.haveDied = _gameData.haveDied;
        //this.breakResistMana = _gameData.breakResistMana;

        this.flaskQuantity = _gameData.flaskQuantity;
        this.fullHealFlaskQuantity = _gameData.fullHealFlaskQuantity;
        this.flaskLv = _gameData.flaskLv;

        this.soulScene = _gameData.soulScene;
        this.soulPos = _gameData.soulPos;
        this.currencySoul = _gameData.currencySoul;

        this.newArmorialItems = _gameData.newArmorialItems;
        this.newImportantItems = _gameData.newImportantItems;
        this.newSwordPieceItems = _gameData.newSwordPieceItems;
        this.newMagicGemItems = _gameData.newMagicGemItems;

        this.amorialHadItems = _gameData.amorialHadItems;
        this.amorialEquippedItems = _gameData.amorialEquippedItems;
        this.currenArmorialSlot = _gameData.currenArmorialSlot;

        this.importantHadItems = _gameData.importantHadItems;
        this.usedImportantItems = _gameData.usedImportantItems;

        this.swordPieceHadItems = _gameData.swordPieceHadItems;
        this.swordPieceEquippedItems = _gameData.swordPieceEquippedItems;
        this.currentSwordPieceSlot = _gameData.currentSwordPieceSlot;
        this.swordPieceMergedItems = _gameData.swordPieceMergedItems;

        this.perfectSwordsHad = _gameData.perfectSwordsHad;
        this.perfectSwordsEquipped = _gameData.perfectSwordsEquipped;

        this.swordPairsActivated = _gameData.swordPairsActivated;

        this.magicGemHadItems = _gameData.magicGemHadItems;
        this.magicGemEquippedItems = _gameData.magicGemEquippedItems;

        this.winBoss1 = _gameData.winBoss1;

        this.selectingTab = _gameData.selectingTab;

        this.learnedSkill = _gameData.learnedSkill;

        this.finishLv1 = _gameData.finishLv1;

        this.upgradedSwordStatues = _gameData.upgradedSwordStatues;
        this.upgradedSwordLv = _gameData.upgradedSwordLv;
    }
}

