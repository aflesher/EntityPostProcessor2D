# Entity Post-Processor 2D
## Description
This tool allows developers to easily run filters and effects on multi-image or assembled mesh objects.
## The Problem
2D skeleton animation tools like Spine 2D are fantastic for creating 2D artwork in games. They allow for smooth animations while reducing the amount of assets required for each object. A drawback of these assets is that they are comprised of multiple images. Any effects that are applied to the asset are applied to the individual images and not the assembled mesh that you see on screen.

Let&apos;s take a look at an example of when this becomes an issue. Say you want to apply an outline effect. We start with following images

![](https://imgur.com/QdiUxb2)

which are assembled into the following mesh at runtime

![](https://imgur.com/rEYrjZL)

when we attach a simple outline shader we get the following

![](https://imgur.com/v9iBY6T)

what we want is this

![](https://imgur.com/mKiwh2F)

## The Solution
A common technique to solve this problem is to use a multi camera setup that captures the assembled mesh and renders it to a texture.

![](https://imgur.com/0c5onka)

This can be a lot to manage though. Entity Post-Processor 2D seeks to automate the setup and management of this multi camera solution for you.
## Getting Started
### Setup
1. Create a new layer named `EntityPostProcessor`. You should treat this as a reserved layer and never directly add anything to it
1. Remove this new layer your main camera _Culling Mask_.
  1. Select the Main Camera from your Scene Hierarchy
  1. Open the Culling Mask drop down in the camera component in the Inspector
  1. Select _EntityPostProcessor_ to remove it from the _Culling Mask_

### Create an Entity
1. Right click on the scene Hierarchy and select _EntityPostProcessor2D_->_Entity_
1. Attach any 2D assets you want to display to the child object _RenderSource_ of your new _Entity_ gameObject. (note: you can replace this RenderSouce object with your own gameObject instead of making it a child object. Just be sure to attach an EntityRenderSource to your object)
1. Position the _RenderSource_ relative to the parent _Entity_ however you want.
1. Set the `Sorting Layer` and `Order In Layer` that you want your _RenderSource_ to appear on.
1. Check `Show Camera Rect` is checked
1. You should see a blue square around your character. This represents the area that will be captured by the post processing camera. The goal is to make this as small possible without your 2D assets going outside this blue box.
1. You can change this capture size by changing the `Render Texture Size` _Width_ and _Height_.
1. You can also change how your asset is positioned within the camera with `Source Capture Offset`
1. Play around with `Render Texture Size` and `Source Capture Offset` until you've found the best fit. Also remember, this has to account for animations.

### Create an EntityPostProcessor
1. Right click on the scene Hierarchy and select _EntityPostProcessor2D_->_PostProcessor_
1. Drag this object into your Project hierarchy and delete it from the scene
1. Assign this `EntityPostProcessor` prefab  to the `PostProcessor` field of the _Entity_ you previously created

  _Note: An EntityPostProcessor prefab can be used by many/all of your Entity objects._

### Test it
At this point when you run your scene your new Entity shold be displayed as if nothings changed. If you look at the Entity the RenderSouce should have be replaced with a Render Texture.

### Add some effects to your Entity
Any scripts attached to your _EntityPostProcessor_ can apply post-processing effects to your entity using the `OnRenderImage` function. See the [docs](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnRenderImage.html) for more details.

To see a few sample effects you can attach `EntityPostProcessor2D/Examples/Scripts/EntityEffects` scripts to your _EntityPostProcessor_ prefab

### Troubleshooting
**Issue** My character appears cut off  
**Solution** Enable `Show Capture Rect` and then change the values of `Source Capture Offset` until the character fits within the blue box. Increase the `Render Texture Size` if needed.

**Issue** A character appears twice on the screen  
**Solution** Make sure that your main camera and other cameras have the `EntityPostProcessor` layer removed the `Culling Mask`.

**Issue** How do I refence the _RenderSource_ from the _Entity_ and vice versa?  
**Solution** Both components keep a reference to the other. If you have a attach a script `MyEntity` to your entity and a script `MyRenderSource` to your render source:
`MyEntity`
```C#
using EntityPostProcessor;
public class MyEntity : MonoBehaviour
{
    MyRenderSource renderSource;
    void Start () {
        renderSource = GetComponent<Entity>().renderSource.GetComponent<MyRenderSource>();
    }
}
```

`MyRenderSource`
```C#
using EntityPostProcessor;
public class MyRenderSource : MonoBehaviour
{
    MyEntity entity;
    void Start () {
        entity = GetComponent<EntityRenderSource>().entity.GetComponent<MyEntity>();
    }
}
```

 
