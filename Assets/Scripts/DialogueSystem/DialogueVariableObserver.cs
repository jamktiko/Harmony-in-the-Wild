using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

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
        List<string> loadedData = SaveManager.instance.GetLoadedData("dialogue");

        int currentVariableIndex = 0;

        foreach (string name in globalVariablesStory.variablesState)
        {
            if (loadedData.Count > 0)
            {
                //variables.Add(LoadedVariable(loadedData[currentVariableIndex]));
            }

            else
            {
                Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
                variables.Add(name, value);
                Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
            }

            currentVariableIndex++;
        }
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
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

    private KeyValuePair<string, Ink.Runtime.Object> LoadedVariable(string serializedData)
    {
        KeyValuePair<string, Ink.Runtime.Object> variable = new KeyValuePair<string, Ink.Runtime.Object>();

        try
        {
            variable = JsonUtility.FromJson<KeyValuePair<string, Ink.Runtime.Object>>(serializedData);
        }

        catch
        {
            Debug.Log("Error when loading saved dialogue variable: " + serializedData);
        }

        return variable;
    }
}