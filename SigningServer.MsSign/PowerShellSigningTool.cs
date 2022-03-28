using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SigningServer.MsSign
{
    public class PowerShellSigningTool : PortableExecutableSigningTool
    {
        private static readonly HashSet<string> PowerShellSupportedExtension =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                ".ps1",
                ".psm1"
            };

        private static readonly string[] PowerShellSupportedHashAlgorithms = { "SHA256" };

        public override string[] SupportedFileExtensions => PowerShellSupportedExtension.ToArray();
        public override string[] SupportedHashAlgorithms => PowerShellSupportedHashAlgorithms;

        public override bool IsFileSupported(string fileName)
        {
            return PowerShellSupportedExtension.Contains(Path.GetExtension(fileName));
        }

        public override void UnsignFile(string inputFileName)
        {
            var script = File.ReadAllLines(inputFileName);
            using (var writer = new StreamWriter(new FileStream(inputFileName, FileMode.Create)))
            {
                var isSignatureBlock = false;
                foreach (var line in script)
                {
                    if (line.StartsWith("# SIG # Begin"))
                    {
                        isSignatureBlock = true;
                    }
                    else if (line.StartsWith("# SIG # End"))
                    {
                        isSignatureBlock = false;
                    }
                    else if (!isSignatureBlock)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}