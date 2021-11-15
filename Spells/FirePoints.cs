using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FirePointID
{
    Feet,
    Front,
    Chest,
    WeaponRight,
    WeaponLeft,
}

public class FirePoints : MonoBehaviour
{
    [SerializeField] Transform feet;
    [SerializeField] Transform front;
    [SerializeField] Transform chest;
    [SerializeField] Transform weapRight;
    [SerializeField] Transform weapLeft;

    Transform[] firePoints;

    private void Awake()
    {
        firePoints = new Transform[5];
        firePoints[0] = feet;
        firePoints[1] = front;
        firePoints[2] = chest;
        firePoints[3] = weapRight;
        firePoints[4] = weapLeft;

        for (int i = 0; i < 5; i++)
            if (firePoints[i] == null) firePoints[i] = gameObject.transform;
    }

    public Vector3 GetFirePoint(FirePointID _index)
    {
        return firePoints[(int)_index].position;
    }
    
    public Quaternion GetFirePointRotation(FirePointID _index)
    {
        return firePoints[(int)_index].rotation;
    }
}
