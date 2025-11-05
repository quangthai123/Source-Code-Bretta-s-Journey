using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject selectSaveSlotUI;
    [SerializeField] private GameObject haveProcessSaveSlot1;
    [SerializeField] private GameObject haveProcessSaveSlot2;
    [SerializeField] private GameObject haveProcessSaveSlot3;

    [SerializeField] private GameObject trongUI1;
    [SerializeField] private GameObject trongUI2;
    [SerializeField] private GameObject trongUI3;


    //[SerializeField] private GameObject startThisSlotUI;

    [SerializeField] private GameObject deleteSlot1;
    [SerializeField] private GameObject deleteSlot2;
    [SerializeField] private GameObject deleteSlot3;
    [Range(1, 3)] public int currentSelectSaveSlot;

    [Header("Slot1 Infor")]
    [SerializeField] private TextMeshProUGUI currencyText1;
    [SerializeField] private TextMeshProUGUI playedTimeText1;
    [SerializeField] private TextMeshProUGUI deadCountText1;
    [Header("Slot2 Infor")]
    [SerializeField] private TextMeshProUGUI currencyText2;
    [SerializeField] private TextMeshProUGUI playedTimeText2;
    [SerializeField] private TextMeshProUGUI deadCountText2;
    [Header("Slot3 Infor")]
    [SerializeField] private TextMeshProUGUI currencyText3;
    [SerializeField] private TextMeshProUGUI playedTimeText3;
    [SerializeField] private TextMeshProUGUI deadCountText3;
    [Header("Buttons")]
    [SerializeField] private Button start1Btn;
    [SerializeField] private Button start2Btn;
    [SerializeField] private Button start3Btn;

    [SerializeField] private Button acceptDelete1Btn;
    [SerializeField] private Button acceptDelete2Btn;
    [SerializeField] private Button acceptDelete3Btn;

    private GameDatas tempGameData;

    private int playedTime;
    private int playedTimeHour;
    private int playedTimeMinute;
    void Start()
    {
        tempGameData = SaveManager.instance.tempGameData;
        selectSaveSlotUI.SetActive(false);
        deleteSlot1.gameObject.SetActive(false);
        deleteSlot2.gameObject.SetActive(false);
        deleteSlot3.gameObject.SetActive(false);

        if(SaveManager.instance.CheckHadSavedData(1))
        {
            currencyText1.text = tempGameData.currency+"";
            deadCountText1.text = "Died: " + tempGameData.deadCount;
            playedTime = tempGameData.playedTime;
            playedTimeHour = playedTime / 3600;
            playedTimeMinute = (playedTime - playedTimeHour * 3600)/60;
            playedTimeText1.text = "Played Time: " + playedTimeHour + "h " + playedTimeMinute+"m";
            trongUI1.gameObject.SetActive(false);
            haveProcessSaveSlot1.gameObject.SetActive(true);
        } else
        {
            trongUI1.gameObject.SetActive(true);
            haveProcessSaveSlot1.gameObject.SetActive(false);
        }
        if (SaveManager.instance.CheckHadSavedData(2))
        {
            currencyText2.text = tempGameData.currency + "";
            deadCountText2.text = "Died: " + tempGameData.deadCount;
            playedTime = tempGameData.playedTime;
            playedTimeHour = playedTime / 3600;
            playedTimeMinute = (playedTime - playedTimeHour * 3600) / 60;
            playedTimeText2.text = "Played Time: " + playedTimeHour + "h " + playedTimeMinute + "m";
            trongUI2.gameObject.SetActive(false);
            haveProcessSaveSlot2.gameObject.SetActive(true);
        }
        else
        {
            trongUI2.gameObject.SetActive(true);
            haveProcessSaveSlot2.gameObject.SetActive(false);
        }
        if (SaveManager.instance.CheckHadSavedData(3))
        {

            currencyText3.text = tempGameData.currency + "";
            deadCountText3.text = "Died: " + tempGameData.deadCount;
            playedTime = tempGameData.playedTime;
            playedTimeHour = playedTime / 3600;
            playedTimeMinute = (playedTime - playedTimeHour * 3600) / 60;
            playedTimeText3.text = "Played Time: " + playedTimeHour + "h " + playedTimeMinute + "m";
            trongUI3.gameObject.SetActive(false);
            haveProcessSaveSlot3.gameObject.SetActive(true);
        }
        else 
        {
            trongUI3.gameObject.SetActive(true);
            haveProcessSaveSlot3.gameObject.SetActive(false);
        }

        start1Btn.onClick.RemoveAllListeners();
        start2Btn.onClick.RemoveAllListeners();
        start3Btn.onClick.RemoveAllListeners();

        start1Btn.onClick.AddListener(SaveManager.instance.StartGameData1);
        start2Btn.onClick.AddListener(SaveManager.instance.StartGameData2);
        start3Btn.onClick.AddListener(SaveManager.instance.StartGameData3);

        acceptDelete1Btn.onClick.RemoveAllListeners();
        acceptDelete2Btn.onClick.RemoveAllListeners();
        acceptDelete3Btn.onClick.RemoveAllListeners();

        acceptDelete1Btn.onClick.AddListener(AcceptDeleteSaveSlot);
        acceptDelete2Btn.onClick.AddListener(AcceptDeleteSaveSlot);
        acceptDelete3Btn.onClick.AddListener(AcceptDeleteSaveSlot);

        if(Player.Instance != null)
            Destroy(Player.Instance.gameObject);
    }
    public void OpenSelectSaveSlotUI()
    {
        selectSaveSlotUI.SetActive(true);
    }
    public void CloseSelectSaveSlotUI()
    {
        selectSaveSlotUI.SetActive(false);
    }
    public void OpenDeleteSlotUI(int slot) // Delete Button
    {
        currentSelectSaveSlot = slot;
        if (slot == 1 && SaveManager.instance.CheckHadSavedData(1))
            deleteSlot1.gameObject.SetActive(true);
        else if(slot == 2 && SaveManager.instance.CheckHadSavedData(2))
            deleteSlot2.gameObject.SetActive(true);
        else if(slot == 3 && SaveManager.instance.CheckHadSavedData(3))
            deleteSlot3.gameObject.SetActive(true);
    }
    public void AcceptDeleteSaveSlot() // Yes Button
    {
        if (currentSelectSaveSlot == 1)
        {
            SaveManager.instance.DeleteSaveSlot1();
            haveProcessSaveSlot1.gameObject.SetActive(false);
            trongUI1.gameObject.SetActive(true);
            deleteSlot1.gameObject.SetActive(false);
        } else if (currentSelectSaveSlot == 2)
        {
            SaveManager.instance.DeleteSaveSlot2();
            haveProcessSaveSlot2.gameObject.SetActive(false);
            trongUI2.gameObject.SetActive(true);
            deleteSlot2.gameObject.SetActive(false);
        } else
        {
            SaveManager.instance.DeleteSaveSlot3();
            haveProcessSaveSlot3.gameObject?.SetActive(false);
            trongUI3.gameObject.SetActive(true);
            deleteSlot3.gameObject.SetActive(false);
        }
    }
    public void CancelDeleteSaveSlot() // No Button
    {
        if (currentSelectSaveSlot == 1)
        {
            deleteSlot1.gameObject.SetActive(false);
        }
        else if (currentSelectSaveSlot == 2)
        {
            deleteSlot2.gameObject.SetActive(false);
        }
        else
        {
            deleteSlot3.gameObject.SetActive(false);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
 
}
