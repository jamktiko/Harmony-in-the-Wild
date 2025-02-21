using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVariableObserver
{
    public Dictionary<DialogueVariables, bool> variables { get; private set; }

    private DialogueVariables latestChangedVariable;

    public DialogueVariableObserver()
    {
        variables = DialogueVariableInitializer.initialVariables;

        // fetch loaded data
        string loadedData = SaveManager.instance.GetLoadedDialogueVariables();

        if (loadedData != "")
        {
            Debug.Log("Dialogue variable data fetched, starting to deserialize.");
            SetSavedVariables(loadedData);
            //Debug.Log("Loaded dialogue variable data: " + loadedData);
        }

        else
        {
            Debug.Log("Loaded dialogue variable data was empty. Using default values instead.");
        }
    }

    private void SetSavedVariables(string loadedData)
    {
        if (loadedData.Contains("inkVersion"))
        {
            Debug.Log("Old version of saved dialogue variable data detected. Using default values instead.");
            return;
        }

        string[] loadedValues = loadedData.Split(",");

        int index = 0;
        foreach(var key in variables.Keys.ToArray())
        {
            if (index < loadedValues.Length)
            {
                bool newValue = System.Convert.ToBoolean(loadedValues[index]);
                variables[key] = newValue;
                index++;
            }
        }

        Debug.Log("Dialogue variables loaded!");
        foreach(var kvp in variables)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }
    }

    public void CallVariableChangeEvent()
    {
        GameEventsManager.instance.dialogueEvents.ChangeDialogueVariable(latestChangedVariable);
    }

    public void ChangeVariable(string variable)
    {
        // fetch the correct enum based on given string
        DialogueVariables correctEnum = GetVariableEnum(variable);

        // set the value to be true, since the corresponding dialogue has been pass
        variables[correctEnum] = true;

        latestChangedVariable = correctEnum;

        SaveManager.instance.SaveGame();
    }

    private DialogueVariables GetVariableEnum(string variableName)
    {
        DialogueVariables name = DialogueVariables.Tutorial_01;

        switch (variableName)
        {
            case "Tutorial_01":
                name = DialogueVariables.Tutorial_01;
                break;

            case "Tutorial_03":
                name = DialogueVariables.Tutorial_03;
                break;

            case "Tutorial_05":
                name = DialogueVariables.Tutorial_05;
                break;

            case "Tutorial_07":
                name = DialogueVariables.Tutorial_07;
                break;

            case "Tutorial_08":
                name = DialogueVariables.Tutorial_08;
                break;

            case "WhaleDiet_02":
                name = DialogueVariables.WhaleDiet_02;
                break;

            case "BoneToPick_03":
                name = DialogueVariables.BoneToPick_03;
                break;
        }

        return name;
    }

    public string ConvertVariablesToString()
    {
        string variablesToJSON = "";

        foreach(KeyValuePair<DialogueVariables, bool> value in variables)
        {
            if(variablesToJSON == "")
            {
                variablesToJSON += value.Value;
            }

            else
            {
                variablesToJSON += "," + value.Value;
            }
        }

        return variablesToJSON;
    }

    /*public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;

    public DialogueVariableObserver(TextAsset loadGlobalsJSON)
    {
        // fetch loaded data
        string loadedData = SaveManager.instance.GetLoadedDialogueVariables();

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

        SaveManager.instance.SaveGame();
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
            if(variable.Value == null)
            {
                variableDataToJSON += "," + 0 + ",{\"VAR=\":\"" + variable.Key + "\"}";
            }

            else
            {
                variableDataToJSON += "," + variable.Value + ",{\"VAR=\":\"" + variable.Key + "\"}";
            }
        }

        variableDataToJSON += ",\"/ev\",\"end\",null],\"#f\":1}],\"listDefs\":{}}" + "}";

        return variableDataToJSON;
    }*/
}