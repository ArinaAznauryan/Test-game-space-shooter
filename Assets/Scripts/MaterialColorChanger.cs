using UnityEngine;

public class MaterialColorChanger : MonoBehaviour
{
    private Renderer _rend;
    private MaterialPropertyBlock propBlock;


    private void Start()
    {
        propBlock = new MaterialPropertyBlock();

    }

    //Setting a materials color dynamically
    public void SetColor(Renderer rend, Color color)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color); 
        rend.SetPropertyBlock(propBlock);
    }
}
