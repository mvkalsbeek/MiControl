using System;
using System.Drawing;
using System.Linq;
using System.Threading;
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
    	/// Set this to 'true' to manually implement a delay between commands. 
    	/// By default, a 50ms 'Thread.Sleep()' is executed between commands to
    	/// prevent command dropping by the WiFi controller.
    	/// </summary>
    	public bool ManualDelay = false;
    	
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

        // TODO: Method for finding MiLight WiFi controllers on the network.

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
            CheckGroup(group);

            var groups = new byte[] { 0x42, 0x45, 0x47, 0x49, 0x4B };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches a specified group of RGB bulbs off.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSwitchOff(int group)
        {
            CheckGroup(group);

            var groups = new byte[] { 0x41, 0x46, 0x48, 0x4A, 0x4C };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
            
            RGBActiveGroup = group;
        }

        /// <summary>
        /// Switches the specified group of RGB bulbs to white.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        public void RGBSwitchWhite(int group)
        {
            CheckGroup(group);

            var groups = new byte[] { 0xC2, 0xC5, 0xC7, 0xC9, 0xCB };
            var command = new byte[] { groups[group], 0x00, 0x55 };
            
            SendCommand(command);
        }

        /// <summary>
        /// Sets the brightness for a group of RGB bulbs.
        /// </summary>
        /// <param name="group">1-4 or 0 for all groups.</param>
        /// <param name="percentage">The percentage (0-100) of brightness to set.</param>
        public void RGBSetBrightness(int group, int percentage)
        {
            if (percentage < 0 || percentage > 100) {
                throw new Exception("Brightness must be between 0 and 100");
            }

            var brightness = new byte[]
            { 0x02,0x03,0x04,0x05,0x08,0x09,
              0x0A,0x0B,0x0D,0x0E,0x0F,0x10,
              0x11,0x12,0x13,0x14,0x15,0x17,
              0x18,0x19 };

            var index = (int)Math.Max(0, (Math.Ceiling((double)percentage / 100 * 19)) - 1);
            var command = new byte[] { 0x4E, brightness[index], 0x55 };
            
            SendCommand(command);
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
            // Send 'on' to select correct group if it 
            // is not the currently selected group
            if (RGBActiveGroup != group) {
                RGBSwitchOn(group);
                RGBActiveGroup = group;
            }
			
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
        	// Send 'on' to select correct group if it
        	// is not the currently selected group
            if (RGBActiveGroup != group) {
                RGBSwitchOn(group);
                RGBActiveGroup = group;
            }
            
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
        	if(!ManualDelay) {
        		Thread.Sleep(50); // Sleep 50ms to prevent command dropping
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
