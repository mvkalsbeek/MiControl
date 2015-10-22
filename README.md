## MiControl

C# application/library for controlling MiLight WiFi enabled lightbulbs. It is currently in development.

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Milfje/MiControl?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge) Ideas, questions? Hit me up!

| Build status |
|--------------|
| [![Build Status](https://travis-ci.org/Milfje/MiControl.svg?branch=master)](https://travis-ci.org/Milfje/MiControl) Mono 3.8.0 |

### What is it?

MiControl is an application/library for controlling <a href="http://www.milight.com/">MiLight</a> light bulbs connected to a MiLight WiFi controller, written in C#. It has a user interface (MiControlGUI) for directly controlling, and a library (<a href="http://github.com/Milfje/MiControl/wiki/MiControl">MiControl</a>) for writing your own applications to control MiLight light bulbs.

The focus in development is currently on the MiControl library. The GUI is constantly undergoing changes to facilitate testing the library.

See the <a href="https://github.com/Milfje/MiControl/wiki">wiki</a> for more information about using MiControl.

### How to use it?

The aim of MiControl is to be as easy in usage as possible. For this reason, one part of the application will be a GUI to allow easy end-user access to controlling MiLight light bulbs. The MiControl library for controlling these lamps is kept as simple as possible in implementation as well.

Switching a lamp on can be done in just two lines of code. First you connect to the MiLight WiFi controller, and then you simply send the desired command.

    var controller = new MiController("192.168.0.123");   // Connect to WiFi controller
    controller.RGBSwitchOn(3);                            // Switch 'on' group 3

For more information on using the GUI, or <a href="https://github.com/Milfje/MiControl/wiki/MiControl">using the MiControl library</a>, see the <a href="https://github.com/Milfje/MiControl/wiki">wiki</a>

### What will it become?

The goal of this project is to create an application/library with advanced functions to make use of the full potential of these cheap, wirelessly controllable lights. Functions that will be implemented are for example:

* Simple switching and changing color.
* Setting the color of the light to match your screen (Ambilight functionality).
* Synchronising color changes to music (Audio visualisation).
* Scheduling switching/color changing (Visual alarm clock).
* Switching by rules (i.e. turn lights on when phone enters local WiFi).

By using the code of the MiControl library, you could create anything to control these lights!
