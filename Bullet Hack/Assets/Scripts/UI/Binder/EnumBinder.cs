using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[AddComponentMenu("")]
public class EnumBinder : ValueBinder
{
    public List<object> values;

    protected override void OnRegister()
    {
        string[] names = System.Enum.GetNames(field.FieldType);
        values = System.Enum.GetValues(field.FieldType).Cast<object>().ToList();

        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(names.Select(x => x.ToFriendly()).ToList());
        dropdown.value = values.IndexOf(field.GetValue(obj));
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(newVal =>
            field.SetValue(obj, values[newVal])
        );

        sync = dropdown;
    }
}
