using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/PhysAnimations/SlimeAttack")]
public class SlimeAttack : PhysAction
{
    public int prepFrames, lungeFrames;
    public float tallFactor, shortFactor, longFactor; //tallFactor must be above 1, shortFactor must be between 0 and 1, longFactor is for the lunge stretch
    public float impulseMagnitude;
    public float canHitRange, canHitRadius, canHitDeltaTheta;
    public override bool CanHit(Unit unit)
    {
        LayerMask mask = ServiceLocator.Instance.layerManager.layerMask;
        RaycastHit[] hits =
        Physics.SphereCastAll(unit.transform.position, canHitRadius, unit.transform.forward, canHitRange, mask);
        foreach(var hit in hits)
        {
            //Debug.Log("spherecast for slime attack canhit is hitting...");
            Unit _target = hit.collider.GetComponent<Unit>();
            if (_target.allegiance == unit.allegiance) continue; //if allied, continue;
            if (!Helpers.IsFacing(unit.gameObject, _target.transform.position, canHitDeltaTheta)) continue;
            Debug.Log("can hit true");
            return true;
        }
        
        return false;
    }
    public override void Do(Unit unit, int currentFrame) //target already chosen and in front, slime may or may not hit
    {
        //phase 1: no forces, grow tall & skinny, turn to face target as best as possible
        //phase 2: shrink from tall down to shorter and fatter than rest, add force (Impulse) forwards
        //phase 3: recover, scale back to 1 1 1
        //if (currentFrame == 0) Debug.Log($"Starting slime attack");
        //Debug.Log("doing slime attack any frame");
        if (currentFrame < prepFrames)
        {
            //phase 1: no forces, grow tall & skinny, turn to face target as best as possible
            var progress = (float)currentFrame / prepFrames;
            float y = 1 + (tallFactor - 1) * progress;
            float x = 1 / y;
            float z = 1 / y;
            SetScale(x, y, z);
        }
        else if (currentFrame < lungeFrames + prepFrames)
        {
            //phase 2: shrink from tall down to shorter and fatter than rest, add force (Impulse) forwards on first frame
            //if (currentFrame == prepFrames) unit.bodyParts.rb.AddForce(unit.transform.forward * impulseMagnitude, ForceMode.Impulse);
            var progress = (float)(currentFrame - prepFrames) / lungeFrames;
            float variableForce = Mathf.Sin(Mathf.PI * progress) * impulseMagnitude;
            unit.bodyParts.rb.AddForce(unit.transform.forward * variableForce);
            float yStart = tallFactor;
            float yEnd = shortFactor;
            float xStart = 1 / yStart;
            float xEnd = 1 / yEnd;
            float zStart = 1 / yStart;
            float zEnd = longFactor * xEnd;
            float y = yStart + (yEnd - yStart) * progress;
            float x = xStart + (xEnd - xStart) * progress;
            float z = zStart + (zEnd - zStart) * progress;
            SetScale(x, y, z);
        }
        else if (currentFrame < totalFrames)
        {
            //phase 3: recover, scale back to 1 1 1 from short & long
            var progress = (float)(currentFrame - prepFrames - lungeFrames) / (totalFrames - prepFrames - lungeFrames);
            float yStart = shortFactor;
            float zStart = longFactor / yStart;

            float y = yStart + (1 - yStart) * progress;
            float x = 1 / y;
            float z = zStart + (1 - zStart) * progress;
            SetScale(x, y, z);
        }

        if (currentFrame == totalFrames - 1) SetScale(1, 1, 1);

        void SetScale(float x, float y, float z)
        {
            var scale = unit.transform.localScale;
            scale.x = x;
            scale.y = y;
            scale.z = z;
            unit.transform.localScale = scale;
        }
    }
}
