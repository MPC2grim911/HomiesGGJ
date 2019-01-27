﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour {

    private int cutscenePosition;

    [SerializeField]
    private GameObject DialogueBox;
    private Queue<Sentence> currentDialogue;
    private Sentence sentence;

    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject answer1Button;
    [SerializeField]
    private GameObject answer2Button;

    private Dialogue sceneDialogue;

    private int choice;

    [SerializeField]
    private TMPro.TextMeshProUGUI cutsceneText;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText;

    [SerializeField]
    private string fileName;

    private void Start() {
        sceneDialogue = new Dialogue();
        cutscenePosition = 0;
        StartDialogue();
    }

    public void StartDialogue () {
        DialogueBox.SetActive(true);

        sceneDialogue.ConstructDialogue(fileName);
        currentDialogue = sceneDialogue.initialDialogue;
        DisplayNextSentences();
        
    }

    public void DisplayNextSentences() {
        if ((cutscenePosition == 0 && sceneDialogue.question == null && sceneDialogue.initialDialogue.Count == 0) ||
            (cutscenePosition == 1 && (sceneDialogue.response1 == null ||sceneDialogue.response1.Count == 0 
            || sceneDialogue.response2.Count == 0))) {
            EndDialogue();
        } else {
            if (cutscenePosition == 0 && sceneDialogue.initialDialogue.Count == 0 && sceneDialogue.question != null) {
                cutscenePosition++;
                continueButton.SetActive(false);
                answer1Button.SetActive(true);
                answer2Button.SetActive(true);
                nameText.text = sceneDialogue.question.m_name;
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sceneDialogue.question.m_sentence));
                answer1Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneDialogue.question.m_answer1;
                answer2Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneDialogue.question.m_answer2;
            } else {
                answer1Button.SetActive(false);
                answer2Button.SetActive(false);
                continueButton.SetActive(true);
                StopAllCoroutines();
                sentence = currentDialogue.Dequeue();
                nameText.text = sentence.m_name;
                StartCoroutine(TypeSentence(sentence.m_sentence));
            }   
        }            
    }

    IEnumerator TypeSentence(string sentence) {
        cutsceneText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            cutsceneText.text += letter;
            yield return null;
        }
    }

    public void AnswerQuestion(int answer) {      
        if (answer == 0) {
            currentDialogue = sceneDialogue.response1;
        } else {
            currentDialogue = sceneDialogue.response2;
        }

        DisplayNextSentences();
    }

    public void EndDialogue() {
        DialogueBox.SetActive(false);
    }
}
