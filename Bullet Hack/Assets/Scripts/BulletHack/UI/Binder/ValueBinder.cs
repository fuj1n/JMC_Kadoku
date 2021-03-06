﻿using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI.Binder
{
    [AddComponentMenu("")]
    public class ValueBinder : MonoBehaviour
    {
        public FieldInfo field;
        public object obj;

        public Selectable sync;

        private void Start()
        {
            OnRegister();
        }

        protected virtual void OnRegister()
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();

            inputField.text = field.GetValue(obj)?.ToString();

            if (field.FieldType == typeof(int))
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;

            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(s =>
            {
                if (field.FieldType == typeof(int))
                {
                    int.TryParse(s, out int current);
                    field.SetValue(obj, current);
                }
                else if (field.FieldType == typeof(string))
                {
                    field.SetValue(obj, s);
                }
            });

            sync = inputField;
        }

        private void Update()
        {
            if (sync)
                sync.interactable = !CombatManager.Instance.Script.IsRunning;
        }
    }
}