# Unity3d Physics2d Visualizer *[work in progress]*


Tool to help you visualize 2d physics colliders. Just add the proper component to GameObject with a Collider2d.

Currently supports:
 * `Box2dVisualizer` for 	`BoxCollider2D` *(EdgeRadius not supported)*
 * `Capsule2dVisualizer` for `CapsuleCollider2D`
 * `1Circle2dVisualizer` for `CircleCollider2D`
 * `Edge2dVisualizer` for `EdgeCollider2D`
 * `Polygon2dVisualizer` for `PolygonCollider2D` *(Multiple polygon Paths not supported)*

 You also need to install Debug Drawing Extension https://assetstore.unity.com/packages/tools/debug-drawing-extension-11396
 
 ###### Don't forget to enable Gizmos visibility
 
 You can manipulate with 3 params:
  * `IsVisible` Enables or disables rendering of collider.
  * `DynamicBounds` Updates bounds of collider every time OnDrawGizmos calls. Useful when you changing Offset, Size, Radius, e.t. of the collider. If you don't just disable to increase performance.
  * `Color` Color of rendered collider.
  
  ![Screenshot](/Screenshots/Scr1.png)
