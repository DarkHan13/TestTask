using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectGrabber : MonoBehaviour
{

    [SerializeField] private float grabDistance = 5f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Transform camT;

    public event Action OnDebugMode;
    
    private Rigidbody _grabbedObjectRb;
    
    // debug
    private bool _debugMode;

    private bool DebugMode
    {
        get => _debugMode;
        set
        {
            if (!value)
            {
                if (_lines != null)
                {
                    foreach (var line in _lines)
                    {
                        Destroy(line.gameObject);
                    }
                }
                _objectsToGrab = null;
                _lines = null;
            }
            else
            {
                OnDebugMode?.Invoke();
            }
            _debugMode = value;
        }
    }
    
    private GameObject[] _objectsToGrab;
    private LineRenderer[] _lines;

    private void Reset()
    {
        camT = Camera.main.transform;
    }

    private void Update()
    {
        bool hasObject = _grabbedObjectRb != null;
        if (Input.GetMouseButtonDown(0) && !hasObject)
        {
            GrabObject();
        }

        if (Input.GetMouseButtonUp(0) && hasObject)
        {
            ReleaseObject();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DebugMode = !DebugMode;
        }
        
        if (_debugMode) ShowObjects();
    }

    private void FixedUpdate()
    {

        if (_grabbedObjectRb != null)
        {
            MoveObject();
        }        
    }

    private void GrabObject()
    {
        Ray ray = new Ray(camT.position, camT.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, grabDistance)) return;
        if (!hit.collider.TryGetComponent(out Rigidbody rb)) return;
        _grabbedObjectRb = rb;
        _grabbedObjectRb.useGravity = false;
        _grabbedObjectRb.drag = 10;
        DebugMode = false;
    }

    private void ReleaseObject()
    {
        _grabbedObjectRb.useGravity = true;
        _grabbedObjectRb.drag = 1;
        _grabbedObjectRb = null;
    }

    private void MoveObject()
    {
        Vector3 targetPosition = camT.position + camT.forward * 1f;
        var dir = (targetPosition - _grabbedObjectRb.position);
        _grabbedObjectRb.velocity = dir * (smoothSpeed/* * Time.fixedDeltaTime*/);
    }
    
    private void ShowObjects()
    {
        if (_objectsToGrab == null)
        {
            _objectsToGrab = GameObject.FindGameObjectsWithTag("Object");
            _lines = new LineRenderer[_objectsToGrab.Length];
            for (var i = 0; i < _lines.Length; i++)
            {
                _lines[i] = new GameObject("Line").AddComponent<LineRenderer>();
                _lines[i].startWidth = 0.05f;
                _lines[i].endWidth = 0.05f;
            }
        }

        for (var i = 0; i < _lines.Length; i++)
        {
            _lines[i].positionCount = 2;
            _lines[i].SetPositions(new []{ transform.position, _objectsToGrab[i].transform.position});
        }
    }
}
