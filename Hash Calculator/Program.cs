using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hash_Calculator
{
	class Program
	{
		private static readonly string app_name = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
		private static bool use_built_in_methods = false;
		private static bool get_crc32_hash = false;
		private static bool get_md5_hash = false;
		private static bool get_sha1_hash = false;
		private static bool get_sha256_hash = false;
		private static bool get_sha384_hash = false;
		private static bool get_sha512_hash = false;

		static void Main(string[] args)
		{
			if (args.Count() > 0)
				ParseSuppliedArguments(args);
			else
			{
				Console.WriteLine("Info: You must pass at least one file path to this program in order to calculate its hash values.");
				Console.WriteLine("\nFor example:");
				Console.WriteLine("\"" + app_name + "\" \"C:\\Folder Path That Contains Spaces\\a file.exe\"");
				Console.WriteLine("\"" + app_name + "\" -crc32 -md5 \"C:\\a file.doc\"");
				Console.WriteLine("\"" + app_name + "\" -sha256 \"C:\\text file.txt\" C:\\music_file.mp3");
				Console.WriteLine("\nFor a list of switch parameters that can be used, please type \"" + app_name + "\" -h");
			}
		}

		private static void ParseSuppliedArguments(string[] args)
		{
			var paths = new List<string>();

			foreach (var arg in args)
			{
				if (arg.ToLower().Equals("-h") || arg.ToLower().Equals("--help"))
				{
					DisplayHelpMessage();
					return;
				}
				else
				{
					if (arg.ToLower().Equals("-use-built-in"))
						use_built_in_methods = true;
					else
						if (arg.ToLower().Equals("-crc32"))
							get_crc32_hash = true;
					else
						if (arg.ToLower().Equals("-md5"))
							get_md5_hash = true;
					else
						if (arg.ToLower().Equals("-sha1"))
							get_sha1_hash = true;
					else
						if (arg.ToLower().Equals("-sha256"))
							get_sha256_hash = true;
					else
						if (arg.ToLower().Equals("-sha384"))
							get_sha384_hash = true;
					else
						if (arg.ToLower().Equals("-sha512"))
							get_sha512_hash = true;
					else
						if (FileInformation.Does_File_Exist(arg))
							paths.Add(arg);
						else
						{
							Console.Error.WriteLine("Error: argument or path {0} is incorrect, please check and try again.", arg);
							Console.WriteLine("\nFor a list of switch parameters that can be used, please type \"" + app_name + "\" -h");
							return;
						}
				}
			}

			if (!get_crc32_hash && !get_md5_hash && !get_sha1_hash && !get_sha256_hash && !get_sha384_hash && !get_sha512_hash)
			{
				// No specific algorithm was selected via arguments, enable them all
				get_crc32_hash = true;
				get_md5_hash = true;
				get_sha1_hash = true;
				get_sha256_hash = true;
				get_sha384_hash = true;
				get_sha512_hash = true;
			}

			ProcessQuery(paths);
		}

		private static void ProcessQuery(List<string> paths)
		{
			var total_elapsed_time = TimeSpan.Zero;

			foreach (var path in paths)
			{
				Console.WriteLine();

				var time_keeper = new TimeKeeper();
				var elapsed_time = time_keeper.Measure(() =>
				{
					Task<string> crc32_task = null;
					Task<string> md5_task = null;
					Task<string> sha1_task = null;
					Task<string> sha256_task = null;
					Task<string> sha384_task = null;
					Task<string> sha512_task = null;

					PrintFileDetails(path);

					if (get_crc32_hash)
					{
						crc32_task = Task.Run(() =>
						{
							Crc32 crc32 = new Crc32();
							return "CRC32: " + crc32.Calculate_Hash_In_House(path);
						});
					}

					try
					{
						if (get_md5_hash)
						{
							md5_task = !use_built_in_methods ?
							Task.Run(() => { return "MD5: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "MD5"); })
							:
							Task.Run(() => { return "MD5: " + HashCalculator.Calculate_Hash_In_House(path, "MD5"); });
						}

						if (get_sha1_hash)
						{
							sha1_task = !use_built_in_methods ?
							Task.Run(() => { return "SHA-1: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA1"); })
							:
							Task.Run(() => { return "SHA-1: " + HashCalculator.Calculate_Hash_In_House(path, "SHA1"); });
						}

						if (get_sha256_hash)
						{
							sha256_task = !use_built_in_methods ?
							Task.Run(() => { return "SHA-256: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA256"); })
							:
							Task.Run(() => { return "SHA-256: " + HashCalculator.Calculate_Hash_In_House(path, "SHA256"); });
						}

						if (get_sha384_hash)
						{
							sha384_task = !use_built_in_methods ?
							Task.Run(() => { return "SHA-384: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA384"); })
							:
							Task.Run(() => { return "SHA-384: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA384"); });
						}

						if (get_sha512_hash)
						{
							sha512_task = !use_built_in_methods ?
							Task.Run(() => { return "SHA-512: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA512"); })
							:
							Task.Run(() => { return "SHA-512: " + HashCalculator.Calculate_Hash_Using_CertUtil(path, "SHA512"); });
						}
					}
					catch (ArgumentNullException ex)
					{
						Console.Error.WriteLine(ex.Message);
					}

					var list_of_tasks = new List<Task<string>> { crc32_task, md5_task, sha1_task, sha256_task, sha384_task, sha512_task };
					foreach (var task in list_of_tasks)
					{
						if (task != null)
						{
							task.Wait();
							Console.WriteLine(task.Result);
						}
					}
				});

				Console.WriteLine("Calculated in: {0} seconds", elapsed_time.TotalSeconds);
				total_elapsed_time += elapsed_time;

				Console.WriteLine("\n----------------");
			}

			Console.WriteLine("\nTotal elapsed time (in seconds): " + total_elapsed_time.TotalSeconds);
		}

		private static void PrintFileDetails(string arg)
		{
			Console.WriteLine("Filename: " + FileInformation.Get_File_Name(arg));
			Console.WriteLine("Full Path: " + arg);
			Console.WriteLine("Created Time: " + FileInformation.Get_File_Creation_Time(arg));
			Console.WriteLine("Modified Time: " + FileInformation.Get_File_Modified_Time(arg));
			Console.WriteLine("Last Accessed Time: " + FileInformation.Get_File_Last_Accessed_Time(arg));
			Console.WriteLine("File Size: " + String.Format("{0:n0}", FileInformation.Get_File_Size(arg)) + " bytes");
			Console.WriteLine("File Version: " + FileInformation.Get_File_Version(arg));
			Console.WriteLine("Product Version: " + FileInformation.Get_Product_Version(arg));
			Console.WriteLine("Extension: " + FileInformation.Get_Extension(arg));
			Console.WriteLine("File Attributes: " + FileInformation.Get_File_Attributes(arg));
		}

		private static void DisplayHelpMessage()
		{
			Console.WriteLine("\"" + app_name + "\" -crc32        \"path to file\" (to return only the CRC32 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -md5          \"path to file\" (to return only the MD5 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -sha1         \"path to file\" (to return only the SHA-1 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -sha256       \"path to file\" (to return only the SHA-256 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -sha384       \"path to file\" (to return only the SHA-384 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -sha512       \"path to file\" (to return only the SHA-512 hash value of a file)");
			Console.WriteLine("\"" + app_name + "\" -use-built-in \"path to file\" (to calculate hash values using built-in algorithms instead of calling the CertUtil utility)");
			Console.WriteLine("\"" + app_name + "\" -h                           (to display this help message)");
			Console.WriteLine("\"" + app_name + "\" --help                       (to display this help message)");
			Console.WriteLine("\nAll switches can be combined together (e.g. -crc32 -md5 -use-built-in) at the exception of -h and --help");
			Console.WriteLine("\nYou can pass multiple file paths to this program by separating them with a space (e.g. \"" + app_name + "\" \"path to file 1\" \"path to file 2\")");
		}
	}
}
