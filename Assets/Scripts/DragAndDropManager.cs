using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DragAndDropManager : MonoBehaviour
{
    [SerializeField] private JsonManager jsonManager;
    [SerializeField] private GameObject firstScene;
    [SerializeField] private GameObject secondScene;
    [SerializeField] private GameObject advicePanel;
    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject waitListPanel;
    [SerializeField] private TextMeshProUGUI reprovedText;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public GameObject actualItem;

    [SerializeField] private List<StudentFieldDnD> m_studentFieldDnDs = new List<StudentFieldDnD>();
    private List<StudentFieldDnD> m_studentFieldDnDsActive = new List<StudentFieldDnD>();

    public void Activate()
    {
        for (int i = 0; i < jsonManager.m_studentsData.Count; i++)
        {
            m_studentFieldDnDs[i].names.text = $"{jsonManager.m_studentsData[i].firstName} {jsonManager.m_studentsData[i].lastName}";
            if (jsonManager.m_studentsData[i].isAproved == 1)
                m_studentFieldDnDs[i].isAproved = true;
            else
                m_studentFieldDnDs[i].isAproved = false;
            m_studentFieldDnDs[i].note = jsonManager.m_studentsData[i].finalNote;
            m_studentFieldDnDs[i].gameObject.SetActive(true);
            m_studentFieldDnDsActive.Add(m_studentFieldDnDs[i]);
        }
        firstScene.SetActive(false);
        secondScene.SetActive(true);
    }

    public void CheckDnD()
    {
        reprovedText.text = "";
        for (int i = 0; i < m_studentFieldDnDsActive.Count; i++)
        {
            if (m_studentFieldDnDsActive[i].transform.parent.name == "Reproved" && m_studentFieldDnDsActive[i].isAproved)
            {
                reprovedText.text = $"{reprovedText.text}{m_studentFieldDnDsActive[i].names.text}\n";
            }
            else if (m_studentFieldDnDsActive[i].transform.parent.name == "Aproved" && !m_studentFieldDnDsActive[i].isAproved)
            {
                reprovedText.text = $"{reprovedText.text}{m_studentFieldDnDsActive[i].names.text}\n";
            }
        }
        if (reprovedText.text == "")
        {
            correctPanel.SetActive(true);
        }
        else
        {
            advicePanel.SetActive(true);
        }
    }

    public void Finish()
    {
        jsonManager.alreadyQualified = true;
    }

    public void ResetDnD()
    {
        for (int i = 0; i < m_studentFieldDnDs.Count; i++)
        {
            m_studentFieldDnDs[i].state.color = Color.white;
            m_studentFieldDnDs[i].transform.SetParent(waitListPanel.transform);
        }
    }
}
