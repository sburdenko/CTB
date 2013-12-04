// Version 1.5.3
// �2013 Reindeer Games
// All rights reserved
// Redistribution of source code without permission not allowed

using PrimitivesPro.Editor;
using PrimitivesPro.Primitives;
using UnityEditor;

[CustomEditor(typeof(PrimitivesPro.GameObjects.Capsule))]
public class CreateCapsule : Editor
{
    private bool useFlipNormals = false;

    [MenuItem(MenuDefinition.Capsule)]
    static void Create()
    {
        var obj = PrimitivesPro.GameObjects.Capsule.Create(1.0f, 2.0f, 20, 1, NormalsType.Vertex, PivotPosition.Center);
        obj.SaveStateAll();

        Selection.activeGameObject = obj.gameObject;
    }

    public override void  OnInspectorGUI()
    {
        var obj = Selection.activeGameObject.GetComponent<PrimitivesPro.GameObjects.Capsule>();

        if (target != obj)
        {
            return;
        }

        bool colliderChange = Utils.MeshColliderSelection(obj);

        EditorGUILayout.Separator();

        useFlipNormals = obj.flipNormals;
        bool uiChange = false;

        uiChange |= Utils.SliderEdit("Radius", 0, 100, ref obj.radius);
        uiChange |= Utils.SliderEdit("Height", 0, 100, ref obj.height);

        EditorGUILayout.Separator();

        uiChange |= Utils.SliderEdit("Sides", 4, 100, ref obj.sides);
        uiChange |= Utils.SliderEdit("Height segments", 1, 100, ref obj.heightSegments);

        EditorGUILayout.Separator();

        uiChange |= Utils.NormalsType(ref obj.normalsType);
        uiChange |= Utils.PivotPosition(ref obj.pivotPosition);

        uiChange |= Utils.Toggle("Flip normals", ref useFlipNormals);
        uiChange |= Utils.Toggle("Share material", ref obj.shareMaterial);

        Utils.StatWindow(Selection.activeGameObject);

        Utils.SaveMesh<PrimitivesPro.GameObjects.Capsule>(this);

        Utils.SavePrefab<PrimitivesPro.GameObjects.Capsule>(this);

        Utils.DuplicateObject<PrimitivesPro.GameObjects.Capsule>(this);

        if (uiChange || colliderChange)
        {
            if (obj.generationMode == 0 && !colliderChange)
            {
                obj.GenerateGeometry();

                if (useFlipNormals)
                {
                    obj.FlipNormals();
                }
            }
            else
            {
                obj.GenerateColliderGeometry();
            }
        }
    }
}
