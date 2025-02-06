#  Bottle Slingshot Shooter AR
A slingshot shooter AR game with virtual & real bottles

##  Description
Bottle Slingshot Shooter AR is an augmented reality (AR) shooter game developed in Unity, where the player uses a virtual slingshot to hit both virtual and real bottles, providing an interactive and immersive experience.

##  Key Features:
- Real-world bottle detection – the game recognizes and overlays visual elements on real bottles placed in the physical environment.
- Virtual bottles – automatically placed in the scene and required to progress.
- Real bottles (optional) – the player can arrange physical bottles, which are detected and visually marked (green for hit, red for missed).
- Interactive slingshot mechanics – controlled by touch gestures (drag and release).
- Scoring system – based on accuracy and speed.
- Integrated timer – players have 59 seconds to achieve the highest score.
- ⚠️ Missing Dependency - 
The project requires com.ptc.vuforia.engine-10.28.4.tgz, which is not included in the repository.

##  Screenshot

![picture alt](https://github.com/victorcb0/Bottle-Slingshot-Shooter-AR/blob/main/Image.png)

##  Demo Video
🔗 Watch the game demo
(https://www.youtube.com/shorts/W-HdxdbihbI)

##  Technologies Used
- Unity (AR Foundation, Vuforia for real-world object detection)
- C# for game logic
- Rigidbody & Colliders for physics simulation

##  Installation & Running
- Clone the repository:
```
git clone https://github.com/victorcb0/Bottle-Slingshot-Shooter-AR
cd Bottle-Slingshot-Shooter-AR
```
- Open the project in Unity (recommended version: 2022+).
- Download and add com.ptc.vuforia.engine-10.28.4.tgz to the Unity project.
- Run it on an ARCore/ARKit-compatible mobile device.
