using TMPro;
using UnityEngine;

public class TextLogEntry
{
    public TextMeshProUGUI TextMesh { get; set; }
    public int PositionIndex { get; set; }
    public Color TextColor { get; set; }
   

    public TextLogEntry(TextMeshProUGUI textMesh, int index, Color color) 
    {
        this.TextMesh = textMesh;
        this.PositionIndex = index;
        this.TextMesh.color = color;
    }

    public void SetText(string text) => TextMesh.text = text;
    public void MoveToIndex(int index) 
    {
        PositionIndex = index;
        TextMesh.transform.SetSiblingIndex(index);
    }
    public void Destroy() 
    {
       GameObject.Destroy(TextMesh.gameObject);
    } 
}
