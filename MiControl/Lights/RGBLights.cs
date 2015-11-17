using System;
using System.Drawing;

namespace MiControl.Lights
{
	/// <summary>
	/// Class with commands for controlling RGB lightbulbs. 
	/// </summary>
	public class RGBLights : BaseLights // 'Legacy' lights without group support
	{
		#region Constants
		
		static readonly byte ON = 0x22;
		static readonly byte OFF = 0x21;
		static readonly byte BRIGHTUP = 0x23;
		static readonly byte BRIGHTDOWN = 0x24;
		static readonly byte HUE = 0x20;
		static readonly byte NEXTEFFECT = 0x27;
		static readonly byte PREVEFFECT = 0x28;
		static readonly byte SPEEDUP = 0x25;
		static readonly byte SPEEDDOWN = 0x26;
		
		#endregion
		
		#region Constructor
		
		/// <summary>
		/// Creates an instance of the RGBLights class to send commands
		/// to RGB lightbulbs. Must be supplied with the (parent) controller.
		/// </summary>
		/// <param name="controller">The parent <see cref="Controller"/> of this light.</param>
		public RGBLights(Controller controller) : base(controller) {}
		
		#endregion
		
		
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
        	var command = new [] { ON, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches 'off' the RGB bulb(s)/strip(s). Groups are not used in the
        /// context of RGB bulb(s)/strip(s), so this is default set to 0 and does
        /// not have to be specified.
        /// </summary>
        public override void SwitchOff()
        {
        	var command = new [] { OFF, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Turns up the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void BrightnessUp()
        {
        	var command = new [] { BRIGHTUP, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Turns down the brightness of the RGB bulb(s)/strip(s)
        /// </summary>
        public void BrightnessDown()
        {
        	var command = new [] { BRIGHTDOWN, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Sets the give hue of the RGB bulb(s)/strip(s)
        /// </summary>
        /// <param name="hue">Hue in 0.0 - 360.0 degrees.</param>
        public void SetHue(float hue)
        {
        	var command = new [] { HUE, HueToMiLight(hue), END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Sets the color of the RGB bulb(s)/strips(s). Translates
        /// to hue, no brightness or saturation taken in consideration.
        /// </summary>
        /// <param name="color">The 'System.Drawing.Color' to set.</param>
        public void SetColor(Color color)
        {
        	var command = new [] { HUE, HueToMiLight(color.GetHue()), END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the next effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void NextEffect()
        {
        	var command = new [] { NEXTEFFECT, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Switches to the previous effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void PreviousEffect()
        {
        	var command = new [] { PREVEFFECT, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Speeds up the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void SpeedUp()
        {
        	var command = new [] { SPEEDUP, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        /// <summary>
        /// Speeds down the current effect of the RGB bulb(s)/strip(s)
        /// </summary>
        public void SpeedDown()
        {
        	var command = new [] { SPEEDDOWN, ZERO, END };
        	Controller.SendCommand(command);
        }
        
        #endregion
	}
}
