# Unity3d Physics2d Visualizer *[work in progress]*

Tool to help you visualize 2d physics colliders and track their transform at runtime. Just add the proper component to GameObject with a Collider2d. 

 <p align="center">
 <img align="center" src="/Screenshots/MovementLogger.gif">
 </p>
 
New version supports visualizing through Collider2dRenderer which renders colliders shape to standard mesh. This version lacks most features of the gizmos-based visualizers but it works in standalone builds, can be batched in one draw call to minimize graphics overhead impact. You can automate routine operations for you visualizers with the new manager which can be found at "Tools/Physics2dVisualizer/Open Manager" menu.
 
# Gizmos visualizers:
 
### Currently supports:
 * `Box2dVisualizer` for 	`BoxCollider2D` *(EdgeRadius not supported)*
 * `Capsule2dVisualizer` for `CapsuleCollider2D`
 * `Circle2dVisualizer` for `CircleCollider2D`
 * `Edge2dVisualizer` for `EdgeCollider2D`
 * `Polygon2dVisualizer` for `PolygonCollider2D` *(Multiple polygon Paths not supported)* 
 * `MovementLogger` for tracking object's position and transform *(Edgae collider not supported)*

### Warning:
 * You also need to install Debug Drawing Extension https://assetstore.unity.com/packages/tools/debug-drawing-extension-11396 
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
   * `DrawState` Draws collider visualizer state   
   
   ### Example:
 <p align="center">
 <img align="center" width="80%" src="/Screenshots/Scr1.png">
 </p>
 
    
   ### Tools in menu:
 <p align="center">
 <img align="center" width="60%" src="/Screenshots/Screenshot_50.png">
 </p>
   
