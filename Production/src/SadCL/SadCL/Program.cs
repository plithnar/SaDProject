using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissileLauncher;
using Targets;
using FileHandlers;

namespace SadCL
{
    class Program
    {
        static void AddTargets(string TargetFile)
        {
            ITargetFileReader reader = null;
            try
            {
                reader = TargetFileReaderFactory.Create(TargetFile);
                TargetManager.AddTargets(reader.read());
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File was not found.");
                Console.Read();
                return;
            }
            catch (FormatException)
            {
                // Couldn't convert either coords, points, or flashrate
                Console.WriteLine("Target file contains invalid integer or float.");
                Console.Read();
                return;
            }
            catch (INITargetFileReader.InvalidName)
            {
                // Name contained a space
                Console.WriteLine("A target name contains a space.");
                Console.Read();
                return;
            }
            catch (INITargetFileReader.InvalidTarget)
            {
                // Missing an entry in a target
                Console.WriteLine("Incomplete target.");
                Console.Read();
                return;
            }
            catch (FileHandlers.FileReader.InvalidHeader)
            {
                // Broken header tag
                Console.WriteLine("Target contains invalid header line.");
                Console.WriteLine("Valid header: [Target]");
                Console.Read();
                return;
            }
        }
        // Print target given the name of the target
        static void PrintTarget(string targetName)
        {
            try
            {
                var target = TargetManager.Instance.GetTarget(targetName);
                Console.WriteLine("Name: " + target.Name);
                Console.WriteLine("X: " + target.X);
                Console.WriteLine("Y: " + target.Y);
                Console.WriteLine("Z: " + target.Z);
                Console.WriteLine("Friend: " + target.Friend);
                Console.WriteLine("Points: " + target.Points);
                Console.WriteLine("Flash Rate: " + target.FlashRate);
            }
            catch (InvalidOperationException)
            {
                // Target does not exist
                Console.WriteLine("That target does not exist.");
            }
        }
        // Print all target names
        static void PrintCommand()
        {
            var targetNames = TargetManager.Instance.GetNames();
            foreach (var name in targetNames)
            {
                Console.WriteLine(name);
            }
        }
        // Print Friendliness of a particular target
        static void IsFriend(string targetName)
        {
            try
            {
                var target = TargetManager.Instance.GetTarget(targetName);
                if (target.Friend)
                    Console.WriteLine("Aye Captain!");
                else
                    Console.WriteLine("Nay, Scallywag!");
            }
            catch (InvalidOperationException)
            {
                // Target does not exist
                Console.WriteLine("That target does not exist.");
            }
        }
        static void Main(string[] args)
        {
            var launcher = MissileLauncherFactory.Create(MissileLauncherTypes.Mock);

            Console.WriteLine("mcommandlinePrompt> SadCL");
            Console.WriteLine("System Loaded.");
            Console.WriteLine("Ready to Fire Ze Missiles.");

            string input;

            do
            {
                Console.Write(">");
                input = Console.ReadLine();
                input = input.ToLower();
                var segments = input.Split(' ');
                string command="", argument1="", argument2="";
                if (segments.Length >= 1) command = segments[0];
                if (segments.Length >= 2) argument1 = segments[1];
                if (segments.Length >= 3) argument2 = segments[2];
                switch (command)
                {
                    case "print":
                        if (argument1 != "")
                            PrintTarget(argument1);
                        else
                            PrintCommand();
                        break;
                    case "load":
                        AddTargets(argument1);
                        break;
                    case "reload":
                        launcher.reload();
                        break;
                    case "status":
                        var mock = (Mock)launcher;
                        mock.printStatus();
                        break;
                    case "fire":
                        launcher.fire();
                        break;
                    case "isfriend":
                        IsFriend(argument1);
                        break;
                    case "move":
                        launcher.moveTo(Convert.ToDouble(argument1), Convert.ToDouble(argument2));
                        break;
                    case "moveby":
                        launcher.moveBy(Convert.ToDouble(argument1), Convert.ToDouble(argument2));
                        break;
                    default:
                        Console.WriteLine("Invalid Command.");
                        break;
                }
            } while (input != "exit");
        }
    }
}
