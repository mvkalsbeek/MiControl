using System;

namespace MiControl
{
	/// <summary>
	/// Base class for new lightbulb types. Used for <see cref="RGBWLights"/> an <see cref="WhiteLights"/>.
	/// </summary>
	public abstract class GroupLights : Lights
	{		
		internal int ActiveGroup;
		
		#region Constructor
		
		internal GroupLights(Controller controller) : base(controller) {}
		
		#endregion
		
		#region Abstract Methods

		/// <summary>
		/// Each light type has a 'SwitchOn' method, must be declared as an
		/// abstract method to be used in the 'SelectGroup' method.
		/// </summary>
		public abstract void SwitchOn(int group);
		
		/// <summary>
		/// Switches 'on' all groups of lightbulbs.
		/// </summary>
		public override void SwitchOn()
		{
			SwitchOn(0);
		}
		
		/// <summary>
		/// Each light type has a 'SwitchOff' method.
		/// </summary>
		public abstract void SwitchOff(int group);
		
		/// <summary>
		/// Switches 'off' all groups of lightbulbs.
		/// </summary>
		public override void SwitchOff()
		{
			SwitchOff(0);
		}
		
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
        /// </summary>
        /// <param name="group">The group to select.</param>
        internal void SelectGroup(int group)
        {
        	// Send 'on' to select correct group if it 
			// is not the currently selected group
			if (ActiveGroup != group) {
				SwitchOn(group);
				ActiveGroup = group;
			}
        }
        
        #endregion
	}
}
