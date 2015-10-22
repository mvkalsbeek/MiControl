using System;
using System.Linq;
using System.Text;
using CSCore.SoundIn;
using CSCore.Streams;

namespace MiControl
{
    /// <summary>
    /// Class for capturing an audio stream and generating colors.
    /// </summary>
    public class AudioColor
    {
    	WasapiLoopbackCapture capture;
    	
    	public AudioColor()
    	{
    		capture.Initialize();
    	}
    }
}
