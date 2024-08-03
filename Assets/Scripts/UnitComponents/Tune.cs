using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tune
{
    Unit egg;
    MoveStats stats;
    public Tune(Unit _egg)
    {
        egg = _egg;
        stats = ServiceLocator.Instance.soHolder.standardEggMoveStats;
    }
    public void Movement()
    {
            //RBs
        egg.bodyParts.rb.mass = stats.mass;
        egg.bodyParts.rb.drag = stats.drag;
        egg.bodyParts.rb.angularDrag = stats.angularDrag;

        egg.bodyParts.leftFootRB.mass = stats.footMass;
        egg.bodyParts.leftFootRB.drag = stats.footDrag;
        egg.bodyParts.leftFootRB.angularDrag = stats.angularDrag;

        egg.bodyParts.rightFootRB.mass = stats.footMass;
        egg.bodyParts.rightFootRB.drag = stats.footDrag;
        egg.bodyParts.rightFootRB.angularDrag = stats.angularDrag;

            //Feet CJs
        var spring = egg.bodyParts.leftFootCJ.linearLimitSpring;
        spring.spring = stats.spring;
        spring.damper = stats.damper;
        egg.bodyParts.leftFootCJ.linearLimitSpring = spring;

        spring = egg.bodyParts.rightFootCJ.linearLimitSpring;
        spring.spring = stats.spring;
        spring.damper = stats.damper;
        egg.bodyParts.rightFootCJ.linearLimitSpring = spring;

        //Colliders
        PhysicMaterial mat = egg.bodyParts.bodyCollider.material;
        mat.dynamicFriction = stats.bodyDynamicFriction;
        mat.staticFriction = stats.bodyStaticFriction;
        egg.bodyParts.bodyCollider.material = mat;

        mat = egg.bodyParts.leftFootCollider.material;
        mat.dynamicFriction = stats.footDynamicFriction;
        mat.staticFriction = stats.footStaticFriction;
        egg.bodyParts.leftFootCollider.material = mat;

        mat = egg.bodyParts.rightFootCollider.material;
        mat.dynamicFriction = stats.footDynamicFriction;
        mat.staticFriction = stats.footStaticFriction;
        egg.bodyParts.rightFootCollider.material = mat;

    }
}
