using Iris.Rms.Interfaces;
using System;
using System.Threading.Tasks;

namespace Iris.Rms.Voice
{
    public class VoiceRms : IVoiceRms
    {
        public async Task Listen()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\voicerms.json");
            var client = new GoogleCloudApiClient();
            var commandEvaluator = new CommandEvaluator();
            var textResponse = string.Empty;
            do
            {
                var task = await client.StreamingMicRecognizeAsync(10, commandEvaluator.ChooseAndCallCommand);
                //Task.WaitAll(task);
            } while (true);
        }
    }
}
