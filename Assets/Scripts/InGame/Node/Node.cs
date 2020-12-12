using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    private GameObject _turret;
    
    private Renderer _rend;
    private Color _startColor;
    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
    }

    private void OnMouseDown()
    {
        if (_turret != null)
        {
            Debug.Log("Can't build there! - TODO: Display on screen.");
            return;
        }
        
        //Build a turret
        GameObject turretToBuild = BuildManager.Instance.GetTurretToBuild();
        var dir =  Quaternion.Euler(transform.eulerAngles +rotationOffset);
        _turret = Instantiate(turretToBuild, transform.position + positionOffset, dir);

    }

    private void OnMouseEnter()
    {
        _rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _startColor;
    }
}
