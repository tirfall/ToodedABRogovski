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
            while (i!=0)
            {
                foreach (var item in new List<byte[]> { Properties.Resources.music1, Properties.Resources.music2, Properties.Resources.music3, Properties.Resources.music4, Properties.Resources.music5, Properties.Resources.music6 })
                {
                    if (i == 0)
                        break;
                    await Task.Run(async () =>
                    {
                        using (MemoryStream stream = new MemoryStream(item))
                        {
                            using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(stream)))
                            {
                                using (waveOut = new WaveOutEvent())
                                {
                                    waveOut.Init(waveStream);
                                    waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
                                    waveOut.Play();

                                    while (waveOut.PlaybackState == PlaybackState.Playing)
                                    {
                                        waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
                                        await Task.Delay(100);
                                        if (i==0)
                                            break;
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }

        public async void Effect(byte[] s)
        {
            await Task.Run(async () =>
            {
                using (MemoryStream stream = new MemoryStream(s))
                {
                    using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(stream)))
                    {
                        using (waveOut = new WaveOutEvent())
                        {
                            waveOut.Init(waveStream);
                            waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
                            waveOut.Play();

                            while (waveOut.PlaybackState == PlaybackState.Playing)
                            {
                                waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
                                await Task.Delay(100);
                            }
                        }
                    }
                }
            });
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
            i = 0;
        }

        public void VolumeMusicChange()
        {
            if (waveOut != null)
                waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
        }
    }
}
