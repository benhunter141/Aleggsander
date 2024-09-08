using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Helpers
{

    static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);

    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    static WaitForSeconds _shortDelay = new WaitForSeconds(0.2f);
    static WaitForSeconds _oneSecond = new WaitForSeconds(1f);

    public static Vector2 AverageVector(List<Vector2> list)
    {
        Vector2 sum = Vector2.zero;
        foreach(var l in list)
        {
            sum += l;
        }
        sum /= list.Count;
        return sum;
    }
    public static Vector3 AveragePosition(List<Unit> objects) //make this generic
    {
        Vector3 position = Vector3.zero;
        foreach (var item in objects)
        {
            //how do you access properties when it is generic and might not have those properties?
            position += item.transform.position;
        }
        position /= objects.Count;
        return position;
    }
    public static void PrintVector3(Vector3 v3)
    {
        Debug.Log($"{v3.x.ToString("F2")}, {v3.y.ToString("F2")}, {v3.z.ToString("F2")}");
    }

    public static bool IsFacing(GameObject _unit, Vector3 _position, float deltaTheta)
    {
        Vector3 displacement = _position - _unit.transform.position;
        Vector3 forward = _unit.transform.forward;
        float delta = Vector3.Angle(displacement, forward);
        
        if (delta > deltaTheta) return false;
        else return true;
    }

    public static WaitForSeconds OneSecond
    {
        get { return _oneSecond; }
    }
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }
    public static WaitForSeconds ShortDelay
    {
        get { return _shortDelay; }
    }

    static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }

    public static WaitForSeconds Get(float seconds) //caches whatever you do.. eg. call Helpers.WaitForSeconds ??? 
    {
        if (!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }
    public static IEnumerator PauseForDuration(float duration) //DOES THIS WORK??
    {
        float pauseFrames = duration / Time.fixedDeltaTime;
        for (int i = 0; i < pauseFrames; i++)
        {
            yield return Helpers.FixedUpdate;
        }
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public static Vector2 PositionOnCircle(float theta, float radius)
    {
        float xCoord = Mathf.Cos(theta) * radius;
        float yCoord = Mathf.Sin(theta) * radius;
        return new Vector2(xCoord, yCoord);
    }
    public static Vector2 PositionOnCircle(float theta)
    {
        float xCoord = Mathf.Cos(theta);
        float yCoord = Mathf.Sin(theta);
        return new Vector2(xCoord, yCoord);
    }

    public static Vector3 PositionOnCircle(float theta, float radius, Vector3 xDirection, Vector3 yDirection)
    {
        Vector2 positionOn2DCircle = PositionOnCircle(theta, radius);
        Vector3 xComponent = xDirection * positionOn2DCircle.x;
        Vector3 yComponent = yDirection * positionOn2DCircle.y;
        return xComponent + yComponent;
    }

    public static Vector3 PositionOnCircle(float theta, Vector3 axis, Vector3 startingDisplacementFromCentre)
    {
        Vector3 xVector = startingDisplacementFromCentre; //this is (1,0) on the unit circle
        Vector3 yVector = Vector3.Cross(xVector, axis); //this is (0,1) on the unit circle
        Vector3 xComponent = xVector * Mathf.Cos(theta);
        Vector3 yComponent = yVector * Mathf.Sin(theta);
        return xComponent + yComponent;
    }

    //For egg, use positions relative to egg
    public static Vector3 Arc(Vector3 startPosition,
        Vector3 endPosition,
        Vector3 centrePosition,
        float progress)
    {
        Vector3 startingDisplacementFromCentre = startPosition - centrePosition;
        Vector3 endDisplacementFromCentre = endPosition - centrePosition;
        Vector3 axis = Vector3.Cross(startingDisplacementFromCentre, endDisplacementFromCentre);
        axis.Normalize();
        //to get theta you need total angle & progress
        float theta = progress * Vector3.SignedAngle(endDisplacementFromCentre, startingDisplacementFromCentre, axis) * Mathf.PI / 180f;

        if (progress == 0)
        {
            //Debug.Log("New Arc");
            //Helpers.PrintVector3(startPosition);
            //Debug.Log($"signed angle:{theta.ToString("F2")}");
        }
            return centrePosition + PositionOnCircle(theta, axis, startingDisplacementFromCentre);
    }

   

    public static Vector3 FlatForward(Vector3 forward) => new Vector3(forward.x, 0, forward.z);

    public static Vector3 ConvertToV3(Vector2 v2) => new Vector3(v2.x, 0, v2.y);
    public static Vector2 ConvertToV2(Vector3 v3) => new Vector2(v3.x, v3.z);
    

}