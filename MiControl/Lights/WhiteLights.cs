using System;

namespace MiControl.Lights
{
	/// <summary>
	/// Class with commands for controlling White lightbulbs. 
	/// </summary>
	public class WhiteLights : GroupLights
	{
		#region Constants
		
		static readonly byte[] ON = { 0x45, 0x38, 0x3D, 0x37, 0x32 };
		static readonly byte[] OFF = { 0x39, 0x3B, 0x33, 0x3A, 0x36 };
		static readonly byte BRIGHTUP = 0x3C;
		static readonly byte BRIGHTDOWN = 0x34;
		static readonly byte WARMER = 0x3E;
		static readonly byte COOLER = 0x3F;
		
		#endregion
		
		#region Constructor
		
		/// <summary>
		/// Creates an instance of the WhiteLights class to send commands
		/// to White lightbulbs. Must be supplied with the (parent) controller.
		/// </summary>
		/// <param name="controller">The MiController class to use as parent.</param>
		public WhiteLights(Controller controller) : base(controller) {}
		
		#endregion
		
		
		#region White Methods
		// Oh, scheisse...
		
		/// <summary>
		/// Switch 'on' one or all white lightbulbs. Can be used to
		/// link bulbs to a group (first time setup).
		/// <para>
		/// Linking can be done by sending this command within three seconds
		/// after powering a lightbulb up the first time (or when unlinked).
		/// It is advised however to link the lightbulbs the first time by using
		/// the MiLight phone app.
		/// </para>
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
		/// Switches 'off' one or all white lightbulbs.
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
		/// Turns up the brightness for the specified group of
		/// white lightbulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void BrightnessUp(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { BRIGHTUP, ZERO, END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Turns down the brightness for the specified group of
		/// white lightbulbs.
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void BrightnessDown(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { BRIGHTDOWN, ZERO, END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Turns up the temperature of a group of white lights (warmer).
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void Warmer(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { WARMER, ZERO, END };
			
			Controller.SendCommand(command);
		}
		
		/// <summary>
		/// Turns down the temperature of a group of white lights (cooler).
		/// </summary>
		/// <param name="group">1-4 or 0 for all groups.</param>
		public void Cooler(int group)
		{
			CheckGroup(group); // Check and select
			SelectGroup(group);
			
			var command = new [] { COOLER, ZERO, END };
			
			Controller.SendCommand(command);
		}

		#endregion
	}
}
