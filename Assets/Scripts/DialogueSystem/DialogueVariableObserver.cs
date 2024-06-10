using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

public class DialogueVariableObserver
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;

    public DialogueVariableObserver(TextAsset loadGlobalsJSON)
    {
        // fetch loaded data
        string loadedData = SaveManager.instance.LoadDialogueVariableData();

        if (loadedData != "")
        {
            globalVariablesStory = new Story(loadedData);
            //Debug.Log("Loaded dialogue variable data: " + loadedData);
        }

        else
        {
            globalVariablesStory = new Story(loadGlobalsJSON.text);
            //Debug.Log("Initialized dialogue variables with default values: " + loadGlobalsJSON);
        }

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();

        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            //Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
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

        Debug.Log("Changed dialogue variable value:" + name + " = " + value);
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

    public string ConvertDialogueVariablesToString(TextAsset globalJSON)
    {
        Story story = new Story(globalJSON.text);

        string variableDataToJSON = "{\"inkVersion\":21,\"root\":[[\"\\n\",[\"done\",{\"#f\":5,\"#n\":\"g-0\"}],null],\"done\",{\"global decl\":[\"ev\"";

        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            variableDataToJSON += "," + variable.Value + ",{\"VAR=\":\"" + variable.Key + "\"}";
        }

        variableDataToJSON += ",\"/ev\",\"end\",null],\"#f\":1}],\"listDefs\":{}}" + "}";

        return variableDataToJSON;
    }
}