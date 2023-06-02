using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Assets.Scripts.Media.TTS;
using UnityEngine;
//using NVorbis;
//using NAudio.Wave;

public enum SynthesisLang
{
    Ru,
    En,
}

public enum SynthesisVoice
{
    Filipp,
}

public class TtsDownloader : MonoBehaviour
{
    private readonly string _yandexCloudUri = "https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize";
    private string _iamToken; 
    private string _folderId;
    
    [Space(10)] [SerializeField] public SynthesisLang SynthesisLang;
    [SerializeField] public SynthesisVoice SynthesisVoice;
    [SerializeField] public string SynthesisText;

    private void Start()
    {
        SynthesizeSpeech();
        
        _iamToken = GetCloudCredentials.GetIamToken();
        _folderId = GetCloudCredentials.GetFolderId("gumrfvr");
    }

    private string BuildCorrectStrFromEnum(SynthesisLang lang)
    {
        switch (lang)
        {
            case SynthesisLang.Ru:
                return "ru-RU";
            case SynthesisLang.En:
                return "en-US";
            default: return null;
        }
    }
    
    private async void SynthesizeSpeech()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_iamToken}");

        var parameters = new Dictionary<string, string>
        {
            {"text", SynthesisText},
            {"lang", BuildCorrectStrFromEnum(SynthesisLang)},
            {"voice", SynthesisVoice.ToString().ToLower()},
            {"folderId", _folderId},
            {"format", "lpcm"},
            {"sampleRateHertz", "48000"},
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await client.PostAsync(_yandexCloudUri, content);
        var responseBytes = await response.Content.ReadAsByteArrayAsync();
        
        // File.WriteAllBytes(Application.dataPath + "/Resources/new.raw", responseBytes);
        
        
        // // Создание Memory Stream и VorbisReader
        // var inputStream = new MemoryStream(responseBytes);
        // var reader = new VorbisReader(inputStream, false);
        //
        // // Создание WaveWriter и сохранение wav файла в папку Resources
        // var outputFile = Application.dataPath + "/Resources/new.wav";
        // var outputStream = new FileStream(outputFile, FileMode.Create);
        // var writer = new WaveWriter(outputStream);
        // var writt = new waveWr
        // // Цикл записи аудио данных в wav файл
        // var buffer = new float[reader.SampleRate * reader.Channels];
        // int samplesRead;
        // while ((samplesRead = reader.ReadSamples(buffer, 0, buffer.Length)) > 0)
        // {
        //     writer.WriteSamples(buffer, 0, samplesRead);
        // }
        //
        // // Освободить память
        // writer.Dispose();
        // inputStream.Dispose();
        // reader.Dispose();
        // outputStream.Dispose();
    }
    //
    // private void ConvertRawToWavFormat(byte[] fileStream, string saveFilePath = "Media/TTS", string saveFileName = "file.wav")
    // {
    //     using (var inputStream = new MemoryStream(fileStream))
    //     {
    //         var reader = new NVorbis.VorbisReader(inputStream, false);
    //         var outputFile = Application.dataPath + "/Resources/new.wav";
    //         var outputStream = new MemoryStream();
    //         var writer = new WaveFileWriter(outputStream);
    //         var buffer = new float[reader.SampleRate * reader.Channels];
    //         int samplesRead;
    //         while ((samplesRead = reader.ReadSamples(buffer, 0, buffer.Length)) > 0)
    //         {
    //             var sampleArray = new short[samplesRead];
    //             for (var i = 0; i < samplesRead; i++)
    //             {
    //                 var floatingPointSample = buffer[i];
    //                 if (floatingPointSample > 1f) floatingPointSample = 1f; // ограничиваем амплитуду
    //                 if (floatingPointSample < -1f) floatingPointSample = -1f;
    //                 var shortSample = (short)(floatingPointSample * short.MaxValue);
    //                 sampleArray[i] = shortSample;
    //             }
    //             writer.WriteSamples(sampleArray, 0, samplesRead);
    //         }
    //         writer.Dispose();
    //
    //         // сохраняем новый .wav файл
    //         File.WriteAllBytes(outputFile, outputStream.ToArray());
    //     }
    // }
    //
}
