using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ArrayTool : EditorWindow
{
    private Vector3 _axis;
    private bool _clearPrevious = true;

    private int _count;
    private bool _group;

    private List<GameObject> _objs;
    private Transform _parent;
    private bool _rotateToSnapNormal;
    private Vector3 _rotationEuler;
    private GameObject[] _selection;
    private Transform _selectionTransform;
    private bool _snapToGround;
    private float _spacing;
    private bool _useBounds;

    private bool transformChanged = false;

    [MenuItem("Window/ArrayTool")]
    private static void Create()
    {
        ArrayTool window = (ArrayTool) GetWindow(typeof(ArrayTool));
        window.Show();
    }

    // Use this for initialization
    private void Awake()
    {
        _objs = new List<GameObject>();
    }

    private void Update()
    {
        if (Selection.gameObjects.Length > 0 && _objs.Count > 0 &&
            (Selection.gameObjects[0].transform != _selectionTransform || Selection.gameObjects[0].transform.hasChanged))
        {
            _selectionTransform = Selection.gameObjects[0].transform;
            GenerateArray(Selection.gameObjects);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _clearPrevious = EditorGUILayout.Toggle("Delete Last: ", _clearPrevious);

        EditorGUI.BeginChangeCheck();
        _axis = EditorGUILayout.Vector3Field("Direction: ", _axis);
        _rotationEuler = EditorGUILayout.Vector3Field("Rotation: ", _rotationEuler);

        EditorGUILayout.Separator();

        _count = EditorGUILayout.IntField("Amount: ", _count > 0 ? _count : 0);
        _spacing = EditorGUILayout.FloatField("Spacing: ", _spacing);

        EditorGUILayout.Separator();

        _useBounds = EditorGUILayout.Toggle("Use bounds: ", _useBounds);
        _group = EditorGUILayout.Toggle("Group: ", _group);
        EditorGUILayout.BeginHorizontal();
        _snapToGround =
            EditorGUILayout.Toggle(
                new GUIContent("Snap to ground", "Do racast in negative y-Axis and snap to first hit normal"),
                _snapToGround);
        _rotateToSnapNormal =
            EditorGUILayout.Toggle(
                new GUIContent("Rotate to hit normal", "Aligns close to hit normal of raycast"), _rotateToSnapNormal);

        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck() && _count > 0 && Selection.gameObjects.Length > 0)
            GenerateArray(Selection.gameObjects);

        if (GUILayout.Button("Delete Array"))
        {
            foreach (GameObject o in _objs) DestroyImmediate(o);
            _objs.Clear();

            if (_parent != null) DestroyImmediate(_parent.gameObject);
        }

        if (GUILayout.Button("Commit Array"))
        {
            _objs.Clear();
            _parent = null;
        }

        EditorGUILayout.EndVertical();
    }

    private void GenerateArray(GameObject[] _selection)
    {
        if (_clearPrevious)
            foreach (GameObject o in _objs)
                if (!_selection.Contains(o)) DestroyImmediate(o);
        _objs.Clear();

        if (!_group && _parent != null)
            DestroyImmediate(_parent.gameObject);

        if ((_parent == null || _selection.Contains(_parent.gameObject)) && _group)
        {
            _parent = new GameObject(_selection[0].name + "_Array").transform;
            _parent.position = _selection[0].transform.position;
        }

        foreach (GameObject selected in _selection)
            for (int i = 1; i < _count + 1; i++)
            {
                Vector3 pos = selected.transform.position + _axis * _spacing * i;
                if (_useBounds)
                    if (selected.GetComponent<MeshRenderer>())
                    {
                        Bounds b = selected.GetComponent<MeshRenderer>().bounds;
                        float offset = 0;
                        if (Math.Abs(_axis.x) > 0)
                            offset = b.extents.x;
                        if (Math.Abs(_axis.y) > 0)
                            offset = b.extents.y;
                        if (Math.Abs(_axis.z) > 0)
                            offset = b.extents.z;

                        pos += i * offset * 2 * _axis;
                    }
                GameObject arrayClone = Instantiate(selected, pos, selected.transform.rotation * Quaternion.Euler(
                                                                       new Vector3(_rotationEuler.x * i,
                                                                           _rotationEuler.y * i,
                                                                           _rotationEuler.z * i)));

                if (_snapToGround)
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(arrayClone.transform.position, -Vector3.up, out hitInfo, Mathf.Infinity) &&
                        arrayClone.GetComponent<MeshRenderer>())
                    {
                        arrayClone.transform.position = hitInfo.point +
                                                        hitInfo.normal.normalized *
                                                        arrayClone.GetComponent<MeshRenderer>().bounds.extents.y;
                        if (_rotateToSnapNormal)
                        {
                            arrayClone.transform.LookAt(hitInfo.point + hitInfo.normal * 5);
                            arrayClone.transform.rotation *= Quaternion.Euler(90, 0, 0);
                            // = Quaternion.LookRotation(arrayClone.transform.right, hitInfo.normal);
                        }
                    }
                }

                if (_group)
                    arrayClone.transform.parent = _parent;

                _objs.Add(arrayClone);
            }
    }
}