using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToodedAB
{
    public class Sound
    {
        public WaveOutEvent waveOut { get; private set; }
        int i = 1 ;
        public async void Music()
        {
            while (true)
            {
                foreach (var item in new List<byte[]> { Properties.Resources.music1, Properties.Resources.music2, Properties.Resources.music3, Properties.Resources.music4, Properties.Resources.music5, Properties.Resources.music6 })
                {
                    if (i == 0)
                        break;
                    using (MemoryStream stream = new MemoryStream(item))
                    {
                        using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(stream)))
                        {
                            using (waveOut = new WaveOutEvent())
                            {
                                waveOut.Init(waveStream);
                                waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.MusicValue)/100f;
                                waveOut.Play();

                                while (waveOut.PlaybackState == PlaybackState.Playing)
                                {
                                    await Task.Delay(1000);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void StopMusic(WaveOutEvent waveOut)
        {
            if (waveOut != null)
                waveOut.Stop();
            i = 0;
        }

    }
}
