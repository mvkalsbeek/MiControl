using System;
using System.Drawing;

namespace MiControl
{
	/// <summary>
	/// Class with commands for controlling RGB lightbulbs. 
	/// </summary>
	public class RGBLights : Lights // 'Legacy' lights without group support
	{
		#region Constructor
		
		/// <summary>
		/// Creates an instance of the RGBLights class to send commands
		/// to RGB lightbulbs. Must be supplied with the (parent) controller.
		/// </summary>
		/// <param name="controller"></param>
		public RGBLights(Controller controller) : base(controller) {}
		
		// These should work for previous generation, single channel bulbs.
        // Perhaps these work for LED strip controllers as well (needs to be tested)...
        #region RGB Methods (legacy)
        
        /// <summary>
        /// Switch 'on' the RGB bulb(s)/strip(s). Groups are not used in the
        /// context of RGB bulb(s)/strip(s), so this is default set to 0 and does
        /// not have to be specified.
        /// </summary>
        public override void SwitchOn()
        {
        	var command = new byte[] { 0x22, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches 'off' the RGB bulb(s)/strip(s). Groups are not used in the
        /// context of RGB bulb(s)/strip(s), so this is default set to 0 and does
        /// not have to be specified.
        /// </summary>
        public override void SwitchOff()
        {
        	var command = new byte[] { 0x21, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Turns up the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void BrightnessUp()
        {
        	var command = new byte[] { 0x23, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Turns down the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void BrightnessDown()
        {
        	var command = new byte[] { 0x24, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Sets the give hue of the RGB bulb(s)/strip(s)
        /// </summary>
        /// <param name="hue">Hue in 0.0 - 360.0 degrees.</param>
        public void SetHue(float hue)
        {
        	var command = new byte[] { 0x20, HueToMiLight(hue), 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Sets the color of the RGB bulb(s)/strips(s). Translates
        /// to hue, no brightness or saturation taken in consideration.
        /// </summary>
        /// <param name="color">The 'System.Drawing.Color' to set.</param>
        public void SetColor(Color color)
        {
        	var command = new byte[] { 0x20, HueToMiLight(color.GetHue()), 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the next effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void NextEffect()
        {
        	var command = new byte[] { 0x27, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the previous effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void PreviousEffect()
        {
        	var command = new byte[] { 0x28, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Speeds up the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void SpeedUp()
        {
        	var command = new byte[] { 0x25, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Speeds down the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void SpeedDown()
        {
        	var command = new byte[] { 0x26, 0x00, 0x55 };
        	Controller.SendCommand(command);
        }
        
        #endregion
		
		#endregion
	}
}
