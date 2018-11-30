# Unity3d Physics2d Visualizer

Tool to help you visualize 2d physics colliders and joints. You can track their transform at runtime. Just add the proper component to GameObject with a Collider2D. Supported Unity 5.6 and higher.

 __Warning:__ Objects with scaled parent can render incorrectly. 

 <p align="center">
 <img align="center" src="/Screenshots/MovementLogger.gif">
 </p>

You can visualize colliders in two ways: rendering them as mesh and using editor gizmos. Frist method needs more steps to get result, second - is very simple but works only in Editor and need to attach different components for each type of Collider2D. Also, this package includes GUI tools to batch some operations with both types of visualizers.

# Collider2dRenderer:

New version supports visualizing through Collider2dRenderer which renders colliders shape to standard mesh. This type of visualization lacks most features of the gizmos-based visualizers but it works in standalone builds, can be batched in one draw call to minimize graphics overhead impact. 

 <p align="center">
 <img align="center" width="60%" src="/Screenshots/Scr2.png">
 </p>
 
  * `AlwaysUpdate` - updates mesh every frame
 * `MeshColor` - vertex color of the mesh
 * `UseMaterialPropertyBlock` - Set color by material property block
 * `SetVertexColors` - Set vertices colors with  `MeshColor` value
 * `UseCircleProximity` - use a custom number of segments created for circular areas of CircleCollider2D and CapsuleColldier2D. If not checked default value will be used (20). You can set the default value by accessing static variable Collider2dPointsGetter::CircleProximity.
 * `CustomCircleProximity` - use a custom number of segments
 * `Thickness` - the thickness of shape in Unity points. If a camera is Orthogonal and `UsePixelSize` is true it will be in pixel size
 * `UsePixelSize` - to set mesh thickness in pixels
 
You can automate routine operations for you visualizers with the new manager which can be found at "Tools/Physics2dVisualizer/Open Manager" menu.

 <p align="center">
 <img align="center" src="/Screenshots/VisualizerWindow.gif">
 </p>
 
# Gizmos visualizers:
 
### Currently supports:
 * `AnchoredJoint2DVisualizer` - for `DistanceJoint2D`, `FixedJoint2D`, `FrictionJoint2D`, `HingeJoint2D`, `SliderJoint2D`, `SpringJoint2D`, `WheelJoint2D`
 * `Box2dVisualizer` for 	`BoxCollider2D` *(EdgeRadius not supported)*
 * `Capsule2dVisualizer` for `CapsuleCollider2D`
 * `Circle2dVisualizer` for `CircleCollider2D`
 * `Edge2dVisualizer` for `EdgeCollider2D`
 * `Polygon2dVisualizer` for `PolygonCollider2D` *(Multiple polygon Paths not supported)* 
 * `MovementLogger` for tracking object's position and transform

### Warning:
 * Don't forget to enable Gizmos visibility
 
 ### You can manipulate with 3 params:
  * `IsVisible` Enables or disables rendering of collider.
  * `DynamicBounds` Updates bounds of collider every time OnDrawGizmos calls. Useful when you changing Offset, Size, Radius, e.t. of the collider. If you don't just disable to increase performance.
  * `Color` Color of rendered collider.
  
   ### Use MovementLogger to track objects transform at runtime. You can manipultate with 8 params:
   * `BufferSize` limit number of records, set 0 if unlimited
   * `RecordInterval` limit time of data storing, set 0 if you wat to do it every Update
   * `Color` Color of visualization
   * `Record` enable recording
   * `DrawPoints` enable visualizing of position points
   * `DrawLines` enable visualizing of path
   * `RecordObjectState` Records collider visualizer state, requires class inherited from BaseVisualizer
   * `RecordUnmoved` record state even when the object wasn't moved
   * `DrawState` Draws collider visualizer state   
   
   ### Example:
 <p align="center">
 <img align="center" width="80%" src="/Screenshots/Scr1.png">
 </p>
 
    
   ### Tools:
 <p align="center">
 <img align="center" width="60%" src="/Screenshots/Screenshot_50.png">
 </p>
 
 This tools can help you to automate some routine operations without opening manager window.
 
   * `Remove all visualizers` - removes all gizmos visualizers found in the scene
 *  `Add visualizers for all Colliders2d` - automatically adds relevant gizmos visualizers for all Collider2D found in the scene
 * `Remove all Collider2dRenderer` - removes all Collider2DRenderer found in the scene
 * `Add the Collider2dRenderer for all Colliders2d` - automatically adds Collider2Renderer visualizer for all Collider2D found in the scene
* `Add default sprites material for all Colliders2d` - sets shared material based on "Sprites/Default" shader for all Collider2Renderer found in the scene
