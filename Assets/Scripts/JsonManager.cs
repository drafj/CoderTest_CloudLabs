using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject alreadyQualifiedPanel;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject addNewFieldButton;
    [SerializeField] private TMP_InputField winConditionText;
    [SerializeField] private TMP_InputField minPosibleNoteText;
    [SerializeField] private TMP_InputField maxPosibleNoteText;
    [SerializeField] private DragAndDropManager manager;
    [SerializeField] private float winCondition;
    [SerializeField] private float minPosibleNote;
    [SerializeField] private float maxPosibleNote;
    public List<StudentsData> m_studentsData = new List<StudentsData>();
    [SerializeField] private List<StudentFields> m_fields = new List<StudentFields>();
    [SerializeField] private List<StudentsData> wrongRating = new List<StudentsData>();
    public bool alreadyQualified;

    private string savedFile = "Assets/StreamingAssets";
    private JsonData jsonData = new JsonData();

    private void Awake()
    {
        if (!Directory.Exists(savedFile))
            Directory.CreateDirectory(savedFile);
        savedFile += "/appData.json";
    }

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(savedFile))
        {
            string content = File.ReadAllText(savedFile);
            jsonData = JsonUtility.FromJson<JsonData>(content);

            m_studentsData = jsonData.studentsData;

            AssignFields();

            Debug.Log("datos cargados");
        }
        else
        {
            JsonData newData = new JsonData()
            {
                studentsData = new List<StudentsData>()
            };

            string jsonString = JsonUtility.ToJson(newData);
            File.WriteAllText(savedFile, jsonString);

            Debug.Log("datos creados");
        }
    }

    private void SaveData()
    {
        jsonData = new JsonData()
        {
            studentsData = m_studentsData
        };

        string jsonString = JsonUtility.ToJson(jsonData);
        File.WriteAllText(savedFile, jsonString);

        Debug.Log("datos guardados");
    }

    public void Check()
    {
        if (!alreadyQualified)
        {
            wrongRating.Clear();
            warningText.text = "";
            for (int i = 0; i < m_studentsData.Count; i++)
            {
                AddToStudentsData(i);
                if (m_studentsData[i].finalNote < winCondition && m_studentsData[i].isAproved == 1 || m_studentsData[i].finalNote >= winCondition && m_studentsData[i].isAproved == 2 || m_studentsData[i].isAproved == 0)
                {
                    warningText.text = $"{warningText.text}{m_studentsData[i].firstName} {m_studentsData[i].lastName}\n";
                    wrongRating.Add(m_studentsData[i]);
                }
            }
            if (wrongRating.Count > 0)
            {
                warningPanel.SetActive(true);
            }
            else
            {
                correctPanel.SetActive(true);
            }
        }
    }

    public void AddNewField()
    {
        if (!alreadyQualified)
        {
            StudentsData newStudentData = new StudentsData();
            m_studentsData.Add(newStudentData);
            m_fields[m_studentsData.Count - 1].gameObject.SetActive(true);
            if (m_studentsData.Count >= 8)
            {
                addNewFieldButton.SetActive(false);
            }
        }
    }

    public void DeleteField(StudentFields studentField)
    {
        int fieldIndex = m_fields.IndexOf(studentField);
        CleanField(studentField);
        m_fields[fieldIndex].gameObject.SetActive(false);
        m_fields[fieldIndex].transform.SetSiblingIndex(7);
        m_fields.Remove(studentField);
        m_fields.Add(studentField);
        m_studentsData.Remove(m_studentsData[fieldIndex]);
        addNewFieldButton.SetActive(true);
    }

    public void OpenChangeNoteFormat()
    {
        winConditionText.text = $"{winCondition}";
        maxPosibleNoteText.text = $"{maxPosibleNote}";
        minPosibleNoteText.text = $"{minPosibleNote}";

        for (int i = 0; i < m_studentsData.Count; i++)
        {
            AddToStudentsData(i);
        }
    }

    public void ChangeNoteFormat()
    {
        for (int i = 0; i < m_studentsData.Count; i++)
        {
            float notePercentage = ((m_studentsData[i].finalNote * 100) / maxPosibleNote);
            m_studentsData[i].finalNote = (int.Parse(maxPosibleNoteText.text) * notePercentage) / 100;
            AddToFields(i);
        }

        winCondition = int.Parse(winConditionText.text);
        maxPosibleNote = int.Parse(maxPosibleNoteText.text);
        minPosibleNote = int.Parse(minPosibleNoteText.text);
    }

    public void AdaptWinCondition()
    {
        float winPercentage = (int.Parse(winConditionText.text) * 100) / maxPosibleNote;
        int maxNote = int.Parse(maxPosibleNoteText.text);
        winConditionText.text = $"{(maxNote * winPercentage) / 100}";
    }

    public void TryToModify()
    {
        if (alreadyQualified)
        {
            alreadyQualifiedPanel.SetActive(true);
        }
    }

    public void ModifyAgain()
    {
        alreadyQualified = false;
    }

    public void Exit()
    {
        for (int i = 0; i < m_studentsData.Count; i++)
        {
            AddToStudentsData(i);
        }
        SaveData();
        Application.Quit();
    }

    public void NextScene()
    {
        manager.Activate();
    }

    public void AssignFields()
    {
        for (int i = 0; i < m_studentsData.Count; i++)
        {
            if (i < m_fields.Count)
            {
                AddToFields(i);
                m_fields[i].gameObject.SetActive(true);
            }
        }
    }

    public void AddToFields(int index)
    {
        m_fields[index].firstName.text = $"{m_studentsData[index].firstName}";
        m_fields[index].lastName.text = $"{m_studentsData[index].lastName}";
        m_fields[index].idCode.text = $"{m_studentsData[index].idCode}";
        m_fields[index].email.text = $"{m_studentsData[index].email}";
        m_fields[index].finalNote.text = $"{m_studentsData[index].finalNote}";
        m_fields[index].isAproved.value = m_studentsData[index].isAproved;
    }

    public void AddToStudentsData(int index)
    {
        m_studentsData[index].firstName = m_fields[index].firstName.text;
        m_studentsData[index].lastName = m_fields[index].lastName.text;
        m_studentsData[index].idCode = int.Parse(m_fields[index].idCode.text);
        m_studentsData[index].email = m_fields[index].email.text;
        m_studentsData[index].finalNote = float.Parse(m_fields[index].finalNote.text);
        m_fields[index].isAproved = m_fields[index].isAproved;
    }

    private void CleanField(StudentFields studentField)
    {
        studentField.firstName.text = "";
        studentField.lastName.text = "";
        studentField.idCode.text = "";
        studentField.email.text = "";
        studentField.finalNote.text = "";
        studentField.isAproved.value = 0;
    }

    public void AssignIsAproved(StudentFields studentFields)
    {
        m_studentsData[m_fields.IndexOf(studentFields)].isAproved = studentFields.isAproved.value;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }*/
    }
}

[System.Serializable]
public class JsonData
{
    public List<StudentsData> studentsData;
}

[System.Serializable]
public class StudentsData
{
    public string firstName;
    public string lastName;
    public int idCode;
    public string email;
    public float finalNote;
    public int isAproved;
}