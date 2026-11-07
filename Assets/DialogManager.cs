using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public float wrightSpeed = 0.01f;
    public static DialogManager dialogManager;
    private bool wrighting = false;
    public bool talking = false;

    private Queue<string> dialog;

    private string currentText;

    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(dialogManager == null)
        {
            dialogManager = this;
        }
        else
        {
            Destroy(this);
        }
        dialog = new Queue<string>();
        ClearDialog();
        textBox.SetActive(false);
    }

    public void StartDialog(string[] text)
    {
        
        ToggleTextBox();
        Debug.Log("Talking");
        dialog.Clear();
        talking = true;
        foreach(string line in text)
        {
            dialog.Enqueue(line);
            Debug.Log($"added {line} to queue");
        }
        ContinueDialog();

    }
    public void StartDialog(string[] text, string[] secondText)
    {
        
        ToggleTextBox();
        Debug.Log("Talking");
        dialog.Clear();
        talking = true;
        foreach (string line in text)
        {
            dialog.Enqueue(line);
            Debug.Log($"added {line} to queue");
        }
        foreach (string line in secondText)
        {
            dialog.Enqueue(line);
            Debug.Log($"added {line} to queue");
        }
        ContinueDialog();

    }
    public void ContinueDialog()
    {
        if (!wrighting)
        {
            ClearDialog();
            if(dialog.Count == 0)
            {
                StopTalking();
                return;
            }
            currentText = dialog.Dequeue();
            StartCoroutine(WriteDialog());
                
        }
        else
        {
            StopAllCoroutines();
            ClearDialog();
            WriteDialog(currentText);
            wrighting = false;
        }
    }
    public void StopTalking()
    {

        Debug.Log("stopped talking");
        ToggleTextBox();
        talking = false;
    }
    IEnumerator WriteDialog()
    {
        wrighting = true;
        for(int i =0; i < currentText.Length; i += 1)
        {
            WriteDialog(currentText[i].ToString());
            yield return new WaitForSeconds(wrightSpeed);
        }
        wrighting = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject textBox;
    public TMP_Text dialogText;

    public void ToggleTextBox()
    {
        if (textBox.activeSelf)
        {
            textBox.SetActive(false);
        }
        else
        {
            textBox.SetActive(true);
        }
    }
    public void WriteDialog(string Dialog)
    {
        dialogText.text += Dialog;
    }
    public void ClearDialog()
    {
        dialogText.text = "";
    }
    
}
