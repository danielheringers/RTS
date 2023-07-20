using UnityEngine;
using UnityEngine.UI;


public class UnitInfoUI : MonoBehaviour
{
    public Text levelText;
    public Text experienceText;
    public UnitData unitData;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Verifica se o script UnitData est� anexado ao objeto unitData
        if (unitData != null)
        {
            // Atualiza os valores de texto com as informa��es do unitData
            levelText.text = "Level: " + unitData.currentLevel.ToString();
            experienceText.text = "Experience: " + unitData.currentExperience.ToString();
        }
        else
        {
            // Caso o unitData n�o esteja atribu�do, limpa os valores de texto
            levelText.text = "Level: -";
            experienceText.text = "Experience: -";
        }
    }
}
