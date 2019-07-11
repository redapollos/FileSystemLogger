using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace FileSystemLogger
{
    class Program
    {
        private static IConfiguration config;

        static void Main(string[] args)
        {
            var mask = "";

            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            if (string.IsNullOrEmpty(config["baseDirectory"]))
            {
                Console.WriteLine("Please specify an base directory.");
                return;
            }

            if (string.IsNullOrEmpty(config["template"]))
            {
                Console.WriteLine("Please specify a template.");
                return;
            }

            if (!string.IsNullOrEmpty(config["mask"]))
            {
                mask = config["mask"];
            }

            // get all the files
            var files = Directory.EnumerateFiles(config["baseDirectory"], "", SearchOption.AllDirectories);

            // use the template and create a big text block
            var sb = new StringBuilder();

            foreach(var f in files)
            {
                var fi = new FileInfo(f);
                var filepath = fi.FullName.Replace(mask, "").Replace("\\", "/");

                /*
                Console.WriteLine(fi.Name);
                Console.WriteLine(fi.FullName);
                Console.WriteLine(fi.FullName.Replace(mask, ""));
                Console.WriteLine(fi.FullName.Replace(mask, "").Replace("\\", "/"));
                */

                //var file
                sb.AppendFormat(config["template"], fi.Name, filepath);                
            }

            // write out the file
            File.WriteAllText("output.txt", sb.ToString());


            Console.WriteLine("Done!");
        }
    }
}
