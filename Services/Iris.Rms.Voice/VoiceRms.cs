using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using System;
using System.IO;
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
                //var pathToFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "voicerms.json") ;

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\voicerms.json");
                var client = new GoogleCloudApiClient();
                var textResponse = string.Empty;
                do
                {
                    var task = await client.StreamingMicRecognizeAsync(10, _commandEvaluator.ChooseAndCallCommand);
                    //Task.WaitAll(task);
                } while (true);
            }
            catch 
            {
                
            }
        }
    }
}
