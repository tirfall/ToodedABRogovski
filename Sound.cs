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

        //для проигрывания музыки
        public async void Music()
        {
            /*Асинхроный метод для проигрывания музыки. 
            * Есть цикл с 6 песнями, которые включаются поочередно и если наступил конец списка, то 
            * мелодии пойдут новым кругом.
            * Мелодия не играет только если i == 0 - костыль для остановки музыки
            * Переменная изменяется в Stop()
            */ 
            while (i!=0)
            {
                foreach (var item in new List<byte[]> { Properties.Resources.music1, Properties.Resources.music2, Properties.Resources.music3, Properties.Resources.music4, Properties.Resources.music5, Properties.Resources.music6 })
                {
                    if (i == 0)
                        break;
                    //await Task.Run(async () =>
                    //{
                        using (MemoryStream stream = new MemoryStream(item))
                        {
                            using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(stream)))
                            {
                                try
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

                                } catch (Exception) { return; }
                            }
                        }
                    //});
                }
            }

        }
        

        //для проигрывания звуковых эффектов
        public async void Effect(byte[] s)
        {
            /*Асинхроный метод который похож на метод музыки, но тут уже есть параметр 's' он отвечает как раз за звуковой эффект
             * тип переменной byte[] потому что в свойтсвах/ресурсах все звуковые файла сохраняются именно с таким
             * типом данных
             */
            //await Task.Run(async () =>
            //{
            using (MemoryStream stream = new MemoryStream(s))
            {
                using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(stream)))
                {
                    try
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
                    catch (Exception) { return; }
                }
            }
            //});
        }
        

        //для остановки музыки/эфектов
        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
            i = 0;
        }

        //для изменения громкости звука
        public void VolumeMusicChange()
        {
            if (waveOut != null)
                waveOut.Volume = (float)Convert.ToDouble(Properties.Settings.Default.SoundValue) / 100f;
            //беру переменную которая сохранена в ресурсах и изменяется при работы в программе в настройках звука.
            //Делю на сто т.к. .Volume берет значени от 0.0 до 1.0, то есть 
            //100% = 1.0 
            //0% = 0
        }
    }
}
