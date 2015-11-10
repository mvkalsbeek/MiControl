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
	public class Controller
	{
		#region Private Variables

		private UdpClient UDPController; // Handles communication with the controller
		private IPAddress _ip;
		private bool _autodelay = true;
		private int _delay = 50;

		#endregion
		
		#region Properties
		
		/// <summary>
		/// Returns the IP address of this controller.
		/// </summary>
		public IPAddress IP {
			get { return _ip; }
		}
		
		/// <summary>
		/// Set this to 'false' to manually implement a delay between commands. 
		/// By default, a 50ms 'Thread.Sleep()' is executed between commands to
		/// prevent command dropping by the WiFi controller.
		/// </summary>
		public bool AutoDelay {
			get { return _autodelay; }
			set { _autodelay = value; }
		}
		
		/// <summary>
		/// Gets and sets the delay in milliseconds between commands. Is only
		/// used when 'AutoDelay' is set to true. Default is set to 50ms.
		/// </summary>
		public int Delay {
			get { return _delay; }
			set { _delay = value; }
		}
		
		#endregion
		
		#region Lightbulbs
		
		RGBWLights _rgbw;
		/// <summary>
		/// Commands to control RGBW light bulbs.
		/// </summary>
		public RGBWLights RGBW {
			get { return _rgbw; }
		}
		
		WhiteLights _white;
		/// <summary>
		/// Commands to control White light bulbs.
		/// </summary>
		public WhiteLights White {
			get { return _white; }
		}
		
		RGBLights _rgb;
		/// <summary>
		/// The set of old (legacy) RGB light bulb(s)/strip(s).
		/// </summary>
		public RGBLights RGB {
			get { return _rgb; }
		}
		
		#endregion
	 

		#region Constructors
		
		/// <summary>
		/// Constructs a new MiLight controller class. Is used for a single
		/// MiLight WiFi controller.
		/// </summary>
		/// <param name="ip">The IP-address of the MiLight WiFi controller.</param>
		public Controller(IPAddress ip) : this(ip.ToString())
		{
		}
		
		/// <summary>
		/// Constructs a new MiLight controller class. Is used for a single
		/// MiLight WiFi controller.
		/// </summary>
		/// <param name="ip">The IP-address of the MiLight WiFi controller.</param>
		public Controller(string ip)
		{
			UDPController = new UdpClient(ip, 8899);
			_ip = IPAddress.Parse(ip);
			
			_rgbw = new RGBWLights(this);
			_white = new WhiteLights(this);
			_rgb = new RGBLights(this);
		}

		#endregion

			  
		#region Static Methods

		// HACK: I only have one controller, this needs to be tested with multiple controllers.

		/// <summary>
		/// Broadcasts an UDP message to discover MiLight WiFi controller(s) on the
		/// local network. Freezes the current thread for +- 1 second.
		/// </summary>
		/// <returns>A list of Controller instances.</returns>
		public static List<Controller> Discover()
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

			// Create Controller instances and return them
			var controllers = new List<Controller>();
			var ips = ep.ToString().Split(':');
			for (int i = 0; i < ips.Length; i+=2) {
				controllers.Add(new Controller(ips[i]));
			}

			return controllers;
		}

		#endregion
		
		
		#region Public Methods
		
		/// <summary>
		/// All light types as <see cref="Lights"/> base types.
		/// </summary>
		public Lights[] All()
		{
			return new Lights[] {RGBW, White, RGB};
		}
		
		/// <summary>
		/// Switches 'on' all lights, <see cref="RGBWLights"/>, 
		/// <see cref="WhiteLights"/> and <see cref="RGBLights"/>.
		/// </summary>
		public void AllOn()
		{
			foreach(var lights in All()) {
				lights.SwitchOn();
			}
		}
		
		/// <summary>
		/// Switches 'off' all lights, <see cref="RGBWLights"/>, 
		/// <see cref="WhiteLights"/> and <see cref="RGBLights"/>.
		/// </summary>		
		public void AllOff()
		{
			foreach(var lights in All()) {
				lights.SwitchOff();
			}
		}
		
		/// <summary>
		/// Sets the <see cref="RGBWLights"/> and 
		/// <see cref="RGBLights"/> to white and
		/// switches 'on' all <see cref="WhiteLights"/>.
		/// </summary>
		public void AllWhite()
		{
			RGBW.SwitchWhite(0);
			White.SwitchOn();
			RGB.SetColor(Color.White);
		}
		
		/// <summary>
		/// Sets the <see cref="RGBWLights"/> and 
		/// <see cref="RGBLights"/> to a given 'System.Drawing.Color'
		/// </summary>
		/// <param name="color">To color to set the lights.</param>
		public void AllColor(Color color)
		{
			RGBW.SetColor(0, color);
			RGB.SetColor(color);
		}
		
		/// <summary>
		/// Sets the <see cref="RGBWLights"/> and 
		/// <see cref="RGBLights"/> to a given hue.
		/// </summary>
		/// <param name="hue">To hue (0.0 - 360.0) degrees.</param>
		public void AllHue(float hue)
		{
			RGBW.SetHue(0, hue);
			RGB.SetHue(hue);
		}
		
		#endregion
		

		#region Private Methods
		
		/// <summary>
		/// Sends a command to the MiLight WiFi controller. If 'AutoDelay' is true, will
		/// suspend the thread for 50ms.
		/// </summary>
		/// <param name="command">A 3 byte long array with the command codes to send.</param>
		internal void SendCommand(byte[] command)
		{
			UDPController.Send(command, 3);
			if(_autodelay) {
				Thread.Sleep(_delay); // Sleep 50ms to prevent command dropping
			}
		}
		
		#endregion
	}
}
