using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;

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
        private int RGBActiveGroup;
        private int whiteActiveGroup;

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


        #region RGB Methods

        /// <summary>
        /// Switches a specified group of RGB bulbs on. Can be used to 
        /// link bulbs to a group (first time setup).
        ///
        /// Linking can be done by sending this command within three seconds
        /// after powering a lightbulb up the first time (or when unlinked).
        /// It is advised however to link the lightbulbs the first time by using
        /// the MiLight phone app.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSwitchOn(int group) 
        {
        	CheckGroup(group); // Just check

            var groups = new byte[] { 0x42, 0x45, 0x47, 0x49, 0x4B };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches a specified group or all RGB bulbs off.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSwitchOff(int group)
        {
        	CheckGroup(group); // Just check

            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches the specified group or all RGB bulbs to white.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSwitchWhite(int group)
        {
            CheckGroup(group); // Just check

            var groups = new byte[] { 0xC2, 0xC5, 0xC7, 0xC9, 0xCB };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Sets the brightness for a group or all RGB bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="percentage">The percentage (0-100) of brightness to set.</param>
        public void RGBSetBrightness(int group, int percentage)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);

            var command = new byte[] { 0x4E, BrightnessToMiLight(percentage), 0x55 };
            
            SendCommand(command);
        }
        
        /// <summary>
        /// Sets the 'Night' mode for the specified group or all RGB bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSetNightMode(int group)
        {
        	CheckGroup(group); // Just check
            
            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            var night = new byte[] { 0xC1, 0xC6, 0xC8, 0xCA, 0xCC };
            var command = new byte[] { groups[group], night[group], 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Sets a given group of RGB bulbs to the specified hue.
        /// 
        /// MiLight bulbs do not support Saturation and Luminosity/Brightness.
        /// Use 'RGBSetColor' for a true representation of the color.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="hue">The hue to set (0 - 360 degrees).</param>
        public void RGBSetHue(int group, float hue)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);
        	
            var command = new byte[] { 0x40, HueToMiLight(hue), 0x55 };
            
            SendCommand(command);
        }
        
        /// <summary>
        /// Sets a group of RGB bulbs to a realistic representation of the color
        /// by also setting the brightness of the bulbs and switching to white light.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="color">The 'System.Drawing.Color' to set.</param>
        public void RGBSetColor(int group, Color color)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);
            
            var saturation = (int)(color.GetSaturation() * 100);
            var brightness = (int)(color.GetBrightness() * 100);
            
            // If the saturation is below 15% or brightness is below 15%, 
            // set white light and set the brightness. Otherwise set the hue
            // and brightness.
            if(saturation < 15 || brightness < 15) {
            	RGBSwitchWhite(group);
            } else {
            	RGBSetHue(group, color.GetHue());
            }
            
            RGBSetBrightness(group, brightness);
        }
        
        /// <summary>
        /// Switches the 'disco' mode of a group or all RGB bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBCycleMode(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);
        	
        	var command = new byte[] { 0x4D, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        /// <summary>
        /// Speeds up the current effect for a group or all RGB bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSpeedUp(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);
        	
        	var command = new byte[] { 0x44, 0x00, 0x55 };
        	
        	SendCommand(command);
        }
        
        public void RGBSpeedDown(int group)
        {
        	CheckGroup(group); // Check and select
        	SelectGroup(group);
        	
        	var command = new byte[] { 0x43, 0x00, 0x55 };
        	
        	SendCommand(command);
        }

        #endregion

        #region White Methods
        // Oh, scheisse...
        
        // TODO: Methods for controlling White MiLight bulbs.

        #endregion


        #region Private Methods
        
        /// <summary>
        /// Sends a command to the MiLight WiFi controller.
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
		private void SelectGroup(int group)
		{
			// Send 'on' to select correct group if it 
			// is not the currently selected group
			if (RGBActiveGroup != group) {
				RGBSwitchOn(group);
				RGBActiveGroup = group;
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
