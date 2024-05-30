using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using UnityEditor;

public class DialogueVariableObserver
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public DialogueVariableObserver(TextAsset loadGlobalsJSON)
    {
        // create the story
        Story globalVariablesStory = new Story(loadGlobalsJSON.text);

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();

        // fetch loaded data
        string loadedData = SaveManager.instance.LoadDialogueVariableData();

        if(loadedData != null)
        {
            //TextAsset loadedGlobals = AssetDatabase.CreateAsset();
            //globalVariablesStory = loadedData;
        }

        else
        {
            foreach(string name in globalVariablesStory.variablesState)
            {
                Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
                variables.Add(name, value);
                Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
            }
        }

        /*if(loadedData != null)
        {
            string 
        }*/

        //int currentVariableIndex = 0;

        //foreach (string name in globalVariablesStory.variablesState)
        //{
        //    if (loadedData != null)
        //    {
                
        //        Ink.Runtime.Object value = loadedVariable.Value;
        //        variables.Add(name, value);
        //        Debug.Log("Loaded global dialogue variable added: " + name + " = " + value);
        //    }

        //    else
        //    {
        //        Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
        //        variables.Add(name, value);
        //        Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        //    }

        //    currentVariableIndex++;
        //}
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }

        Debug.Log("Changed dialogue variable value:" + name + " = " + value);
        SaveManager.instance.SaveGame();
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}