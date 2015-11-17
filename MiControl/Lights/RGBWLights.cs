using System;
using System.Drawing;

namespace MiControl.Lights
{
	/// <summary>
	/// Class with commands for controlling RGBW lightbulbs. 
	/// </summary>
	public class RGBWLights : GroupLights
	{		
		#region Constants
		
		static readonly byte[] ON = { 0x42, 0x45, 0x47, 0x49, 0x4B };
		static readonly byte[] OFF = { 0x41, 0x46, 0x48, 0x4A, 0x4C };
		static readonly byte[] WHITE = { 0xC2, 0xC5, 0xC7, 0xC9, 0xCB };
		static readonly byte[] NIGHT = { 0xC1, 0xC6, 0xC8, 0xCA, 0xCC };
		static readonly byte BRIGHTNESS = 0x4E;
		static readonly byte HUE = 0x40;
		static readonly byte MODE = 0x4D;
		static readonly byte SPEEDUP = 0x44;
		static readonly byte SPEEDDOWN = 0x43;
		
		#endregion
		
		
		#region Constructor
		
		/// <summary>
		/// Creates an instance of the RGBWLights class to send commands
		/// to RGBW lightbulbs. Must be supplied with the (parent) controller.
		/// </summary>
		/// <param name="controller">The MiController class to use as parent.</param>
		public RGBWLights(Controller controller) : base(controller) {}
		
		#endregion
		
		
		#region RGBW Methods

		/// <summary>
		/// Switches a specified group of RGBW bulbs on. Can be used to 
		/// link bulbs to a group (first time setup).
		/// <para>
		/// Linking can be done by sending this command within three seconds
		/// after powering a lightbulb up the first time (or when unlinked).
		/// It is advised however to link the lightbulbs the first time by using
		/// the MiLight phone app.
		/// </para
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public override void SwitchOn(int group) 
		{
			CheckGroup(group); // Just check

			var command = new [] { ON[group], ZERO, END };
			
			Controller.SendCommand(command);
			
			ActiveGroup = group;
		}

		/// <summary>
		/// Switches a specified group or all RGBW bulbs off.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public override void SwitchOff(int group)
		{
			CheckGroup(group); // Just check

			var command = new [] { OFF[group], ZERO, END };
			
			Controller.SendCommand(command);
			
			ActiveGroup = group;
		}

		/// <summary>
		/// Switches the specified group or all RGBW bulbs to white.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void SwitchWhite(int group)
		{
			CheckGroup(group); // Just check

			var command = new [] { WHITE[group], ZERO, END };
			
			Controller.SendCommand(command);
			
			ActiveGroup = group;
		}
		
		/// <summary>
		/// Sets the 'Night' mode for the specified group or all RGBW bulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void SetNightMode(int group)
		{
			CheckGroup(group); // Just check
			
			var command = new byte[] { OFF[group], NIGHT[group], END };
			
			Controller.SendCommand(command);
			
			ActiveGroup = group;
		}

		/// <summary>
		/// Sets the brightness for a group or all RGBW bulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		/// <param name="percentage">The percentage (0-100) of brightness to set.</param>
		public void SetBrightness(int group, int percentage)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new byte[] { BRIGHTNESS, BrightnessToMiLight(percentage), END };
			
			Controller.SendCommand(command);
		}

		/// <summary>
		/// Sets a given group of RGBW bulbs to the specified hue.
		/// </summary>
		/// <remarks>
		/// MiLight bulbs do not support Saturation and Luminosity/Brightness.
		/// Use <see cref="SetTrueColor"/> for a beter representation of the color.
		/// </remarks>
		/// <param name="group">1-4 or 0 for all groups.</param>
		/// <param name="hue">The hue to set (0 - 360 degrees).</param>
		public void SetHue(int group, float hue)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { HUE, HueToMiLight(hue), END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Sets a given group of RGBW bulbs to the specified color's hue. Does not 
		/// support Saturation and Luminosity/Brightness of the color.
		/// For a better representation of the color use <see cref="SetTrueColor"/>.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		/// <param name="color">The <see cref="System.Drawing.Color"/> to set.</param>
		public void SetColor(int group, Color color)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { HUE, HueToMiLight(color.GetHue()), END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Sets a group of RGBW bulbs to a realistic representation of the color
		/// by also setting the brightness of the bulbs and switching to white light
		/// when neccesary (very bright or very dull color).
		/// </summary>
		/// <remarks>
		/// Switches to white when saturation or brightness is below 15%. Also
		/// sets the brightness of the light according to the colors intensity.
		/// </remarks>
		/// <param name="group">1-4 or 0 for all groups.</param>
		/// <param name="color">The <see cref="System.Drawing.Color"/> to set.</param>
		public void SetTrueColor(int group, Color color)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var saturation = (int)(color.GetSaturation() * 100);
			var brightness = (int)(color.GetBrightness() * 100);
			
			// If the saturation is below 15% or brightness is below 15%, 
			// set white light and set the brightness. Otherwise set the hue
			// and brightness.
			if(saturation < 15 || brightness < 15) {
				SwitchWhite(group);
			} else {
				SetHue(group, color.GetHue());
			}
			
			SetBrightness(group, brightness);
		}
		
		/// <summary>
		/// Switches the 'disco' mode of a group or all RGBW bulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void CycleMode(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { MODE, ZERO, END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Speeds up the current effect for a group or all RGBW bulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void SpeedUp(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { SPEEDUP, ZERO, END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Speeds down the current effect for a group or all RGBW bulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void SpeedDown(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { SPEEDDOWN, ZERO, END };
			
			Controller.SendCommand(command);
		}

		#endregion
	}
}
