using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI.Binder
{
    [AddComponentMenu("")]
    public class BoolBinder : ValueBinder
    {
        protected override void OnRegister()
        {
            Toggle toggle = GetComponent<Toggle>();
            toggle.isOn = (bool) field.GetValue(obj);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(
                    b => field.SetValue(obj, b)
            );

            sync = toggle;
        }
    }
}