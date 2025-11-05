using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameCutSceneManager : MonoBehaviour
{
    private DialogueTextDataSO dialogueTextData;
    private int currentSentenceIndex = 0;
    [SerializeField] private Transform dialogueText;
    void Start()
    {
        dialogueTextData = Resources.Load<DialogueTextDataSO>("Dialogues/StartGameCutSceneDialogue");
        DialogueUI.Instance.onClickNextSentence += LoadNextSentence;
        Invoke("LoadNextSentence", 1f);
    }
    private void LoadNextSentence()
    {
        if (currentSentenceIndex == dialogueTextData.sentences.Count)
        {
            DialogueUI.Instance.onClickNextSentence -= LoadNextSentence;
            StartCoroutine(StartDisableDialogueText());
            return;
        }
        DialogueUI.Instance.LoadSentence(dialogueTextData.sentences[currentSentenceIndex++]);
    }
    private IEnumerator StartDisableDialogueText()
    {
        while(dialogueText.GetComponent<CanvasGroup>().alpha != 0)
        {
            yield return new WaitForSeconds(.1f);
            dialogueText.GetComponent<CanvasGroup>().alpha -= .05f;
        }
        LoadingScene.instance.StartFadeIn(1 / 6f, true);
        Invoke("LoadScene", .5f);
    }
    private void LoadScene()
    {
        Debug.LogWarning("Load!!!");
        SceneManager.LoadSceneAsync("SelectLevel");
    }
}
