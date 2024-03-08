using UnityEngine;
using System.Collections;

public class FPSShaderColorGradient : MonoBehaviour
{
    public RFX4_ShaderProperties ShaderColorProperty = RFX4_ShaderProperties._TintColor;
    public Gradient Color = new Gradient();
    public float TimeMultiplier = 1;
    public bool IsLoop;

    [HideInInspector]
    public bool canUpdate;
    //private Material mat;
    private int propertyID;
    private float startTime;
    private Color startColor;

    private bool isInitialized;
    private string shaderProperty;

    private MaterialPropertyBlock props;
    private Renderer rend;

    void Awake()
    {
        if (props == null) props = new MaterialPropertyBlock();
        if (rend == null) rend = GetComponent<Renderer>();

        shaderProperty = ShaderColorProperty.ToString();
        propertyID = Shader.PropertyToID(shaderProperty);
        startColor = rend.sharedMaterial.GetColor(propertyID);
    }


    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;

        rend.GetPropertyBlock(props);

        startColor = rend.sharedMaterial.GetColor(propertyID);
        props.SetColor(propertyID, startColor * Color.Evaluate(0));

        rend.SetPropertyBlock(props);
    }

    private void Update()
    {
		if (Application.isBatchMode == true)
			return;

        rend.GetPropertyBlock(props);

        var time = Time.time - startTime;
        if (canUpdate)
        {
            var eval = Color.Evaluate(time / TimeMultiplier);
            props.SetColor(propertyID, eval * startColor);
        }
        if (time >= TimeMultiplier)
        {
            if (IsLoop) startTime = Time.time;
            else canUpdate = false;
        }

        rend.SetPropertyBlock(props);
    }

    public enum RFX4_ShaderProperties
    {
        _TintColor,
        _Cutoff,
        _Color,
        _EmissionColor,
        _MaskPow,
        _Cutout,
        _Speed,
        _BumpAmt,
        _MainColor,
        _Distortion,
        _FresnelColor
    }
}
