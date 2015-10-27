using System;
using System.Linq;
using System.Text;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;
using CSCore.Streams.Effects;
using CSCore.DSP;

namespace MiControl
{
    /// <summary>
    /// Class for capturing an audio stream and generating colors.
    /// </summary>
    public class AudioCapture
    {
    	WasapiLoopbackCapture capture;
    	SoundInSource source;
    	ISampleSource sampleSource;
    	Equalizer eq;
    	
    	public AudioCapture()
    	{
    		capture = new WasapiLoopbackCapture();
    		capture.Initialize();
    		
    		source = new SoundInSource(capture);
    		
    		sampleSource = WaveToSampleBase.CreateConverter(source);
    		
    		eq = new Equalizer(sampleSource);
    	}
    }
}
