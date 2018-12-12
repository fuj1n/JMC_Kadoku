using UnityEngine;

public static class CommonResources
{
    public const string COMPONENTS_PATH = "Prefabs/UI/Components/";

    public static readonly GameObject DROPDOWN = Resources.Load<GameObject>(COMPONENTS_PATH + "Dropdown");
    public static readonly GameObject INPUT_FIELD = Resources.Load<GameObject>(COMPONENTS_PATH + "InputField");
    public static readonly GameObject TOGGLE = Resources.Load<GameObject>(COMPONENTS_PATH + "Toggle");
}
