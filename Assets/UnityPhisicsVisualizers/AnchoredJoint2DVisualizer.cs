// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using Artics.Physics.UnityPhisicsVisualizers.Base;
using Artics.Math;
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
            var tmpPosition = tmpTransform.position;

            MultipliedPoints[1] = MultipliedPoints[0] = tmpPosition;

            var matrix = tmpTransform.GetGlobalTransformMatrix(MultipliedPoints[0]);
            tmpTransform = Joint.connectedBody.transform;

            MultipliedPoints[1] += (Vector2) matrix.MultiplyVector(Joint.anchor);
            MultipliedPoints[3] = MultipliedPoints[2] = tmpPosition;

            matrix = tmpTransform.GetGlobalTransformMatrix(MultipliedPoints[2]);

            MultipliedPoints[2] += (Vector2) matrix.MultiplyVector(Joint.connectedAnchor);
        }
        else
        {
            var tmpTransform = transform;
            MultipliedPoints[1] = MultipliedPoints[0] = tmpTransform.position;

            var matrix = tmpTransform.GetGlobalTransformMatrix(MultipliedPoints[0]);

            MultipliedPoints[1] += (Vector2) matrix.MultiplyVector(Joint.anchor);
            MultipliedPoints[2] = Joint.connectedAnchor;
        }
    }
}