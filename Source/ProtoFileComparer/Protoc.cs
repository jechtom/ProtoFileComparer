using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProtoFileComparer
{
    public class Protoc
    {
        public Protoc()
        {
            DescriptorOutFile = "descriptor.pb";
            RootDirectories = new List<string>();
            InputFiles = new List<string>();
        }

        public const string ExecutablePath = "ProtoTools\\protoc.exe";

        public IList<string> RootDirectories { get; set; }

        public IList<string> InputFiles { get; set; }

        public string DescriptorOutFile { get; set; }

        public Google.ProtocolBuffers.DescriptorProtos.FileDescriptorSet ExecuteAndReadResult()
        {
            byte[] content;
            using (var tempFile = new TempFile())
            {
                DescriptorOutFile = tempFile.Path;
                Execute();
                content = System.IO.File.ReadAllBytes(tempFile.Path);
            }
            var descriptorSet = Google.ProtocolBuffers.DescriptorProtos.FileDescriptorSet.ParseFrom(content);
            return descriptorSet;
        }

        public void Execute()
        {
            // setup parameters
            ProcessStartInfo processInfo = new ProcessStartInfo(ExecutablePath);
            processInfo.Arguments = ValidateAndBuildParameters();
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardInput = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.ErrorDialog = false;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = Environment.CurrentDirectory;

            // start
            Process process = Process.Start(processInfo);
            if (process == null)
            {
                throw new Exception();
            }

            // wait for exit and process output
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string errorOutput = process.StandardError.ReadToEnd();
            
            Debug.WriteLine(output);
            Debug.WriteLine(errorOutput);

            if(process.ExitCode != 0)
                throw new Exception(string.Format("protoc.exe returned status code: {0}\nError output:\n{1}", process.ExitCode, errorOutput));

        }

        private string ValidateAndBuildParameters()
        {
            if (string.IsNullOrWhiteSpace(DescriptorOutFile))
                throw new InvalidOperationException("DescriptorOutFile is null or white space.");

            if (InputFiles == null || InputFiles.Count == 0)
                throw new InvalidOperationException("InputFiles collection are null or empty.");

            var result = new StringBuilder();

            // process includes
            result.Append("--include_imports");

            // output file
            result.Append(" --descriptor_set_out=");
            result.Append(EncodeParameterArgument(DescriptorOutFile));

            // input directories
            foreach (var directory in RootDirectories ?? new string[0])
            {
                result.Append(" --proto_path=");
                result.Append(EncodeParameterArgument(directory));
            }

            // input files
            foreach (var file in InputFiles ?? new string[0])
            {
                result.Append(" ");
                result.Append(EncodeParameterArgument(file));
            }

            return result.ToString();
        }

        /// <summary>
        /// Encodes an argument for passing into a program
        /// From: http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp
        /// </summary>
        /// <param name="original">The value that should be received by the program</param>
        /// <returns>The value which needs to be passed to the program for the original value 
        /// to come through</returns>
        public static string EncodeParameterArgument(string original)
        {
            if (string.IsNullOrEmpty(original))
                return original;

            string value = Regex.Replace(original, @"(\\*)" + "\"", @"$1\$0");
            value = Regex.Replace(value, @"^(.*\s.*?)(\\*)$", "\"$1$2$2\"");
            return value;
        }
    }
}
