using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iris.Rms.Voice
{
    public class CommandEvaluator
    {
        private IRmsService _rmsService;
        private RmsDbContext _context;

        public CommandEvaluator(IRmsService rmsService, RmsDbContext context)
        {
            _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task ChooseAndCallCommand(string commandText)
        {
            if (commandText.Contains("lights off"))
            {
                Console.WriteLine("Turning off the lights");

                await LightsOff();
                return;
            }
            else if (commandText.Contains("lights on"))
            {
                Console.WriteLine("Turning on the lights");
                await LightsOn();
                return;
            }
            else if (commandText.Contains("set") && commandText.Contains("temperature"))
            {
                Console.WriteLine("Setting the temperature");
                await SetTemperature(commandText);
                return;
            }
            else
            {
                Console.WriteLine($"Unknown command: {commandText}");
            }
        }

        private async Task SetTemperature(string commandText)
        {
            Models.Hvac firstHvac = _context.Hvacs?.Include(x=>x.Hooks).FirstOrDefault() ;
            if (firstHvac != null)
            {
                Models.WebHook tempHook = firstHvac.Hooks?.FirstOrDefault(hook => hook.ActivationCommand == Models.Enums.RmsCommand.HvacSetTemperature);
                if (tempHook != null)
                {
                    double dt;
                    var temperature = commandText
                        .Replace("set", "")
                        .Replace("temperature", "")
                        .Replace("the", "")
                        .Replace("to", "")
                        .Replace(" ", "");
                    if (double.TryParse(temperature, out dt))
                    {
                        var restoreHookBody = tempHook.Body.ToString();
                        tempHook.Body = tempHook.Body.Replace("{setTemperature}", temperature);

                        await _rmsService.ActOnWebHook(tempHook);
                        Console.WriteLine($"-------- TEMPERATURE IS SET TO {temperature} --------- i think");
                        //temp workaround for demo - this is dbcontext scope issue
                        tempHook.Body = restoreHookBody;
                    }

                }

            }
        }

        private async Task LightsOn()
        {
            Models.Light firstLights = _context.RmsList?.Include(x => x.Lights).FirstOrDefault()?.Lights?.FirstOrDefault();//
            if (firstLights != null)
            {
                Models.WebHook onHook = firstLights.Hooks?.FirstOrDefault(hook => hook.ActivationCommand == Models.Enums.RmsCommand.LightsOn);
                if (onHook != null)
                {
                    await _rmsService.ActOnWebHook(onHook); 
                }
            }
            Console.WriteLine("-------- LIGHTS ARE ON --------- i think");
        }


        private async Task LightsOff()
        {
            Models.Light firstLights = _context.Lights?.Include(x=>x.Hooks).FirstOrDefault(dev => dev.Type == Models.Enums.DeviceType.LightOnOff);
            if (firstLights != null)
            {
                Models.WebHook ofHook = firstLights.Hooks?.FirstOrDefault(hook => hook.ActivationCommand == Models.Enums.RmsCommand.LightsOff);
                if (ofHook != null)
                {
                    await _rmsService.ActOnWebHook(ofHook); 
                }
            }
            Console.WriteLine("-------- LIGHTS ARE OFF -------- i think");
        }

    }
}
