using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using System;
using System.Threading.Tasks;

namespace Iris.Rms.Voice
{
    public class VoiceRms : IVoiceRms
    {
        CommandEvaluator _commandEvaluator;// = new CommandEvaluator(_rmsService, _context);

        public VoiceRms(CommandEvaluator commandEvaluator)
        {
            _commandEvaluator = commandEvaluator ?? throw new ArgumentNullException(nameof(commandEvaluator));
        }
        public async Task Listen()
        {
            try
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\voicerms.json");
                var client = new GoogleCloudApiClient();
                var textResponse = string.Empty;
                do
                {
                    var task = await client.StreamingMicRecognizeAsync(10, _commandEvaluator.ChooseAndCallCommand);
                    //Task.WaitAll(task);
                } while (true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
