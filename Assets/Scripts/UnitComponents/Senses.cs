using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses
{
    Unit unit;
    int stabilityTimer = 0;
    bool stabilityFlag;
    Vector3 oldPosition;
    Vector3 oldDirection;
    Vector3 oldDisplacement;
    List<Vector2> leanHistory;

    public Senses(Unit _unit)
    {
        unit = _unit;
        leanHistory = new List<Vector2>();
    }
    public void WipeLeanHistory()
    {
        leanHistory.Clear();
    }
    public Vector3 LocalLeanPID()
    {
        //depends on balance base for making local
        Vector3 v3 = ConvertV2ToV3(LeanPID());
        Vector3 local = unit.bodyParts.balanceBaseGizmo.transform.InverseTransformVector(v3);
        return local;
    }

    public Vector3 LeanPIDGlobal()
    {
        Vector2 leanPID = LeanPID();
        return new Vector3(leanPID.x, 0, leanPID.y);
    }
    
    public Vector2 LeanPID() //GLOBAL
    {
        PositionGizmos();
        Vector3 balanceBaseDelta = unit.bodyParts.centreOfBalanceGizmo.transform.position - unit.bodyParts.balanceBaseGizmo.transform.position;
        Vector2 globalP = new Vector2(balanceBaseDelta.x, balanceBaseDelta.z) * unit.stats.pCoeff;

        leanHistory.Add(globalP);
        if (leanHistory.Count > unit.stats.memoryFrames) leanHistory.RemoveAt(0);
        Vector2 globalI = Helpers.AverageVector(leanHistory) * unit.stats.iCoeff;

        Vector2 globalD = ConvertRotationToLeanDirection(AngularMomentumGlobal()) * unit.stats.dCoeff;   //RED

        Debug.DrawRay(TrueCentreOfMass(), ConvertV2ToV3(globalP), Color.blue, 0.05f);
        Debug.DrawRay(TrueCentreOfMass(), ConvertV2ToV3(globalI), Color.cyan, 0.05f);
        Debug.DrawRay(TrueCentreOfMass(), ConvertV2ToV3(globalD), Color.red, 0.05f);

        //Debug.DrawRay(TrueCentreOfMass(), unit.bodyParts.rb.velocity, Color.green, 0.05f);
        //Debug.DrawRay(TrueCentreOfMass(), balanceBaseDelta, Color.magenta, 0.05f);


        return globalP + globalI + globalD;
    }
    public void PositionGizmos()
    {
        PositionBalanceBaseGizmo();
        PositionCentreOfBalanceGizmo();
    }
    public Vector3 ConvertV2ToV3(Vector2 v2) => new Vector3(v2.x, 0, v2.y);
    public Vector2 PositionLeanDirection()
    {
        Vector3 leanLocal = unit.transform.InverseTransformVector(PositionLeanGlobal());
        leanLocal = Helpers.FlatForward(leanLocal);
        return new Vector2(leanLocal.x, leanLocal.z);
    }
    Vector2 ConvertRotationToLeanDirection(Vector3 axisOfRotation)
    {
        return new Vector2(-axisOfRotation.z, axisOfRotation.x);
    }
    Vector3 ConvertLeanDirectionToRotation(Vector2 leanDirection) => new Vector3(leanDirection.y, 0, -leanDirection.x);
    public Vector3 AngularMomentumGlobal()
    {
        var centreOfMass = unit.senses.TrueCentreOfMass();

        //Body: use ang vel
        var body = unit.bodyParts.rb.mass * unit.bodyParts.rb.angularVelocity * unit.bodyStats.waistRadius / 4;
        //Debug.DrawRay(unit.transform.position, body, Color.magenta, 0.05f);
        //Hands: use vel
        var rightHandLever = unit.bodyParts.rightHand.transform.position - centreOfMass;
        var rh = unit.bodyParts.rightHandRB.mass * Vector3.Cross(rightHandLever, unit.bodyParts.rightHandRB.velocity);
        //Debug.DrawRay(unit.bodyParts.rightHand.transform.position, rh, Color.magenta, 0.05f);
        //Debug.DrawRay(unit.bodyParts.rightHand.transform.position, rightHandLever, Color.yellow, 0.05f);
        //Debug.DrawRay(unit.bodyParts.rightHand.transform.position, unit.bodyParts.rightHandRB.velocity, Color.green, 0.05f);
        //var leanDebug = ConvertLeanDirectionToV3(ConvertRotationToLeanDirection(rh));
        //Debug.DrawRay(unit.bodyParts.rightHand.transform.position, leanDebug, Color.red, 0.05f);
        var leftHandLever = unit.bodyParts.leftHand.transform.position - centreOfMass;
        var lh = unit.bodyParts.leftHandRB.mass * Vector3.Cross(leftHandLever, unit.bodyParts.leftHandRB.velocity);
        //Debug.DrawRay(unit.bodyParts.leftHand.transform.position, leftHandLever, Color.yellow, 0.05f);
        //Debug.DrawRay(unit.bodyParts.leftHand.transform.position, unit.bodyParts.leftHandRB.velocity, Color.green, 0.05f);
        //leanDebug = ConvertLeanDirectionToV3(ConvertRotationToLeanDirection(lh));
        //Debug.DrawRay(unit.bodyParts.leftHand.transform.position, leanDebug, Color.red, 0.05f);
        //Debug.DrawRay(unit.bodyParts.leftHand.transform.position, lh, Color.magenta, 0.05f);

        //Feet
        var rightFootLever = unit.bodyParts.rightFoot.transform.position - centreOfMass;
        var rf = unit.bodyParts.rightFootRB.mass * Vector3.Cross(rightFootLever, unit.bodyParts.rightFootRB.velocity);
        var leftFootLever = unit.bodyParts.leftFoot.transform.position - centreOfMass;
        var lf = unit.bodyParts.leftFootRB.mass * Vector3.Cross(leftFootLever, unit.bodyParts.leftFootRB.velocity);
        //leanDebug = ConvertLeanDirectionToV3(ConvertRotationToLeanDirection(lf));
        //Debug.DrawRay(unit.bodyParts.leftFoot.transform.position, leanDebug, Color.red, 0.05f);
        //leanDebug = ConvertLeanDirectionToV3(ConvertRotationToLeanDirection(rf));
        //Debug.DrawRay(unit.bodyParts.rightFoot.transform.position, leanDebug, Color.red, 0.05f);
        //Debug.DrawRay(unit.bodyParts.leftFoot.transform.position, unit.bodyParts.leftFootRB.velocity, Color.green, 0.05f);
        //Debug.DrawRay(unit.bodyParts.rightFoot.transform.position, unit.bodyParts.rightFootRB.velocity, Color.green, 0.05f);

        //sum them...?
        var total = body + rh + lh + rf + lf;
        return total;
    }
    public Vector3 MomentumLocal() => unit.transform.InverseTransformVector(MomentumGlobal());
    public Vector3 MomentumGlobal() 
    {
        //sum momenta
        var body = unit.bodyParts.rb.velocity * unit.bodyParts.rb.mass;
        var rh = unit.bodyParts.rightHandRB.velocity * unit.bodyParts.rightHandRB.mass;
        var lh = unit.bodyParts.leftHandRB.velocity * unit.bodyParts.leftHandRB.mass;
        var rf = unit.bodyParts.rightFootRB.velocity * unit.bodyParts.rightFootRB.mass;
        var lf = unit.bodyParts.leftFootRB.velocity * unit.bodyParts.leftFootRB.mass;
        float totalMass = unit.bodyParts.rb.mass + 
            unit.bodyParts.rightHandRB.mass + 
            unit.bodyParts.leftHandRB.mass + 
            unit.bodyParts.rightFootRB.mass + 
            unit.bodyParts.leftFootRB.mass;
        var avg = (body + rh + lh + rf + lf) / totalMass;
        return avg;
    }
    public bool Stable() => stabilityFlag;
    public Vector3 TrueCentreOfMass() //GLOBAL
    {
        Vector3 bodyCentre = unit.transform.TransformPoint(unit.bodyParts.rb.centerOfMass) * unit.bodyParts.rb.mass;
        Vector3 rh = unit.bodyParts.rightHand.transform.position * unit.bodyParts.rightHandRB.mass;
        Vector3 lh = unit.bodyParts.leftHandRB.transform.position * unit.bodyParts.leftHandRB.mass;
        Vector3 rf = unit.bodyParts.rightFootRB.transform.position * unit.bodyParts.rightFootRB.mass;
        Vector3 lf = unit.bodyParts.leftHandRB.transform.position * unit.bodyParts.leftFootRB.mass;
        float totalMass = unit.bodyParts.rb.mass +
            unit.bodyParts.rightHandRB.mass +
            unit.bodyParts.leftHandRB.mass +
            unit.bodyParts.rightFootRB.mass +
            unit.bodyParts.leftFootRB.mass;
        Vector3 centre = (bodyCentre + rh + lh + rf + lf) / totalMass;
        return centre;
    }

    public Vector3 PositionLeanGlobal() 
    {
        Vector3 CoBalance = CentreOfBalance(); //GLOBAL
        Vector3 CoBase = CentreOfBase(); //GLOBAL
        Vector3 leanDirection = CoBalance - CoBase;
        return leanDirection;
    }
    public Vector3 Displacement() => oldDisplacement;
    public bool Rising()
    {
        float newY = unit.transform.forward.y;
        float oldY = oldDirection.y;
        if (newY > oldY) return true;
        return false;
    }
    public bool Upright()
    {
        float angle = Vector3.Angle(unit.transform.up, Vector3.up);
        if (angle < unit.stats.uprightTolerance) return true;
        return false;
    }
    public bool OnHeels()
    {
        float dot = Vector3.Dot(PositionLeanGlobal(), unit.transform.forward);
        if (dot < 0) return true;
        return false;
    }
    Vector3 CentreOfBase() //GLOBAL
    {
        Vector3 leftFoot = unit.bodyParts.leftFoot.transform.position;
        Vector3 rightFoot = unit.bodyParts.rightFoot.transform.position;
        Vector3 pos = (leftFoot + rightFoot) / 2;
        pos.y = 0;
        return pos;
    }
    void PositionCentreOfBalanceGizmo()
    {
        unit.bodyParts.centreOfBalanceGizmo.transform.position = CentreOfBalance();
    }
    public void PositionBalanceBaseGizmo()
    {
        //Scale
        float footLength = 0.5f;
        float footWidth = 0.3f;

        float xMin = footWidth;
        float zMin = footLength;
        var gizmo = unit.bodyParts.balanceBaseGizmo;

        Vector3 deltaFootPos = unit.bodyParts.rightFoot.transform.position - unit.bodyParts.leftFoot.transform.position;
        Vector3 localDeltaFootPos = unit.transform.InverseTransformVector(deltaFootPos);
        float deltaX = localDeltaFootPos.x;
        float deltaZ = localDeltaFootPos.z;

        float zSize = zMin + deltaZ;
        float xSize = xMin + deltaX;

        Vector3 scale = unit.bodyParts.balanceBaseChild.transform.localScale;
        scale.x = xSize;
        scale.z = zSize;
        unit.bodyParts.balanceBaseChild.transform.localScale = scale;

        //Rotation = quat always?
        Quaternion rot = unit.transform.rotation;
        rot.x = 0;
        rot.z = 0;
        gizmo.transform.rotation = rot;
        //Position - average foot position
        gizmo.transform.position = CentreOfBase();
    }

    public Vector3 CentreOfBalance() //CoB is CoM projected onto the ground
    {
        Vector3 globalCOM = TrueCentreOfMass();
        Vector3 pos = new Vector3(globalCOM.x, 0f, globalCOM.z);
        unit.bodyParts.centreOfBalanceGizmo.transform.position = pos;
        return pos;
    }
        
    public void StabilityCheck() //checks if pos, rot are same as 10 frames prior
    {
        stabilityTimer++;
        if (stabilityTimer != unit.stats.stabilityTime) return;
        float posDelta = Vector3.Distance(unit.transform.position, oldPosition);
        float rotDelta = Vector3.Angle(unit.transform.forward, oldDirection);
        if (posDelta > unit.stats.posTolerance || rotDelta > unit.stats.rotTolerance) //if out of tolerance...
            stabilityFlag = false;
        else stabilityFlag = true;

        oldDisplacement = (unit.transform.position - oldPosition) / unit.stats.stabilityTime;
        oldPosition = unit.transform.position;
        oldDirection = unit.transform.forward;
        stabilityTimer = 0;
    }

    public bool OnBelly(float tolerance) //90 tolerance means sideways is barely on belly
    {
        Vector3 fwd = unit.transform.forward;
        Vector3 down = Vector3.down;
        float angle = Vector3.Angle(fwd, down);
        if (angle < tolerance) return true;
        return false;
    }

    public bool OnBack(float tolerance)
    {
        Vector3 fwd = unit.transform.forward;
        Vector3 up = Vector3.up;
        if (Vector3.Angle(fwd, up) < tolerance) return true;
        return false;
    }
}
