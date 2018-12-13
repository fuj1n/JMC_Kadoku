using TMPro;
using UnityEngine;

public class SerializeDeserializeController : MonoBehaviour
{
    public RectTransform root;

    public BlockManagerBase startBlock;

    public GameObject codeInOutPanel;
    public TMP_InputField codeInOutField;

    public void Import()
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

        codeInOutField.text = "";
        codeInOutPanel.SetActive(true);
    }

    public void Export()
    {
        codeInOutField.text = SerializedBlock.Serialize(startBlock.GetOutConnection());
        codeInOutPanel.SetActive(true);
    }

    public void Import_DO()
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

        BlockManagerBase man = SerializedBlock.Deserialize(codeInOutField.text, root);
        if (man)
            man.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        Close();
    }

    public void Close()
    {
        codeInOutPanel.SetActive(false);
    }
}
