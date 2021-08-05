﻿using Newtonsoft.Json;
using QuestConverter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace QuestConverter
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                var inputFileName = args[0];
                DefaultQuests inputJSON;

                try
                {
                    inputJSON = JsonConvert.DeserializeObject<DefaultQuests>(File.ReadAllText(inputFileName));

                    Console.WriteLine("Input file have been read successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"There was a problem while reading input file: {ex.Message}, stack trace: {ex.StackTrace}");
                    Console.WriteLine("Exiting...");
                    return -1;
                }


                Console.WriteLine("Enter output lang name(Default=en_US.lang):");
                var outputLangName = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(outputLangName))
                {
                    outputLangName = "en_US.lang";
                }

                var outputLangFileName = Path.Combine(Directory.GetCurrentDirectory(), outputLangName);

                Console.WriteLine("Enter output quest file name(Default=DefaultQuests.json):");
                var outputQuestsName = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(outputQuestsName))
                {
                    outputQuestsName = "DefaultQuests.json";
                }

                var outputQuestsFileName = Path.Combine(Directory.GetCurrentDirectory(), outputQuestsName);

                MakeLang(inputJSON, outputLangFileName);

                MakeQuestFile(inputJSON, outputQuestsFileName);

                return 0;
            }
            else
            {
                Console.WriteLine("You must specify a DefaultQuests.json to split. Exiting...");
                return 0;
            }
        }

        private static void MakeQuestFile(DefaultQuests inputJSON, string outputQuestsFileName)
        {
            foreach (var questLine in inputJSON.questLines)
            {
                questLine.Value.properties.betterQuesting.name = questLine.Value.NameKey;
                questLine.Value.properties.betterQuesting.desc = questLine.Value.DescKey;
            }

            foreach (var quest in inputJSON.questDatabase)
            {
                quest.Value.properties.betterQuesting.name = quest.Value.NameKey;
                quest.Value.properties.betterQuesting.desc = quest.Value.DescKey;
            }
            try
            {
                File.WriteAllText(outputQuestsFileName, JsonConvert.SerializeObject(inputJSON, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                Console.WriteLine($"Quest file {outputQuestsFileName} written.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. Try a correct file name next time.");
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. A Bridge Too Far.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. Something is wrong with your drive.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. Not enough rights to write the file. Try launching from non-system folder that you have write access to.");
            }
            catch (SecurityException ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. Not enough rights to write the file. Try launching from non-system folder that you have write access to.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write Quest file {outputQuestsFileName}. Error: {ex.Message}. Equip the Pendant of Courage and try again.");
            }


        }

        private static void MakeLang(DefaultQuests inputJSON, string outputLangFileName)
        {
            var outputLang = new List<string>();

            outputLang.Add("#Quest lines");
            outputLang.Add("");

            foreach (var questLine in inputJSON.questLines)
            {
                outputLang.Add(questLine.Value.NameKey + "=" + questLine.Value.properties.betterQuesting.name);
                outputLang.Add(questLine.Value.DescKey + "=" + questLine.Value.properties.betterQuesting.desc);
            }

            outputLang.Add("#Quests");
            outputLang.Add("");

            foreach (var quest in inputJSON.questDatabase)
            {
                outputLang.Add(quest.Value.NameKey + "=" + quest.Value.properties.betterQuesting.name);
                outputLang.Add(quest.Value.DescKey + "=" + quest.Value.properties.betterQuesting.desc);
            }

            try
            {
                File.WriteAllLines(outputLangFileName, outputLang);

                Console.WriteLine($"Lang file {outputLangFileName} written.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. Try a correct file name next time.");
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. A Bridge Too Far.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. Something is wrong with your drive.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. Not enough rights to write the file. Try launching from non-system folder that you have write access to.");
            }
            catch (SecurityException ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. Not enough rights to write the file. Try launching from non-system folder that you have write access to.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write Lang file {outputLangFileName}. Error: {ex.Message}. Equip the Pendant of Courage and try again.");
            }
        }

    }
}
