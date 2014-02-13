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
                var targets = reader.read();
                TargetManager.AddTargets(targets);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("File was not found.");
                return;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File was not found.");
                return;
            }
            catch (FormatException)
            {
                // Couldn't convert either coords, points, or flashrate
                Console.WriteLine("Target file contains invalid integer or float.");
                return;
            }
            catch (INITargetFileReader.InvalidName)
            {
                // Name contained a space
                Console.WriteLine("A target name contains a space.");
                return;
            }
            catch (INITargetFileReader.InvalidTarget)
            {
                // Missing an entry in a target
                Console.WriteLine("Incomplete target.");
                return;
            }
            catch (FileHandlers.FileReader.InvalidHeader)
            {
                // Broken header tag
                Console.WriteLine("Target contains invalid header line.");
                Console.WriteLine("Valid header: [Target]");
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
        static void PrintFriends()
        {
            var targets = TargetManager.Instance.Friends;
            foreach (var target in targets)
            {
                Console.WriteLine("Name: " + target.Name);
                Console.WriteLine("Position: X: {0}, Y: {1}, Z: {2}", target.X, target.Y, target.Z);
                Console.WriteLine("Friend: " + target.Friend);
                Console.WriteLine("Points: " + target.Points);
                Console.WriteLine("Flash Rate: " + target.FlashRate);
            }
        }
        static void PrintEnemies()
        {
            var targets = TargetManager.Instance.Enemies;
            foreach (var target in targets)
            {
                Console.WriteLine("Name: " + target.Name);
                Console.WriteLine("Position: X: {0}, Y: {1}, Z: {2}", target.X, target.Y, target.Z);
                Console.WriteLine("Friend: " + target.Friend);
                Console.WriteLine("Points: " + target.Points);
                Console.WriteLine("Flash Rate: " + target.FlashRate);
            }
        }
        private static void Kill(IMissileLauncher launcher, string targetName)
        {
            Target target = null;
            try
            {
                target = TargetManager.Instance.GetTarget(targetName);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("That target does not exist.");
                return;
            }
            if (!target.Friend)
            {
                var phi = Conversions.calcPhi(target.X, target.Y, target.Z);
                var theta = Conversions.calcTheta(target.X, target.Y);
                launcher.moveTo(phi, theta);
                try
                {
                    launcher.fire();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("We can't do that Captain! We don't have the powa!");
                }
            }
            else
            {
                Console.WriteLine("Can't shoot that target, Captain. It is friendly!");
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
                    case "kill":
                        Kill(launcher, argument1);
                        break;
                    case "load":
                        AddTargets(argument1);
                        break;
                    case "reload":
                        launcher.reload();
                        break;
                    case "status":
                        Console.WriteLine(launcher.status());
                        break;
                    case "fire":
                        try
                        {
                            launcher.fire();
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("We can't do that Captain! We don't have the powa!");
                        }
                        break;
                    case "isfriend":
                        IsFriend(argument1);
                        break;
                    case "move":
                        try
                        {
                            launcher.moveTo(Convert.ToDouble(argument1), Convert.ToDouble(argument2));
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid coordinates.");
                        }
                        break;
                    case "moveby":
                        try
                        {
                            launcher.moveBy(Convert.ToDouble(argument1), Convert.ToDouble(argument2));
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid coordinates.");
                        }
                        break;
                    case "friends":
                        PrintFriends();
                        break;
                    case "scallywags":
                        PrintEnemies();
                        break;
                    default:
                        if (input != "exit")
                            Console.WriteLine("Invalid Command.");
                        break;
                }
            } while (input != "exit");
        }
    }
}
