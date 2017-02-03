using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System;

public class HandInteraction : MonoBehaviour, HandInteractionMaster
{

    bool _handTracked = false;
    private GameObject _crs;

    public bool showHandCrsr = true;

    public GameObject CurFocusedObj { get; set; }

    // Use this for initialization
    void Awake()
    {
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
    }

    void Start()
    {
        if (showHandCrsr)
        {
            _crs = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _crs.transform.localScale = new Vector3(0.04F, 0.04F, 0.04F);
            _crs.GetComponent<Renderer>().material.color = Color.green;
            _crs.GetComponent<Collider>().enabled = false;
            _crs.transform.parent = transform;
            _crs.SetActive(false);
        }
    }

    private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }

    private void InteractionManager_SourceUpdated(InteractionSourceState state)
    {
        RaycastHit hitInfo;
        Vector3 handGesture;
        if (state.properties.location.TryGetPosition(out handGesture))
        { 
            Physics.Raycast(Camera.main.transform.position, handGesture, out hitInfo);
            if (hitInfo.collider != null)
            {
                CurFocusedObj = hitInfo.collider.gameObject;
                if (CurFocusedObj != null)
                {
                    CurFocusedObj.SendMessage("Highlight", this as HandInteractionMaster);
                }
                if (showHandCrsr)
                {
                    _crs.transform.position = Camera.main.transform.position + (handGesture.normalized) * hitInfo.distance * 0.95F;
                }
            }
            else
            {
                CurFocusedObj = null;
                if (showHandCrsr)
                {
                    _crs.transform.position = Camera.main.transform.position + (handGesture.normalized * 2F);
                }
            }
        }
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        _handTracked = false;
        CurFocusedObj = null;
        if (showHandCrsr)
        {
            _crs.SetActive(false);
        }
    }

    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
        _handTracked = true;
        if (showHandCrsr)
        {
            _crs.SetActive(true);
        }
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (CurFocusedObj != null)
        {
            CurFocusedObj.SendMessage("Select");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
