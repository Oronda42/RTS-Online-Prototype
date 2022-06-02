using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MapExtent))]
public class MapExtentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapExtent mapExtent = (MapExtent)target;
        if (GUILayout.Button("Retrive Extent Data"))
        {
            mapExtent.name = "PlayerMapExtent[" + StringToVector.Vec3ToStr(mapExtent.transform.position) + "]";
            mapExtent.RetrieveExtentData();

            foreach (MapElementBase meb in mapExtent.Elements)
            {
                EditorUtility.SetDirty(meb);
            }
        }

        if (GUILayout.Button("Copy SQL to clipboard"))
        {
            mapExtent.RetrieveExtentData();
            EditorGUIUtility.systemCopyBuffer = GenerateSQL();
        }
    }

    void OnDisable()
    {
        MapExtent mapExtent = (MapExtent)target;
        mapExtent.name = "PlayerMapExtent[" + StringToVector.Vec3ToStr(mapExtent.transform.position) + "]";
    }

    void OnEnable()
    {

    }

    public string GenerateSQL()
    {
        MapExtent mapExtent = (MapExtent)target;
        string sqlQueries = @"
        DELETE FROM map_extent WHERE id = " + mapExtent.Model.id + @";
        DELETE FROM map_extent_element WHERE map_extent_id = " + mapExtent.Model.id + ";";

        sqlQueries += string.Format(@"
        INSERT INTO map_extent (id, name, width, depth) 
        VALUES ({0},'{1}',0,0);",
        mapExtent.Model.id,
        mapExtent.Model.name);

        for (int i = 0; i < mapExtent.Model.Elements.Count ; i++)
        {
            sqlQueries += string.Format(@"
            INSERT INTO map_extent_element (map_extent_id, position, map_element_id, entity_id, instance_id) 
            VALUES ({0},'{1}', {2}, {3}, {4});",
            mapExtent.Model.id,
            mapExtent.Model.Elements[i].Position,
            mapExtent.Model.Elements[i].Element.Id,
            mapExtent.Model.Elements[i].EntityId,
            mapExtent.Model.Elements[i].InstanceId);
        }

        return sqlQueries;
        
    }
}