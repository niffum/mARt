# mARt : Interactive visualisation of MRI data in AR

This application was implemented as part of my master thesis in media information technology. 
The thesis paper can be found here: https://github.com/niffum/Masterarbeit

It contains a AR and VR application to view and interact with MRI data. 
The motivation is to investigate the benefits of an AR application in the field of stroke diagnosis and treatment. 
The included datasets show MRI scans of brains after a stroke. The affected area was marked using masks.
Using these it can be displayed in the volume renderings of the brains. 

The volume rendering was realised using: https://github.com/mattatz/unity-volume-rendering

Requirements: 

- Unity (Version 2017.4 or higher)
- Leap Motion SDK Orion
- Steam VR
- VR/AR system 
- Leap Motion

## Use VR application on HoloLens 

- Make sure volume texture assets are created and referenced (see: Create volume textures)
- Connect VR system to the computer
- Switch to "PC, MAC & Linux Standalone" in Build Setting of the project
- Go to Main/Scenes/Preload.unity
- Put "main_2D" as First Scene Name in appState GameObject 
- Start Application from editor

## Use AR application on HoloLens 

To use the application on HoloLens it has to be started from the editor and streamed on it. 
To do this do the following:
- Istall and start the "Holographic Remoting Player" app on HoloLens
- Switch to "Universial Windows Platform" in Build Setting of the project
- Choose "HoloLens" as Target Device
- Open Window>Holographic
- Choose "Remote to Device" as Emulation Mode
- Put IP adress of HoloLens in the "Remote Machine" field (It is displayed on HoloLens when "Holographic Remoting Player" is startet)
- Press "Connect"
- Make sure volume texture assets are created and referenced (see: Create volume textures)
- Go to Main/Scenes/Preload.unity
- Put "main_2D_AR" as First Scene Name in appState GameObject 
- Start Application from editor

## Create volume textures

Due to their size the volume textures which will be rendered are not included in the project.
To create and use them do the following:

- Go to 3DUI/Load3DTexture.unity.
- In the LoadMRTTo3DTexture GameObject put the path to the MRI data. Paths of the datasets are:
	
	Assets/StreamingAssets/MRTImages/01
	and
	Assets/StreamingAssets/MRTImages/02

	The path of the masks to display the stroke affected area are:

	Assets/StreamingAssets/Masks/01
	and
	Assets/StreamingAssets/Masks/02

- Start the scene. The volumes will be saved in the 3DTextures folder as assets.
- Go to 3DUI/Scenes/main_3D.unity.
- In the GameObject VolumeAndUiParent>VolumeParent>Volume_Primary>VolumeRendering reference the volume asset as Volume and the mask volume asset as Volume Mask.
- Do the same for the GameObject VolumeAndUiParent>VolumeParent>Volume_Secondary>VolumeRendering.
- Repeat the last two steps for the scene 3DUI/Scenes/main_3D_AR.unity.

To just display the Rendering without the interactions go to VolumeRendering/Demo.unity.
Reference the volume assets in the VolumeRendering GameObject like described above.
