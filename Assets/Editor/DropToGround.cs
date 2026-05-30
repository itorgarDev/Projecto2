using UnityEngine;
using UnityEditor;
public class DropToGround : EditorWindow
{
    [MenuItem("Tools/Drop To Ground")]
    static void DropSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RaycastHit hit;

            // Lanzamos un rayo desde MUY arriba hacia abajo
            Vector3 start = obj.transform.position + Vector3.up * 1000f;

            if (Physics.Raycast(start, Vector3.down, out hit, Mathf.Infinity))
            {
                obj.transform.position = hit.point;
            }
        }
    }
}
