This is a special version of XboxKeyboardMouse for Read Dead Redemption 2. Original project here:

https://github.com/fqlx/XboxKeyboardMouse

# Dec 2 2018 update

* Text file based key remapping supported
* Fixed bug holding down keys

# Major changes between this and the original:

* A separate deadzone when aiming
* Allow multiple keys to bind to one button
* Changed default controls

| Xbox Controller | Keyboard/Mouse |
| ------------- |:-----------------:|
| A | Space or Return    |
| B | B or T    |
| X | X or R    |
| Y | Y or F    |
| LeftShoulder |Q    |
| RightShoulder |E    |
| LeftStickClick |LeftShift or G    |
| RightStickClick |C or G    |
| Guide |~Tilde    |
| Menu |M    |
| View |V    |
| DPadUp |Up    |
| DPadDown |Down    |
| DPadLeft |Left    |
| DPadRight |Right    |

* Center mouse on Xbox app window to allow multimonitor
* Default Tick rate 10

Original README:


**Help support XboxKeyboardMouse by ⭐️'ing it! 😍**

# XboxKeyboardMouse
Keyboard and mouse for Xbox One streaming to Windows 10.

It sends keyboard and mouse inputs to an emulated Xbox controller using SCP driver.  

Note: This app only starts when the Xbox streaming app is in the foreground.

![Screenshot](https://cloud.githubusercontent.com/assets/6545688/23099037/50091be8-f655-11e6-838c-b94d7a62572b.PNG "Image of the GUI as of 11/12/16")

# Prerequisites
1. [You need SlimDX Runtime installed (x86)](https://slimdx.org/download.php)

# Installation
Simply run the installer: https://cdn.rawgit.com/fqlx/XboxKeyboardMouse/master/Installer/setup.exe
* The application will check for updates upon each successive launch.

# Current Layout
You may customize your own controller layout through the GUI but the default layout is:

| Keyboard/Mouse| Xbox Controller   |
| ------------- |:-----------------:|
| Space         | A                 |
| Left Ctrl     | B                 |
| R             | X                 |
| Number 1      | Y                 |
|               |                   |
| Q             | Right Bumper      |
| E             | Left Bumper       |
|               |                   |
| Left Shift    | Left Stick Click  |
| C             | Right Stick Click |
|               |                   |
| B             | Start/Menu        |
| V             | Select/Back/Nav   |
| Tilde         | Xbox Logo/Guide   |
|               |                   |
| Arrow Keys    | DPAD              |
|               |                   |
| Left Click    | Right Trigger     |
| Right Click   | Left Trigger      |
|               |                   |
| WASD          | Left Stick        |
| Mouse         | Right Stick       |
|               |                   |
| F12           | (Set Dead Zone)   |
| F11           | (Adjust Dead Zone)|

# Dead Zones
The default mouse engine has the ability to accommodate for a game's dead zone for effective, accurate mouse control.
However, XboxKeyboardMouse must be calibrated to learn the running game's dead zone. This takes a few seconds.
Once learned, the saved profile will retain the current dead zone size, reducing the need to recalibrate every time, if you frequently return to the same game or games that happen to use very similar dead zones.
To calibrate, follow these instructions, while running a game:
* Use keyboard controls for a moment to ensure your XboxKeyboardMouse is active.
* Press F12 to begin calibration. Wait with your finger over F12 and press it again when your aim starts moving.
* Press F11 to begin fine-tuning the calibration. Wait with your finger over F11 and press it again when your aim starts moving.
* (If aim started moving right away for the above step, you can repeat the step to try fine tuning again.)
At this point, XboxKeyboardMouse has learned the game's dead zone size, and even single-pixel mouse movements should now translate into tiny amounts of aim. If you switch games, or aiming doesn't feel natural yet, you can simply repeat the process to recalibrate.

# Lag
1.  Run ScpToolkit Settings Manager
2.  Use asynchronous HID Report Processing
3.  Disable Rumble and Native Feed

# Todo
* Check if SlimDX is installed and if not throw an understandable error.
* Scroll Wheel movement has not been added (Main usage maybe RTS).
