using UnityEngine;
using UnityEngine.Rendering;

#if KRIPTO_FX_LWRP_RENDERING
using UnityEngine.Experimental.Rendering.LightweightPipeline;
#endif

[ExecuteInEditMode]
public class FPS_Decal : MonoBehaviour
{
    public bool ScreenSpaceDecals = true;
    public float randomScalePercent = 50;
    private MaterialPropertyBlock props;
    MeshRenderer rend;
    private Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;
    }


    private void OnEnable()
    {
        var meshRend = GetComponent<MeshRenderer>();
        if (meshRend != null)
        {
            meshRend.reflectionProbeUsage = ReflectionProbeUsage.Off;
            meshRend.shadowCastingMode = ShadowCastingMode.Off;
            if (ScreenSpaceDecals)
            {
                meshRend.sharedMaterial.DisableKeyword("USE_QUAD_DECAL");
                meshRend.sharedMaterial.SetInt("_ZTest1", (int)UnityEngine.Rendering.CompareFunction.Greater);
            }
            else
            {
                meshRend.sharedMaterial.EnableKeyword("USE_QUAD_DECAL");
                meshRend.sharedMaterial.SetInt("_ZTest1", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
            }
        }
        if (Application.isPlaying)
        {
            transform.localRotation = Quaternion.Euler(Random.Range(0, 360), 90, 90);
            var randomScaleRange = Random.Range(startScale.x - startScale.x * randomScalePercent * 0.01f,
                startScale.x + startScale.x * randomScalePercent * 0.01f);
            transform.localScale = new Vector3(randomScaleRange, ScreenSpaceDecals ? startScale.y : 0.001f, randomScaleRange);
        }

        if (Camera.main.depthTextureMode != DepthTextureMode.Depth) Camera.main.depthTextureMode = DepthTextureMode.Depth;
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.TRS(this.transform.TransformPoint(Vector3.zero), this.transform.rotation, this.transform.lossyScale);
        Gizmos.color = new Color(1, 1, 1, 1);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
