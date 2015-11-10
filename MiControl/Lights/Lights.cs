using System;

namespace MiControl
{
	/// <summary>
	/// Base class for lightbulb types. Used for <see cref="RGBWLights"/>,
	/// <see cref="WhiteLights"/> and <see cref="RGBLights"/>.
	/// </summary>
	public abstract class Lights
	{
		internal MiController Controller;
		internal int ActiveGroup;
		
		#region Constructor
		
		public Lights(MiController controller)
		{
			this.Controller = controller;
		}
		
		#endregion
		
		#region Abstract Methods
		
		/// <summary>
		/// Each lightbulb has a 'SwitchOn' method, must be declared as an
		/// abstract method to be used in the 'SelectGroup' method.
		/// </summary>
		public abstract void SwitchOn(int group);
		
        #endregion
        
        
        #region Helper Methods
		
		/// <summary>
        /// Checks if the specified group is between 0 and 4.
        /// Throws an Exception otherwise.
        /// </summary>
        /// <param name="group">The group to check.</param>
        internal static void CheckGroup(int group)
        {
			const int lower = 0;
			const int upper = 4;
        	
			if (group < lower || group > upper) {
				throw new ArgumentOutOfRangeException("group", group, "Value must be between " + lower + " and " + upper);
			}
        }
        
        /// <summary>
        /// Method for selecting (sending on command) and setting
        /// the 'ActiveGroup'. Implementation differs per lightbulb type.
        internal void SelectGroup(int group)
        {
        	// Send 'on' to select correct group if it 
			// is not the currently selected group
			if (ActiveGroup != group) {
				SwitchOn(group);
				ActiveGroup = group;
			}
        }
        
        /// <summary>
        /// Converts a percentage value (0 - 100) to a byte value between 2 and 27
        /// for use in MiLight commands.
        /// </summary>
        /// <param name="percentage">Brightness percentage (0 - 100) to convert.</param>
        /// <returns>A byte value between 2 and 27. Or 0 when percentage is 0.</returns>
        internal static byte BrightnessToMiLight(int percentage)
        {
        	// Instead of throwing an exception when out of bounds ( <0 - >100 )
        	// just correct the value
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
