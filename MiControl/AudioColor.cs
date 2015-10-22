using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSCore.SoundIn;
using CSCore.DSP;
using CSCore.Streams;

namespace MiControl
{
    /// <summary>
    /// Class for capturing an audio stream and generating colors.
    /// </summary>
    public class AudioColor
    {
    	#region Private Variables
    	
    	WasapiLoopbackCapture capture;
    	
    	#endregion
    	
    	public AudioColor()
    	{
    		capture = new WasapiLoopbackCapture();
    		capture.Initialize();
    	}
    }
}
