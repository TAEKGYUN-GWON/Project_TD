using System;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{

    public GameObject standardTurretPrefab;

    private void Awake()
    {
        turretToBuild = standardTurretPrefab;
    }

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }
    
}
