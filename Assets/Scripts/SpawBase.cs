using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawBase: MonoBehaviour
{
    private const float marginFromBorder = 0.5f;

    public static GameObject SpawObject(Vector3 centerPointAreaSpaw, float radiusArea, Transform floor, Transform objectForSpaw, List<Transform> listOnFloor)
    {
        int countAttemptFindPosition = 100;
        System.Tuple<Vector3, Vector2> coordinatesRect = GetAreaForSpaw(centerPointAreaSpaw, radiusArea, floor);

        
        float radiusX = coordinatesRect.Item2.x / 2f;
        float radiusZ = coordinatesRect.Item2.y / 2f;
        while (countAttemptFindPosition > 0)
        {
            Vector3 pointSpaw = coordinatesRect.Item1;
            pointSpaw.x = Random.Range(pointSpaw.x - radiusX, pointSpaw.x + radiusX);
            pointSpaw.z = Random.Range(pointSpaw.z - radiusZ, pointSpaw.z + radiusZ);
            pointSpaw.y = floor.position.y + objectForSpaw.localScale.y / 2f;
            if (IsPlaceForSpawFree(pointSpaw, objectForSpaw.localScale, listOnFloor))
            {
                GameObject newObject = Instantiate(objectForSpaw.gameObject, pointSpaw, Quaternion.identity, null);
                return newObject;
            }
            countAttemptFindPosition--;
        }
        return null;
    }

    public static GameObject SpawRestoredObject(Vector3 position, Quaternion rotation, Transform objectForSpaw)
    {
        GameObject newObject = Instantiate(objectForSpaw.gameObject, position, rotation, null);
        return newObject;
    }

    public static GameObject SpawObject(Transform placeForSpaw, Transform objectForSpaw, List<Transform> listOnFloor)
    {
        int countAttemptFindPosition = 100;
        while (countAttemptFindPosition > 0)
        {
            Vector3 pos = placeForSpaw.position;
            float border = placeForSpaw.localScale.x / 2f - objectForSpaw.localScale.x / 2f;
            pos.x = Random.Range(placeForSpaw.position.x - border, placeForSpaw.position.x + border);
            pos.z = Random.Range(placeForSpaw.position.z - border, placeForSpaw.position.z + border);
            pos.y = placeForSpaw.position.y + objectForSpaw.localScale.y / 2f;
            if (IsPlaceForSpawFree(pos, objectForSpaw.localScale, listOnFloor))
            {
                GameObject newObject = Instantiate(objectForSpaw.gameObject, pos, Quaternion.identity, null);
                return newObject;
                //listOnFloor.Add(newObject.GetComponent<Animal>());
                //Debug.Log("animalListOnFloor.Count: " + animalListOnFloor.Count);
                //break;
            }
            countAttemptFindPosition--;
            //Debug.Log("countAttemptFindPosition: " + countAttemptFindPosition);
        }
        return null;
    }

    private static System.Tuple<Vector3, Vector2> GetAreaForSpaw(Vector3 centerPoint, float radius, Transform floor)
    {
        // first coordinate of Rect
        Vector3 p1 = centerPoint - new Vector3(radius,0f, radius);
        if (p1.x < floor.position.x - floor.localScale.x / 2f + marginFromBorder)
            p1.x = floor.position.x - floor.localScale.x / 2f + marginFromBorder;

        if (p1.z < floor.position.z - floor.localScale.z / 2f + marginFromBorder)
            p1.z = floor.position.z - floor.localScale.z / 2f + marginFromBorder;

        // second (opposite) coordinate of Rect
        Vector3 p2 = centerPoint + new Vector3(radius, 0f, radius);
        if (p2.x > floor.position.x + floor.localScale.x / 2f - marginFromBorder)
            p2.x = floor.position.x + floor.localScale.x / 2f - marginFromBorder;

        if (p2.z > floor.position.z + floor.localScale.z / 2f - marginFromBorder)
            p2.z = floor.position.z + floor.localScale.z / 2f - marginFromBorder;

        Vector3 newCenter = (p1 + p2) / 2f;

        float width = p2.x - p1.x;
        float height = p2.z - p1.z;
        Vector2 size = new Vector2(width, height);

        return System.Tuple.Create(newCenter, size);
    }

    private static  bool IsPlaceForSpawFree(Vector3 positionForSpaw, Vector3 scaleSpawObject, List<Transform> listOnFloor)
    {
        bool isFree = true;
        foreach (Transform item in listOnFloor)
        {
            if (IsTwoObjectsIntersects(
                item.transform.position,
                item.transform.localScale,
                positionForSpaw,
                scaleSpawObject))
            {
                //Debug.Log("IsPlaceForSpawFree: false");
                return false;
            }
        }
        return isFree;
    }

    private static bool IsTwoObjectsIntersects(Vector3 position1, Vector3 scale1, Vector3 position2, Vector3 scale2)
    {
        if (!IsIntersectByAxe(position1.x, scale1.x, position2.x, scale2.x))
            return false;

        if (!IsIntersectByAxe(position1.z, scale1.z, position2.z, scale2.z))
            return false;

        return true;
    }

    private static bool IsIntersectByAxe(float p1, float s1, float p2, float s2)
    {
        float borderMin1 = p1 - s1 / 2f;
        float borderMax1 = p1 + s1 / 2f;
        float borderMin2 = p2 - s2 / 2f;
        float borderMax2 = p2 + s2 / 2f;
        bool isIntersect = false;
        if (borderMin2 < borderMax1 && borderMax2 > borderMin1)
        {
            isIntersect = true;
        }
        return isIntersect;
    }
}
