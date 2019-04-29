using System;
using System.Net.Sockets;

namespace Iris.Rms.Voice
{
    public class CommandEvaluator
    {

        public void ChooseAndCallCommand(string commandText)
        {
            if (commandText.Contains("lights off"))
            {
                Console.WriteLine("Turning off the lights");

                LightsOff();
                return;
            }
            else if (commandText.Contains("lights on"))
            {
                Console.WriteLine("Turning on the lights");
                LightsOn();
            }
            else if (commandText.Contains("set temperature 25"))
            {
                Console.WriteLine("Changing temperature to 25 degrees");
            }
            else if (commandText.Contains("lights on"))
            {

            }
            else if (commandText.Contains("lights on"))
            {

            }
        }

        private void LightsOn()
        {
            Console.WriteLine("-------- LIGHTS ARE ON --------- i think");
        }


        private void LightsOff()
        {

            Console.WriteLine("-------- LIGHTS ARE OFF -------- i think");
        }

    }
}
