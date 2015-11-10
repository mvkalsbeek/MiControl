using System;

namespace MiControl
{
	/// <summary>
	/// Base class for lightbulb types. Used for <see cref="RGBLights"/>.
	/// Is extended by the <see cref="NewLights"/> class to supply 'group'
	/// handling for see <see cref="RGBWLights"/> and <see cref="WhiteLights"/>
	/// </summary>
	public abstract class Lights
	{
		internal Controller Controller; // All lights need a parent Controller
		
		#region Constructor
		
 		internal Lights(Controller controller)
		{
			this.Controller = controller;
		}
		
		#endregion
		
		#region Abstract Methods
		
		/// <summary>
		/// Switches 'on' the light bulbs.
		/// </summary>
		public abstract void SwitchOn();
		
		/// <summary>
		/// Switches 'off' the light bulbs.
		/// </summary>
		public abstract void SwitchOff();
		
		#endregion
		
		
		#region Helper Methods
		
		/// <summary>
		/// Converts a percentage value (0 - 100) to a byte value between 2 and 27
		/// for use in MiLight commands.
		/// </summary>
		/// <param name="percentage">Brightness percentage (0 - 100) to convert.</param>
		/// <returns>A byte value between 2 and 27. Or 0 when percentage is 0.</returns>
		internal static byte BrightnessToMiLight(int percentage)
		{
			// Instead of throwing an exception when 
			// out of bounds ( <0 - >100 ), just correct the value
			if (percentage <= 0) {
				return 0x00;
			}
			if (percentage > 100) {
				percentage = 100;
			}
			
			return (byte)((percentage / 4) + 2);
		}

		/// <summary>
		/// Calculates the MiLight color value from a given Hue.
		/// </summary>
		/// <param name="hue">The Hue (in degrees from 0 - 360) to convert.</param>
		/// <returns>Returns a byte for use in MiLight command.</returns>
		internal static byte HueToMiLight(float hue)
		{
			return (byte)((256 + 176 - (int)(hue / 360.0 * 255.0)) % 256);
		}
		
		#endregion
	}
}
