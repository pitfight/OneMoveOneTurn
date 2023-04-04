using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private Button buttonNew;
    private Button buttonLoad;
    private Button buttonSave;
    private Button buttonDelete;

    private ScrollView scrollViewSaves;

    private TextField textField;

    private List<RadioButton> radioButtons;

    [SerializeField] private LevelMenager levelMenager;

    private void Start()
    {
        var frame = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Frame");

        scrollViewSaves = frame.Q<ScrollView>("SaveList");
        textField = frame.Q<TextField>("Input");

        buttonNew = frame.Q<Button>("New");
        buttonLoad = frame.Q<Button>("Load");
        buttonSave = frame.Q<Button>("Save");
        buttonDelete = frame.Q<Button>("Delete");

        buttonLoad.clicked += LoadLevel;
        buttonNew.clicked += NewLevel;
        buttonSave.clicked += SaveLevel;
        buttonDelete.clicked += DeleteSave;

        radioButtons = new List<RadioButton>(10);
    }

    public void NewLevel()
    {
        int seed = 0;
        if (int.TryParse(textField.text, out seed))
        {
            //Load map of Seed.
            levelMenager.GanarateMap(null ,seed);
            Debug.Log(seed);
        }
    }

    public void SaveLevel()
    {
        var radioB = new RadioButton();
        radioB.text = levelMenager.GetCurrentLevel();
        radioButtons.Add(radioB);
        scrollViewSaves.Add(radioB);
    }

    public void LoadLevel()
    {
        var radioB = radioButtons.FirstOrDefault(r => r.value == true);
        
        Debug.Log(radioB?.text);
        if (radioB != null)
        {
            levelMenager.GanarateMap(radioB.text);
        }
    }

    public void DeleteSave()
    {
        var radioB = radioButtons.FirstOrDefault(r => r.value == true);

        if (radioB != null)
        {
            radioB?.RemoveFromHierarchy();
            radioButtons.Remove(radioB);
        }
    }

    public void HideSaveList() => scrollViewSaves.style.display = DisplayStyle.None;
}
