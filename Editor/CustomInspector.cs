#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace lilToon
{
    public class lilCustom_PhaseShiftShaderInspector : lilToonInspector
    {
        // Custom properties
        //MaterialProperty customVariable;
        MaterialProperty PS_MirageColloidContrast;
        MaterialProperty PS_MirageColloidHSVG;
        MaterialProperty PS_MirageColloidDissolveBorder;
        MaterialProperty PS_MirageColloidDissolveBlur;
        MaterialProperty PS_MirageColloidDissolveMask;
        MaterialProperty PS_MirageColloidDissolveMask_ST;
        MaterialProperty PS_MirageColloidDissolveNoiseMask;
        MaterialProperty PS_MirageColloidDissolveNoiseMask_ST;
        MaterialProperty PS_MirageColloidDissolveNoiseMaskAnim;
        MaterialProperty PS_MirageColloidDissolveNoiseMaskStrength;
        MaterialProperty PS_MirageColloidDissolveColor;

        private static bool isShowCustomProperties = true;
        private static bool isShowDissolveNoiseMaskProps;
        private const string shaderName = "lilCustom_PhaseShiftShader";

        protected override void LoadCustomProperties(MaterialProperty[] props, Material material)
        {
            isCustomShader = true;

            ReplaceToCustomShaders();
            isShowRenderMode = !material.shader.name.Contains("Optional");

            LoadCustomLanguage("42484cf85d197b14ca988de0cf622b97");
            PS_MirageColloidContrast = FindProperty("_PS_MirageColloidContrast", props);
            PS_MirageColloidHSVG = FindProperty("_PS_MirageColloidHSVG", props);
            PS_MirageColloidDissolveBorder = FindProperty("_PS_MirageColloidDissolveBorder", props);
            PS_MirageColloidDissolveBlur = FindProperty("_PS_MirageColloidDissolveBlur", props);
            PS_MirageColloidDissolveMask = FindProperty("_PS_MirageColloidDissolveMask", props);
            PS_MirageColloidDissolveMask_ST = FindProperty("_PS_MirageColloidDissolveMask_ST", props);
            PS_MirageColloidDissolveNoiseMask = FindProperty("_PS_MirageColloidDissolveNoiseMask", props);
            PS_MirageColloidDissolveNoiseMask_ST = FindProperty("_PS_MirageColloidDissolveNoiseMask_ST", props);
            PS_MirageColloidDissolveNoiseMaskAnim = FindProperty("_PS_MirageColloidDissolveNoiseMaskAnim", props);
            PS_MirageColloidDissolveNoiseMaskStrength = FindProperty("_PS_MirageColloidDissolveNoiseMaskStrength", props);
            PS_MirageColloidDissolveColor = FindProperty("_PS_MirageColloidDissolveColor", props);
        }

        protected override void DrawCustomProperties(Material material)
        {
            // GUIStyles Name   Description
            // ---------------- ------------------------------------
            // boxOuter         outer box
            // boxInnerHalf     inner box
            // boxInner         inner box without label
            // customBox        box (similar to unity default box)
            // customToggleFont label for box

            isShowCustomProperties = Foldout(GetLoc("sPSShaderSettingsTitle"), GetLoc("sPSShaderSettingsTitleHelp"), isShowCustomProperties);
            if(isShowCustomProperties)
            {
                EditorGUILayout.BeginVertical(boxOuter);
                EditorGUILayout.LabelField(GetLoc("sPSShaderMirageColloidSettings"), customToggleFont);
                EditorGUILayout.BeginVertical(boxInnerHalf);

                m_MaterialEditor.ShaderProperty(PS_MirageColloidContrast, GetLoc("sPSShaderMirageColloidContrast"));
                DrawLine();
                EditorGUILayout.LabelField("HSV/Gamma", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                m_MaterialEditor.ShaderProperty(PS_MirageColloidHSVG, "Hue|Saturation|Value|Gamma");
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                
                DrawLine();

                EditorGUILayout.LabelField("Dissolves", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                m_MaterialEditor.ShaderProperty(PS_MirageColloidDissolveBorder, "Dissolve Border");
                m_MaterialEditor.ShaderProperty(PS_MirageColloidDissolveBlur, "Dissolve Blur");
                EditorGUILayout.Space();

                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Dissolve Mask"), PS_MirageColloidDissolveMask);
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;
                m_MaterialEditor.TextureScaleOffsetProperty(PS_MirageColloidDissolveMask);
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Dissolve Noise"), PS_MirageColloidDissolveNoiseMask, PS_MirageColloidDissolveNoiseMaskStrength);
                EditorGUI.indentLevel++;
                EditorGUI.indentLevel++;
                m_MaterialEditor.TextureScaleOffsetProperty(PS_MirageColloidDissolveNoiseMask);
                m_MaterialEditor.ShaderProperty(PS_MirageColloidDissolveNoiseMaskAnim, "Animate");
                m_MaterialEditor.ShaderProperty(PS_MirageColloidDissolveColor, "Dissolve Color");
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                
                EditorGUI.indentLevel--;

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
        }

        protected override void ReplaceToCustomShaders()
        {
            lts         = Shader.Find(shaderName + "/lilToon");
            ltsc        = Shader.Find("Hidden/" + shaderName + "/Cutout");
            ltst        = Shader.Find("Hidden/" + shaderName + "/Transparent");
            ltsot       = Shader.Find("Hidden/" + shaderName + "/OnePassTransparent");
            ltstt       = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparent");

            ltso        = Shader.Find("Hidden/" + shaderName + "/OpaqueOutline");
            ltsco       = Shader.Find("Hidden/" + shaderName + "/CutoutOutline");
            ltsto       = Shader.Find("Hidden/" + shaderName + "/TransparentOutline");
            ltsoto      = Shader.Find("Hidden/" + shaderName + "/OnePassTransparentOutline");
            ltstto      = Shader.Find("Hidden/" + shaderName + "/TwoPassTransparentOutline");

            ltsoo       = Shader.Find(shaderName + "/[Optional] OutlineOnly/Opaque");
            ltscoo      = Shader.Find(shaderName + "/[Optional] OutlineOnly/Cutout");
            ltstoo      = Shader.Find(shaderName + "/[Optional] OutlineOnly/Transparent");

            ltstess     = Shader.Find("Hidden/" + shaderName + "/Tessellation/Opaque");
            ltstessc    = Shader.Find("Hidden/" + shaderName + "/Tessellation/Cutout");
            ltstesst    = Shader.Find("Hidden/" + shaderName + "/Tessellation/Transparent");
            ltstessot   = Shader.Find("Hidden/" + shaderName + "/Tessellation/OnePassTransparent");
            ltstesstt   = Shader.Find("Hidden/" + shaderName + "/Tessellation/TwoPassTransparent");

            ltstesso    = Shader.Find("Hidden/" + shaderName + "/Tessellation/OpaqueOutline");
            ltstessco   = Shader.Find("Hidden/" + shaderName + "/Tessellation/CutoutOutline");
            ltstessto   = Shader.Find("Hidden/" + shaderName + "/Tessellation/TransparentOutline");
            ltstessoto  = Shader.Find("Hidden/" + shaderName + "/Tessellation/OnePassTransparentOutline");
            ltstesstto  = Shader.Find("Hidden/" + shaderName + "/Tessellation/TwoPassTransparentOutline");

            ltsl        = Shader.Find(shaderName + "/lilToonLite");
            ltslc       = Shader.Find("Hidden/" + shaderName + "/Lite/Cutout");
            ltslt       = Shader.Find("Hidden/" + shaderName + "/Lite/Transparent");
            ltslot      = Shader.Find("Hidden/" + shaderName + "/Lite/OnePassTransparent");
            ltsltt      = Shader.Find("Hidden/" + shaderName + "/Lite/TwoPassTransparent");

            ltslo       = Shader.Find("Hidden/" + shaderName + "/Lite/OpaqueOutline");
            ltslco      = Shader.Find("Hidden/" + shaderName + "/Lite/CutoutOutline");
            ltslto      = Shader.Find("Hidden/" + shaderName + "/Lite/TransparentOutline");
            ltsloto     = Shader.Find("Hidden/" + shaderName + "/Lite/OnePassTransparentOutline");
            ltsltto     = Shader.Find("Hidden/" + shaderName + "/Lite/TwoPassTransparentOutline");

            ltsref      = Shader.Find("Hidden/" + shaderName + "/Refraction");
            ltsrefb     = Shader.Find("Hidden/" + shaderName + "/RefractionBlur");
            ltsfur      = Shader.Find("Hidden/" + shaderName + "/Fur");
            ltsfurc     = Shader.Find("Hidden/" + shaderName + "/FurCutout");
            ltsfurtwo   = Shader.Find("Hidden/" + shaderName + "/FurTwoPass");
            ltsfuro     = Shader.Find(shaderName + "/[Optional] FurOnly/Transparent");
            ltsfuroc    = Shader.Find(shaderName + "/[Optional] FurOnly/Cutout");
            ltsfurotwo  = Shader.Find(shaderName + "/[Optional] FurOnly/TwoPass");
            ltsgem      = Shader.Find("Hidden/" + shaderName + "/Gem");
            ltsfs       = Shader.Find(shaderName + "/[Optional] FakeShadow");

            ltsover     = Shader.Find(shaderName + "/[Optional] Overlay");
            ltsoover    = Shader.Find(shaderName + "/[Optional] OverlayOnePass");
            ltslover    = Shader.Find(shaderName + "/[Optional] LiteOverlay");
            ltsloover   = Shader.Find(shaderName + "/[Optional] LiteOverlayOnePass");

            ltsm        = Shader.Find(shaderName + "/lilToonMulti");
            ltsmo       = Shader.Find("Hidden/" + shaderName + "/MultiOutline");
            ltsmref     = Shader.Find("Hidden/" + shaderName + "/MultiRefraction");
            ltsmfur     = Shader.Find("Hidden/" + shaderName + "/MultiFur");
            ltsmgem     = Shader.Find("Hidden/" + shaderName + "/MultiGem");
        }
        
        // You can create a menu like this
        [MenuItem("Assets/lilCustom_PhaseShiftShader/Convert material to custom shader", false, 1100)]
        private static void ConvertMaterialToCustomShaderMenu()
        {
            if(Selection.objects.Length == 0) return;
            lilCustom_PhaseShiftShaderInspector inspector = new lilCustom_PhaseShiftShaderInspector();
            for(int i = 0; i < Selection.objects.Length; i++)
            {
                if(Selection.objects[i] is Material)
                {
                    inspector.ConvertMaterialToCustomShader((Material)Selection.objects[i]);
                }
            }
        }
    }
}
#endif
