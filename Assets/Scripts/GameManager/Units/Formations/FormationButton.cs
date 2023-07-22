using UnityEngine;
using UnityEngine.UI;

public class FormationButton : MonoBehaviour
{
    public Button button;
    public UnitSelection unitSelection;

    private void Start()
    {
        button.onClick.AddListener(OnClickFormationButton);
    }

    private void OnClickFormationButton()
    {
        unitSelection.SetUnitsInFormation();
    }
}
