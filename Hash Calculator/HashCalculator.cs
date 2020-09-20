using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hash_Calculator
{
	public class HashCalculator
	{
		public static string Calculate_Hash_In_House(string file_path, string an_algorithm)
		{
			using var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(an_algorithm);
			if (algorithm != null)
			{
				using var stream = new BufferedStream(File.OpenRead(file_path), 1048576);
				var hash = algorithm.ComputeHash(stream);
				var formatted = new StringBuilder(2 * hash.Length);

				foreach (var b in hash)
				{
					formatted.AppendFormat("{0:x2}", b);
				}

				return formatted.ToString();
			}
			else
				throw new ArgumentNullException("Hash algorithm not found", an_algorithm);
		}

		public static string Calculate_Hash_Using_CertUtil(string file_path, string an_algorithm)
		{
			var certutil_process = Process.Start(new ProcessStartInfo(@"certutil", $"-hashfile \"{file_path}\" \"{an_algorithm}\"")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false
			});

			certutil_process.WaitForExit();

			StringBuilder formatted = new StringBuilder();
			formatted.Append(certutil_process.StandardOutput.ReadToEnd());
			string[] lines = formatted.ToString().Split('\n');

			return lines[1];
		}
	}
}
