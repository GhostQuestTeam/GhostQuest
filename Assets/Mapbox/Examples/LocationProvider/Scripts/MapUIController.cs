using SkillSystem;
using UnityEngine;
using UnityEngine.UI;

public class MapUIController:MonoBehaviour
{
    private GameObject _attributesPanel;
    private GameObject _skillsButton;
    void Start()
    {
        _attributesPanel = GameObject.Find("AttributesPanel");
        _skillsButton = GameObject.Find("Skills");


        _skillsButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            _attributesPanel.SetActive(true);
            _skillsButton.SetActive(false);
        });

        _attributesPanel.GetComponent<AttributesView>().OnClose += CloseHandler;

        _attributesPanel.SetActive(false);

    }

    private void OnDestroy()
    {
       _attributesPanel.GetComponent<AttributesView>().OnClose -= CloseHandler;
    }

    void CloseHandler()
    {
        _attributesPanel.SetActive(false);
        _skillsButton.SetActive(true);
    }
}