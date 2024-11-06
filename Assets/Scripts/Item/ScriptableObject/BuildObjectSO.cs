using UnityEngine;

[CreateAssetMenu(fileName = "BuildObjectSO", menuName = "BuildObjectSO", order = 4)]
public class BuildObjectSO : ScriptableObject
{
    public enum BuildType
    {
        Foundation,
        Wall,
        Ground
    }

    public ItemIdentifier identifier;
    public BuildType buildType;
    public PreviewObject Preview;
    public GameObject Building;
}
