// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using Artics.Physics.UnityPhisicsVisualizers.Base;
using UnityEngine;

/// <summary>
/// Gizmos visualizer for DistanceJoint2D, FixedJoint2D, FrictionJoint2D, HingeJoint2D, SliderJoint2D;SpringJoint2D,WheelJoint2D
/// </summary>
[RequireComponent(typeof(AnchoredJoint2D))]
public class AnchoredJoint2DVisualizer : ShapeVisualizer
{
    protected AnchoredJoint2D Joint;

    public override void Init()
    {
        Joint = GetComponent<AnchoredJoint2D>();
        PointsLenght = 4;
        MultipliedPoints = new Vector2[PointsLenght];
        IsClosed = false;

        base.Init();
    }

    protected override void MultiplyMatrix()
    {
        if (Joint.connectedBody != null)
        {
            var tmpTransform = transform;
            MultipliedPoints[1] = MultipliedPoints[0] = tmpTransform.position;

            var matrix = Matrix4x4.TRS(MultipliedPoints[0], tmpTransform.rotation, tmpTransform.lossyScale);

            MultipliedPoints[1] += (Vector2)matrix.MultiplyVector(Joint.anchor);
            tmpTransform = Joint.connectedBody.transform;
            MultipliedPoints[3] = MultipliedPoints[2] = tmpTransform.position;

            matrix = Matrix4x4.TRS(MultipliedPoints[2], tmpTransform.rotation, tmpTransform.lossyScale);

            MultipliedPoints[2] += (Vector2)matrix.MultiplyVector(Joint.connectedAnchor);
        }
        else
        {
            var tmpTransform = transform;
            MultipliedPoints[1] = MultipliedPoints[0] = tmpTransform.position;

            var matrix = Matrix4x4.TRS(MultipliedPoints[0], tmpTransform.rotation, tmpTransform.lossyScale);

            MultipliedPoints[1] += (Vector2)matrix.MultiplyVector(Joint.anchor);
            MultipliedPoints[2] = Joint.connectedAnchor;            
        }
    }
}
