using UnityEngine;
using UnityEditor;

public class BaseCustomEditor<T> : BaseEditor where T: Object
{
    protected T current;

    private void OnEnable()
    {
        current = target as T;
    }
}