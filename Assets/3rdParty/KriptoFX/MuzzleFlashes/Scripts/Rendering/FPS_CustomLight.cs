using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class FPS_CustomLight : MonoBehaviour
{
	/*
    static int MaxLightsCount = 40;
    Texture2D PointLightAttenuation;
    List<Light> sceneLights;
	*/
    private void Awake()
    {
        Shader.SetGlobalInt("RFX4_LightCount", 0);
        Shader.SetGlobalVectorArray("RFX4_LightPositions", new[] { Vector4.zero });
        Shader.SetGlobalVectorArray("RFX4_LightColors", new[] { Vector4.zero });
        Shader.SetGlobalColor("RFX4_AmbientColor", Color.black);
		/*
        sceneLights = GameObject.FindObjectsOfType<Light>().ToList();
        PointLightAttenuation = GeneratePointAttenuationTexture();
        Shader.SetGlobalTexture("RFX4_PointLightAttenuation", PointLightAttenuation);
        Shader.SetGlobalVectorArray("RFX4_LightPositions", ListToArrayWithMaxCount(null, 40));
        Shader.SetGlobalVectorArray("RFX4_LightColors", ListToArrayWithMaxCount(null, 40));
        */
    }
	/*
    void Update()
    {
		if (Application.isBatchMode == true)
			return;

        //var allLights = GetAllLights();
        var allLights = FindObjectsOfType<Light>().ToList();

        int lightCount = 0;
        var lightPositions = new List<Vector4>();
        var lightColors = new List<Vector4>();

        lightCount += FillDirectionalLights(allLights, lightPositions, lightColors);

        //allLights = SortPointLightsByDistance(allLights);
        lightCount += FillPointLights(allLights, lightPositions, lightColors);

        Shader.SetGlobalInt("RFX4_LightCount", lightCount);
        Shader.SetGlobalVectorArray("RFX4_LightPositions", ListToArrayWithMaxCount(lightPositions, MaxLightsCount));
        Shader.SetGlobalVectorArray("RFX4_LightColors", ListToArrayWithMaxCount(lightColors, MaxLightsCount));

        var ambientColor = SampleLightProbesUp(transform.position, 0.5f);
        Shader.SetGlobalColor("RFX4_AmbientColor", ambientColor);
    }

    void OnDisable()
    {
        Shader.SetGlobalInt("RFX4_LightCount", 0);
        Shader.SetGlobalVectorArray("RFX4_LightPositions", new[] { Vector4.zero });
        Shader.SetGlobalVectorArray("RFX4_LightColors", new[] { Vector4.zero });
        Shader.SetGlobalColor("RFX4_AmbientColor", Color.black);
    }

    List<Light> GetAllLights()
    {
        var allLights = transform.root.GetComponentsInChildren<Light>().ToList();
        foreach (var sceneLight in sceneLights)
        {
            if(sceneLight!=null) allLights.Add(sceneLight);
        }
        return allLights;
    }

    int FillDirectionalLights(List<Light> lights, List<Vector4> lightPositions, List<Vector4> lightColors)
    {
        int lightCount = 0;
        for (int i = 0; i < lights.Count; i++)
        {
            if (!lights[i].isActiveAndEnabled) continue;

            if (lights[i].type == LightType.Directional)
            {
                var pos = (-lights[i].transform.forward);

                lightPositions.Add(new Vector4(pos.x, pos.y, pos.z, 0));
                var color = lights[i].color * lights[i].intensity;
                lightColors.Add(new Vector4(color.r, color.g, color.b, 0));
                lightCount++;
            }
        }
        return lightCount;
    }

    int FillPointLights(List<Light> lights, List<Vector4> lightPositions, List<Vector4> lightColors)
    {
        int lightCount = 0;
        for (int i = 0; i < lights.Count; i++)
        {
            if (!lights[i].isActiveAndEnabled) continue;

            if (lights[i].type == LightType.Point)
            {
                var pos = lights[i].transform.position;

                lightPositions.Add(new Vector4(pos.x, pos.y, pos.z, lights[i].range));
                var color = lights[i].color * lights[i].intensity;
                lightColors.Add(new Vector4(color.r, color.g, color.b, 1));
                lightCount++;
            }
        }
        return lightCount;
    }

    Vector4[] ListToArrayWithMaxCount(List<Vector4> list, int count)
    {
        Vector4[] arr = new Vector4[count];
        for (int i = 0; i < count; i++)
        {
            if (list != null && list.Count > i) arr[i] = list[i];
            else arr[i] = Vector4.zero;
        }
        return arr;
    }

    List<Light> SortPointLightsByDistance(List<Light> lights)
    {
        var pos = transform.position;
        var dict = new SortedDictionary<float, Light>();
        foreach (var customLight in lights)
        {
            float distance = (pos - customLight.transform.position).magnitude + Random.Range(-10000f, 10000f)/1000000;
            if (!dict.ContainsKey(distance)) dict.Add(distance, customLight);
        }

        return dict.Values.ToList();
    }

    public Color SampleLightProbesUp(Vector3 pos, float grayScaleFactor)
    {
        SphericalHarmonicsL2 sh;
        LightProbes.GetInterpolatedProbe(pos, null, out sh);

        var unity_SHAr = new Vector4(sh[0, 3], sh[0, 1], sh[0, 2], sh[0, 0] - sh[0, 6]);
        var unity_SHAg = new Vector4(sh[1, 3], sh[1, 1], sh[1, 2], sh[1, 0] - sh[1, 6]);
        var unity_SHAb = new Vector4(sh[2, 3], sh[2, 1], sh[2, 2], sh[2, 0] - sh[2, 6]);

        var unity_SHBr = new Vector4(sh[0, 4], sh[0, 6], sh[0, 5] * 3, sh[0, 7]);
        var unity_SHBg = new Vector4(sh[1, 4], sh[1, 6], sh[1, 5] * 3, sh[1, 7]);
        var unity_SHBb = new Vector4(sh[2, 4], sh[2, 6], sh[2, 5] * 3, sh[2, 7]);

        var unity_SHC = new Vector3(sh[0, 8], sh[2, 8], sh[1, 8]);

        var norm = new Vector4(0, 1, 0, 1);

        Color colorLinear = Color.black;
        colorLinear.r = Vector4.Dot(unity_SHAr, norm);
        colorLinear.g = Vector4.Dot(unity_SHAg, norm);
        colorLinear.b = Vector4.Dot(unity_SHAb, norm);

        // half4 vB = normal.xyzz * normal.yzzx;
        var normB = new Vector4(norm.x * norm.y, norm.y * norm.z, norm.z * norm.z, norm.z * norm.x);
        Color colorQuad = Color.black;
        colorQuad.r = Vector4.Dot(unity_SHBr, normB);
        colorQuad.g = Vector4.Dot(unity_SHBg, normB);
        colorQuad.b = Vector4.Dot(unity_SHBb, normB);

        float vC = norm.x * norm.x - norm.y * norm.y;
        var finalQuad = unity_SHC * vC;
        Color colorFinalQuad = new Color(finalQuad.x, finalQuad.y, finalQuad.z);
        Color finalColor = colorLinear + colorQuad + colorFinalQuad;
        var grayColor = finalColor.r * 0.33f + finalColor.g * 0.33f + finalColor.b * 0.33f;
        finalColor = Color.Lerp(finalColor, Color.white * grayColor, grayScaleFactor);

        if (QualitySettings.activeColorSpace == ColorSpace.Gamma)
            return (colorLinear + colorQuad + colorFinalQuad).gamma;
        else return finalColor;
    }

    Texture2D GeneratePointAttenuationTexture()
    {
        var tex = new Texture2D(256, 1);
        tex.wrapMode = TextureWrapMode.Clamp;
        for (int i = 0; i < 256; i++)
        {
            float distance = i / 256f;
            var color = Mathf.Clamp01(1.0f / (1.0f + 25.0f * distance * distance) * Mathf.Clamp01((1f - distance) * 5.0f));
            tex.SetPixel(i, 0, Color.white * color);
        }
        tex.Apply();
        return tex;
    }
    */
}
