using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MagicConstRewriter;

public class MagicConstRewriterCommand
{
        public static int Main(string[] args)
        {
                if (!ProcessArgs(args, out var inputFiles, out var outputFile)) {
                        Console.Error.WriteLine ("Usage: MagicConstRewriter INPUT [INPUTS...] -o OUTPUT");
                        return 1;
                }
                foreach (var input in inputFiles) {
                        Console.WriteLine ($"input: {input}");
                }
                Console.WriteLine ($"output: {outputFile}");
                if (inputFiles.Length > 1) {
                        Console.Error.WriteLine ("multiple inputs not supported yet; ignoring all but the first");
                        inputFiles = new string[1]{inputFiles[0]};
                }
                ScanAndRewrite(inputFiles[0], outputFile);
                return 0;
        }

        private static bool ProcessArgs(string[] args, [NotNullWhen(true)] out string[]? inputFiles, [NotNullWhen(true)] out string? outputFile)
        {
                List<string> inputs = new();
                string? output = null;
                for(int i = 0; i < args.Length; i++) {
                        if (args[i] == "-o") {
                                if (output == null && i < args.Length - 1) {
                                        output = args[++i];
                                        continue;
                                } else {
                                        Console.Error.WriteLine($"i = {i}, args.Length = {args.Length}");
                                        Console.Error.WriteLine("multiple -o or -o without a path");
                                        inputFiles = null;
                                        outputFile = null;
                                        return false;
                                }
                        }
                        inputs.Add(args[i]);
                }
                if (output == null) {
                        Console.Error.WriteLine ("no output specified");
                        inputFiles = null;
                        outputFile = null;
                        return false;
                }
                inputFiles = inputs.ToArray();
                outputFile = output;
                return true;
        }

        public static readonly ReadOnlyMemory<byte> MagicPrefix = new byte[] { 0xAA, 0xBB, 0x33, 0x55 };

        public static readonly ReadOnlyMemory<byte> Replacement = new byte[] { 0x4D, 0x6F, 0x6E, 0x6F };

        private static void ScanAndRewrite(string inputPath, string outputFile)
        {
                var bytes = File.ReadAllBytes(inputPath);
                var magicSpan = FindMagicPrefix(bytes);
                if (magicSpan.IsEmpty)
                        throw new ArgumentException ($"Could not find magic prefix in {inputPath}");
                var payloadSpan = magicSpan.Slice(MagicPrefix.Length);
                if (payloadSpan.Length < Replacement.Length)
                        throw new ArgumentOutOfRangeException ($"magic payload of length {payloadSpan.Length} cannot fit replacement of length {Replacement.Length}");
                Replacement.Span.CopyTo(payloadSpan);
                File.WriteAllBytes(outputFile, bytes);
        }

        private static Span<byte> FindMagicPrefix(Span<byte> input)
        {
                int idx = input.IndexOf(MagicPrefix.Span);
                if (idx < 0)
                        return Span<byte>.Empty;
                else
                        return input.Slice(idx); // FIXME: only slice until the 4 nul bytes
        }

}
