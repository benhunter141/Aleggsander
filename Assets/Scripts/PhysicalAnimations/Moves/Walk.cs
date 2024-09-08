using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/Walk")]
public class Walk : PhysAction
{
    public float peakFootXRot;
    public override void Do(Unit egg, int currentFrame)
    {
        var stats = ServiceLocator.Instance.soHolder.standardEggMoveStats;
        PlaceFeet(egg.brain.move);
        RotateFeet();
        //goal is to set cj.ca.positions and cj.targetrotations for feet
        //maybe a little nudge force here or there
        //frame is incremented elsewhere, we just do one frame

        void RotateFeet()
        {
            float progress = (float)currentFrame / totalFrames;
            float rotateAmountLeft = -peakFootXRot * Mathf.Cos(2 * Mathf.PI * progress);
            float rotateAmountRight = -rotateAmountLeft;

            ConfigurableJoint leftCJ = egg.bodyParts.leftFootCJ;
            ConfigurableJoint rightCJ = egg.bodyParts.rightFootCJ;

            Quaternion leftQuat = Quaternion.Euler(new Vector3(rotateAmountLeft, 0, 0));
            Quaternion rightQuat = Quaternion.Euler(new Vector3(rotateAmountRight, 0, 0));

            ConfigurableJointExtensions.SetTargetRotationLocal(leftCJ, leftQuat, Quaternion.identity);
            ConfigurableJointExtensions.SetTargetRotationLocal(rightCJ, rightQuat, Quaternion.identity);
        }

        void PlaceFeet(Vector2 move)
        {
            float stepProgress = (float)currentFrame / totalFrames;
            //Debug.Log($"placing feet {stepProgress}", egg.gameObject);
            PlaceRightFoot(stepProgress, move);
            PlaceLeftFoot(stepProgress, move);
        }

        void PlaceRightFoot(float stepProgress, Vector2 move)
        {
            //STARTS BACK, GOES FWD, THEN BACK
            Vector3 footPosition;
            Vector3 moveV3 = new Vector3(move.x, 0, move.y);
            float dot = Vector3.Dot(moveV3, egg.transform.forward);
            float stepLength = stats.stepLength;
            if (dot < 0.5) stepLength /= 2;
            else stepLength *= dot;

            float moveMagnitude = move.SqrMagnitude();
            stepLength *= moveMagnitude;

            //what direction is move relative to facing direction?
            Vector3 localMove = egg.transform.InverseTransformDirection(moveV3);

            //fwd then back
            if (stepProgress < 0.5f)
            {
                ////foot moves fwd, low centre in midline
                Vector3 centre = new Vector3(stats.footLateralDistance, stats.centreFwdHeight, 0);
                footPosition = Helpers.Arc(RightFootBackPosition(stepLength, localMove), RightFootForwardPosition(stepLength, localMove), centre, stepProgress * 2f);
            }
            else
            {
                //foot moves back, tilted axis so foot gets underneath smoothly
                Vector3 centre = new Vector3(stats.centreBackLateralDistance, stats.centreBackHeight, 0);
                footPosition = Helpers.Arc(RightFootForwardPosition(stepLength, localMove), RightFootBackPosition(stepLength, localMove), centre, 2 * stepProgress - 1f);
                //nudge right as right foot moves back
                egg.bodyParts.rb.AddForce(egg.transform.right * stats.nudgeForce);
            }
            //Debug.Log($"stepprogress: {stepProgress}. footPosition:{footPosition.x.ToString("F2")},{footPosition.y.ToString("F2")},{footPosition.z.ToString("F2")}");
            //Debug.Log($"stepprogress: {stepProgress}.
            Vector3 actualFootPosition = egg.transform.InverseTransformPoint(egg.bodyParts.rightFoot.transform.position);
            Vector3 displacement = footPosition - actualFootPosition;
            if (Vector3.SqrMagnitude(displacement) > stats.maxConnectedAnchorDistance * stats.maxConnectedAnchorDistance)
            {
                footPosition = actualFootPosition + displacement.normalized * stats.maxConnectedAnchorDistance;
                //Debug.Log("limiting CA, length:" + displacement.magnitude);
            }
            //Debug.Log($"setting r foot position to");
            //Helpers.PrintVector3(footPosition);
            egg.bodyParts.rightFootCJ.connectedAnchor = footPosition;
            //ServiceLocator.Instance.gizmos.gizmos[1].transform.position = egg.transform.TransformPoint(footPosition);
        }

        void PlaceLeftFoot(float stepProgress, Vector2 move)
        {
            //STARTS FWD, BACK, THEN FWD
            //reversed: back then fwd
            Vector3 footPosition;
            Vector3 moveV3 = new Vector3(move.x, 0, move.y);
            float dot = Vector3.Dot(moveV3, egg.transform.forward);
            float stepLength = stats.stepLength;
            if (dot < 0.5) stepLength /= 2;
            else stepLength *= dot;

            float moveMagnitude = move.SqrMagnitude();

            //what direction is move relative to facing direction?
            Vector3 localMove = egg.transform.InverseTransformDirection(moveV3);

            stepLength *= moveMagnitude;
            //back then fwd
            if (stepProgress < 0.5f)
            {
                //foot moves back, tilted axis so foot gets underneath smoothly
                Vector3 centre = new Vector3(-stats.centreBackLateralDistance, stats.centreBackHeight, 0);
                footPosition = Helpers.Arc(LeftFootForwardPosition(stepLength, localMove), LeftFootBackPosition(stepLength, localMove), centre, 2 * stepProgress);
                //TORQUE VERSION
                //egg.bodyParts.rb.AddTorque(-egg.transform.forward * stats.nudgeForce);
                //FORCE VERSION
                egg.bodyParts.rb.AddForce(-egg.transform.right * stats.nudgeForce);
            }
            else
            {
                ////foot moves fwd, low centre in midline
                Vector3 centre = new Vector3(-stats.footLateralDistance, stats.centreFwdHeight, 0);
                footPosition = Helpers.Arc(LeftFootBackPosition(stepLength, localMove), LeftFootForwardPosition(stepLength, localMove), centre, stepProgress * 2f - 1);
            }
            //Debug.Log($"stepprogress: {stepProgress}. footPosition:{footPosition.x.ToString("F2")},{footPosition.y.ToString("F2")},{footPosition.z.ToString("F2")}");
            //Debug.Log($"stepprogress: {stepProgress}.

            //SET CA FOR LEFT FOOT
            Vector3 actualFootPosition = egg.transform.InverseTransformPoint(egg.bodyParts.leftFoot.transform.position);
            Vector3 displacement = footPosition - actualFootPosition;
            if (Vector3.SqrMagnitude(displacement) > stats.maxConnectedAnchorDistance * stats.maxConnectedAnchorDistance)
            {
                footPosition = actualFootPosition + displacement.normalized * stats.maxConnectedAnchorDistance;
                //Debug.Log("limiting CA, length:" + displacement.magnitude);
            }
            egg.bodyParts.leftFootCJ.connectedAnchor = footPosition;
            //ServiceLocator.Instance.gizmos.gizmos[0].transform.position = egg.transform.TransformPoint(footPosition);
        }

        Vector3 RightFootRestPosition() => new Vector3(stats.footLateralDistance, stats.footHeight, 0);
        Vector3 LeftFootRestPosition() => new Vector3(-stats.footLateralDistance, stats.footHeight, 0);
        //Vector3 LeftFootForwardPosition(float stepLength, Vector3 localMove) => new Vector3(-stats.footLateralDistance, stats.footHeight, stepLength / 2);
        //Vector3 LeftFootBackPosition(float stepLength, Vector3 localMove) => new Vector3(-stats.footLateralDistance, stats.footHeight, -stepLength / 2);
        //Vector3 RightFootForwardPosition(float stepLength, Vector3 localMove) => new Vector3(stats.footLateralDistance, stats.footHeight, stepLength / 2);
        //Vector3 RightFootBackPosition(float stepLength, Vector3 localMove) => new Vector3(stats.footLateralDistance, stats.footHeight, -stepLength / 2);
        Vector3 RightFootBackPosition(float stepLength, Vector3 localMove)
        {
            Vector3 directlyBack = new Vector3(stats.footLateralDistance, stats.footHeight, -stepLength / 2);
            Vector3 displacement = directlyBack - RightFootRestPosition();
            Vector3 newDisplacement = -displacement.magnitude * localMove;
            return RightFootRestPosition() + newDisplacement;
        }
        Vector3 RightFootForwardPosition(float stepLength, Vector3 localMove)
        {
            Vector3 directlyForward = new Vector3(stats.footLateralDistance, stats.footHeight, -stepLength / 2);
            Vector3 displacement = directlyForward - RightFootRestPosition();
            Vector3 newDisplacement = displacement.magnitude * localMove;
            return RightFootRestPosition() + newDisplacement;
        }
        Vector3 LeftFootBackPosition(float stepLength, Vector3 localMove)
        {
            Vector3 directlyBack = new Vector3(-stats.footLateralDistance, stats.footHeight, -stepLength / 2);
            Vector3 displacement = directlyBack - LeftFootRestPosition();
            Vector3 newDisplacement = -displacement.magnitude * localMove;
            return LeftFootRestPosition() + newDisplacement;
        }
        Vector3 LeftFootForwardPosition(float stepLength, Vector3 localMove)
        {
            Vector3 directlyForward = new Vector3(-stats.footLateralDistance, stats.footHeight, -stepLength / 2);
            Vector3 displacement = directlyForward - LeftFootRestPosition();
            Vector3 newDisplacement = displacement.magnitude * localMove;
            return LeftFootRestPosition() + newDisplacement;
        }
    }


}
