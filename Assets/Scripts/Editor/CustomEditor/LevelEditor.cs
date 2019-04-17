using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : BaseCustomEditor<Level>
{
    

    private void OnSceneGUI()
    {
        Event ev = Event.current;
        Vector2 position = ev.mousePosition;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(position);
        int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlId);

        switch (ev.GetTypeForControl(controlId))
        {
            case EventType.MouseDown:
                if(ev.button == 0)
                {
                    current.Dig(mouseRay.origin);
                    ev.Use();
                }
                break;

            case EventType.Repaint:
                Handles.CircleHandleCap(controlId, mouseRay.origin, Quaternion.identity, 1f, EventType.Repaint);
                break;
        }
    }
}