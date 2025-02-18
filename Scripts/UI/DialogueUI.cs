using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    [SerializeField] private float runTextspeed;
    private TextMeshProUGUI dialogueText;
    private Transform nextImage;
    private Coroutine runSentenceCorou;
    public Action onClickNextSentence;
    private string currentSentence;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    void Start()
    {
        dialogueText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nextImage = transform.Find("NextImage");
    }
    public void LoadSentence(string sentence)
    {
        currentSentence = sentence;
        dialogueText.text = "";
        nextImage.gameObject.SetActive(false);
        runSentenceCorou = StartCoroutine(StartRunSentence(sentence));
    }
    private IEnumerator StartRunSentence(string sentence)
    {
        int charIndex = 0;
        while (dialogueText.text != sentence)
        {
            yield return new WaitForSeconds(runTextspeed);
            dialogueText.text += sentence[charIndex++];
        }
        Invoke("ShowNextImage", 1f);
    }
    private void ShowNextImage()
    {
        nextImage.gameObject.SetActive(true);
        runSentenceCorou = null;
    }
    public void OnClickNextSentence()
    {
        if (runSentenceCorou != null)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            if (!IsInvoking("ShowNextImage"))
                Invoke("ShowNextImage", 1f);
            return;
        }
        onClickNextSentence?.Invoke();
    }
}
