using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MiControl
{
    /// <summary>
    /// Class for controlling a MiLight WiFi controller box. Must be instantiated
    /// by supplying the IP-address of the controller. Commands for specified groups
    /// or all lightbulbs can then be sent using the methods for RGB or white lightbulbs.
    ///
    /// The controller must be linked to the local WiFi network and lightbulbs must 
    /// first be linked to groups with the MiLight phone app.
    ///
    /// See http://github.com/Milfje/MiControl/wiki for more information.
    /// </summary>
    public class MiController
    {
    	#region Properties
    	
    	/// <summary>
    	/// Returns the IP address of this controller.
    	/// </summary>
    	public string IP {
    		get { return _ip; }
    	}
    	
    	/// <summary>
    	/// Set this to 'false' to manually implement a delay between commands. 
    	/// By default, a 50ms 'Thread.Sleep()' is executed between commands to
    	/// prevent command dropping by the WiFi controller.
    	/// </summary>
    	public bool AutoDelay = true;
    	
    	#endregion
    	
        #region Private Variables

        private UdpClient Controller; // Handles communication with the controller
        private string _ip;
        private int RGBWActiveGroup;
        private int WhiteActiveGroup;

        #endregion


        #region Constructor

        /// <summary>
        /// Constructs a new MiLight controller class. Is used for a single
        /// MiLight WiFi controller.
        /// </summary>
        /// <param name="ip">The IP-address of the MiLight WiFi controller</param>
        public MiController(string ip)
        {
            Controller = new UdpClient(ip, 8899);
            _ip = ip;
        }

        #endregion

        
        
        #region Static Methods

        // HACK: I only have one controller, this needs to be tested with multiple controllers.

        /// <summary>
        /// Broadcasts an UDP message to discover MiLight WiFi controller(s) on the
        /// local network. Freezes the current thread for +- 1 second.
        /// </summary>
        /// <returns>A list of MiController instances.</returns>
        public static List<MiController> Discover()
        {
            // Create a UDP client for discovering MiLight WiFi controller(s)
            var ep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 48899);
            var udp = new UdpClient();
            udp.EnableBroadcast = true;

            // Set up the broadcast to send
            var broadcast = System.Text.Encoding.UTF8.GetBytes("Link_Wi-Fi");

            // Send the broadcast 20 times with a 50ms interval (will take 1 second)
            for(int i = 0; i < 20; i++) {
                udp.Send(broadcast, broadcast.Length, ep);
                Thread.Sleep(50); // Sleep 50ms between broadcasts
            }

            // Receive possible responses from MiLight WiFi controller(s)
            var received = udp.Receive(ref ep);

            // Create MiController instances and return them
            var controllers = new List<MiController>();
            var ips = ep.ToString().Split(':');
            for (int i = 0; i < ips.Length; i+=2) {
                controllers.Add(new MiController(ips[i]));
            }

            return controllers;
        }

        #endregion

        

        #region RGBW Methods

        /// <summary>
        /// Switches a specified group of RGBW bulbs on. Can be used to 
        /// link bulbs to a group (first time setup).
        ///
        /// Linking can be done by sending this command within three seconds
        /// after powering a lightbulb up the first time (or when unlinked).
        /// It is advised however to link the lightbulbs the first time by using
        /// the MiLight phone app.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSwitchOn(int group) 
        {
        	CheckGroup(group); // Just check

            var groups = new byte[] { 0x42, 0x45, 0x47, 0x49, 0x4B };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBWActiveGroup = group;
        }

        /// <summary>
        /// Switches a specified group or all RGBW bulbs off.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSwitchOff(int group)
        {
        	CheckGroup(group); // Just check

            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBWActiveGroup = group;
        }

        /// <summary>
        /// Switches the specified group or all RGBW bulbs to white.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSwitchWhite(int group)
        {
            CheckGroup(group); // Just check

            var groups = new byte[] { 0xC2, 0xC5, 0xC7, 0xC9, 0xCB };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBWActiveGroup = group;
        }

        /// <summary>
        /// Sets the brightness for a group or all RGBW bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="percentage">The percentage (0-100) of brightness to set.</param>
        public void RGBWSetBrightness(int group, int percentage)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);

            var command = new byte[] { 0x4E, BrightnessToMiLight(percentage), 0x55 };
            
            SendCommand(command);
        }
        
        /// <summary>
        /// Sets the 'Night' mode for the specified group or all RGBW bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSetNightMode(int group)
        {
        	CheckGroup(group); // Just check
            
            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            var night = new byte[] { 0xC1, 0xC6, 0xC8, 0xCA, 0xCC };
            var command = new byte[] { groups[group], night[group], 0x55 };
            
            SendCommand(command);
            
            RGBWActiveGroup = group;
        }

        /// <summary>
        /// Sets a given group of RGBW bulbs to the specified hue.
        /// 
        /// MiLight bulbs do not support Saturation and Luminosity/Brightness.
        /// Use 'RGBSetColor' for a beter representation of the color.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="hue">The hue to set (0 - 360 degrees).</param>
        public void RGBWSetHue(int group, float hue)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);
        	
            var command = new byte[] { 0x40, HueToMiLight(hue), 0x55 };
            
            SendCommand(command);
        }
        
        /// <summary>
        /// Sets a group of RGBW bulbs to a realistic representation of the color
        /// by also setting the brightness of the bulbs and switching to white light
        /// when neccesary (very bright or very dull color).
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="color">The 'System.Drawing.Color' to set.</param>
        public void RGBWSetColor(int group, Color color)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);
            
            var saturation = (int)(color.GetSaturation() * 100);
            var brightness = (int)(color.GetBrightness() * 100);
            
            // If the saturation is below 15% or brightness is below 15%, 
            // set white light and set the brightness. Otherwise set the hue
            // and brightness.
            if(saturation < 15 || brightness < 15) {
            	RGBWSwitchWhite(group);
            } else {
            	RGBWSetHue(group, color.GetHue());
            }
            
            RGBWSetBrightness(group, brightness);
        }
        
        /// <summary>
        /// Switches the 'disco' mode of a group or all RGBW bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWCycleMode(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);
        	
        	var command = new byte[] { 0x4D, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Speeds up the current effect for a group or all RGBW bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSpeedUp(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);
        	
        	var command = new byte[] { 0x44, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Speeds down the current effect for a group or all RGBW bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBWSpeedDown(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectRGBWGroup(group);
        	
        	var command = new byte[] { 0x43, 0x00, 0x55 };
        	
        	SendCommand(command);
        }

        #endregion

        
        
        // These should work for previous generation, single channel bulbs.
        // Perhaps these work for LED strip controllers as well (needs to be tested)...
        #region RGB Methods (legacy)
        
        /// <summary>
        /// Switch 'on' the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBSwitchOn()
        {
        	var command = new byte[] { 0x22, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Switches 'off' the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBSwitchOff()
        {
        	var command = new byte[] { 0x21, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Turns up the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBBrightnessUp()
        {
        	var command = new byte[] { 0x23, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Turns down the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBBrightnessDown()
        {
        	var command = new byte[] { 0x24, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Sets the give hue of the RGB bulb(s)/strip(s)
        /// </summary>
        /// <param name="hue">Hue in 0.0 - 360.0 degrees.</param>
        public void RGBSetHue(float hue)
        {
        	var command = new byte[] { 0x20, HueToMiLight(hue), 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Sets the color of the RGB bulb(s)/strips(s). Translates
        /// to hue, no brightness or saturation taken in consideration.
        /// </summary>
        /// <param name="color">The 'System.Drawing.Color' to set.</param>
        public void RGBSetColor(Color color)
        {
        	var command = new byte[] { 0x20, HueToMiLight(color.GetHue()), 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the next effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBNextEffect()
        {
        	var command = new byte[] { 0x27, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the previous effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBPreviousEffect()
        {
        	var command = new byte[] { 0x28, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Speeds up the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBSpeedUp()
        {
        	var command = new byte[] { 0x25, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        /// <summary>
        /// Speeds down the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void RGBSpeedDown()
        {
        	var command = new byte[] { 0x26, 0x00, 0x55 };
        	SendCommand(command);
        }
        
        #endregion
        
        
        
        #region White Methods
        // Oh, scheisse...
        
        /// <summary>
        /// Switch 'on' one or all white lightbulbs. Can be used to 
        /// link bulbs to a group (first time setup).
        ///
        /// Linking can be done by sending this command within three seconds
        /// after powering a lightbulb up the first time (or when unlinked).
        /// It is advised however to link the lightbulbs the first time by using
        /// the MiLight phone app.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteSwitchOn(int group)
        {
        	CheckGroup(group); // Just check
        	
        	var groups = new byte[] { 0x45, 0x38, 0x3D, 0x37, 0x32 };
        	var command = new byte[] { groups[group], 0x00, 0x55 };
        	
        	SendCommand(command);
        	
        	WhiteActiveGroup = group;
        }
        
        /// <summary>
        /// Switches 'off' one or all white lightbulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteSwitchOff(int group)
        {
        	CheckGroup(group); // Just check
        	
        	var groups = new byte[] { 0x39, 0x3B, 0x33, 0x3A, 0x36 };
        	var command = new byte[] { groups[group], 0x00, 0x55 };
        	
        	SendCommand(command);
        	
        	WhiteActiveGroup = group;
        }
        
        /// <summary>
        /// Turns up the brightness for the specified group of
        /// white lightbulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteBrightnessUp(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectWhiteGroup(group);
        	
        	var command = new byte[] { 0x3C, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Turns down the brightness for the specified group of
        /// white lightbulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteBrightnessDown(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectWhiteGroup(group);
        	
        	var command = new byte[] { 0x34, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Turns up the temperature of a group of white lights (warmer).
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteWarmer(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectWhiteGroup(group);
        	
        	var command = new byte[] { 0x3E, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Turns down the temperature of a group of white lights (cooler).
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void WhiteCooler(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectWhiteGroup(group);
        	
        	var command = new byte[] { 0x3F, 0x00, 0x55 };
        	
        	SendCommand(command);
        }

        #endregion

        

        #region Private Methods
        
        /// <summary>
        /// Sends a command to the MiLight WiFi controller. If 'AutoDelay' is true, will
        /// suspend the thread for 50ms.
        /// </summary>
        /// <param name="command">A 3 byte long array with the command codes to send.</param>
        private void SendCommand(byte[] command)
        {
        	Controller.Send(command, 3);
        	if(AutoDelay) {
        		Thread.Sleep(50); // Sleep 50ms to prevent command dropping
        	}
        }
        
        
        
        /// <summary>
        /// Sends an 'on' command to the specified group to make
        /// it the active group. Will not send a command if the last
        /// sent command is the active group.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
		private void SelectRGBWGroup(int group)
		{
			// Send 'on' to select correct group if it 
			// is not the currently selected group
			if (RGBWActiveGroup != group) {
				RGBWSwitchOn(group);
				RGBWActiveGroup = group;
			}
		}
		
		/// <summary>
        /// Sends an 'on' command to the specified group to make
        /// it the active group. Will not send a command if the last
        /// sent command is the active group.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
		private void SelectWhiteGroup(int group)
		{
			// Send 'on' to select correct group if it
			// is not the selected group
			if (WhiteActiveGroup != group) {
				WhiteSwitchOn(group);
				WhiteActiveGroup = group;
			}
		}

		
		
        /// <summary>
        /// Checks if the specified group is between 0 and 4.
        /// Throws an Exception otherwise.
        /// </summary>
        /// <param name="group">The group to check.</param>
        private static void CheckGroup(int group)
        {
            if (group < 0 || group > 4) {
                throw new Exception("Specified group must be between 0 and 4.");
            }
        }
		
        
        
        /// <summary>
        /// Converts a percentage value (0 - 100) to a byte value between 2 and 27
        /// for use in MiLight commands.
        /// </summary>
        /// <param name="percentage">Brightness percentage (0 - 100) to convert.</param>
        /// <returns>A byte value between 2 and 27. Or 0 when percentage is 0.</returns>
        private static byte BrightnessToMiLight(int percentage)
        {
        	if (percentage < 0 || percentage > 100) {
                throw new Exception("Brightness must be between 0 and 100");
            }
        	
        	if(percentage == 0) {
				return 0x00;
        	}
        	
        	return (byte)((percentage / 4) + 2);
        }

        /// <summary>
        /// Calculates the MiLight color value from a given Hue.
        /// </summary>
        /// <param name="hue">The Hue (in degrees from 0 - 360) to convert.</param>
        /// <returns>Returns a byte for use in MiLight command.</returns>
        private static byte HueToMiLight(float hue)
        {
            return (byte)((256 + 176 - (int)(hue / 360.0 * 255.0)) % 256);
        }

        #endregion
    }
}
