using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AlwaysRenderInFront : MonoBehaviour {

    [HideInInspector()]
    public UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

    private void Start()
    {
		Graphic image = GetComponent<Graphic>();
		Material existingGlobalMat = image.materialForRendering;
		Material updatedMaterial = new Material(existingGlobalMat);
		updatedMaterial.SetInt("unity_GUIZTestMode", (int)comparison);
		image.material = updatedMaterial;
    }
}