using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text levelText;
    public TMP_Text experiencePercentageText;
    public TMP_Text experienceText;
    public TMP_Text attackDamageText;
    public TMP_Text armorText;
    public TMP_Text attackSpeedText;
    public TMP_Text moveSpeedText;
    public Camera mainCamera;
    public LayerMask unitLayer;
    private UnitController selectedUnit;

    public GameObject unitStatsUI;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                selectedUnit = hit.collider.GetComponent<UnitController>();
            }
            else
            {
                unitStatsUI.SetActive(false);
            }
        }

        if (selectedUnit != null)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (selectedUnit.unitStats != null)
        {
            unitStatsUI.SetActive(true);
            // Atualiza os valores de texto com as informações do unitStats
            levelText.text = selectedUnit.unitStats.currentLevel.ToString();
            experienceText.text = selectedUnit.unitStats.currentExperience.ToString();
            attackDamageText.text = selectedUnit.unitStats.attackDamage.ToString();
            attackSpeedText.text = selectedUnit.unitStats.attackSpeed.ToString("F2") + "/Sec";
            armorText.text = selectedUnit.unitStats.armor.ToString();
            moveSpeedText.text = selectedUnit.unitStats.moveSpeed.ToString();
            float currentHP = selectedUnit.currentHealth;
            float maxHP = selectedUnit.unitStats.maxHealth;
            healthSlider.value = currentHP / maxHP;

            // Calcula a porcentagem de experiência atual em relação à experiência necessária para o próximo nível
            float currentExp = selectedUnit.unitStats.currentExperience;
            float expToNextLevel = selectedUnit.unitStats.experienceToNextLevel;
            float percentage = (currentExp / expToNextLevel) * 100;

            // Mostra a porcentagem no texto correspondente
            experiencePercentageText.text = percentage.ToString("F2") + "%";
        }
        else
        {
            unitStatsUI.SetActive(false);
        }
    }
}
