using System;
using System.Diagnostics;
using System.IO;

namespace Hash_Calculator
{
	public class FileInformation
	{
		public static bool Does_File_Exist(string file_path)
		{
			return File.Exists(file_path);
		}

		public static string Get_File_Name(string file_path)
		{
			return Path.GetFileName(file_path);
		}

		public static DateTime Get_File_Creation_Time(string file_path)
		{
			return File.GetCreationTime(file_path);
		}

		public static DateTime Get_File_Modified_Time(string file_path)
		{
			return File.GetLastWriteTime(file_path);
		}

		public static DateTime Get_File_Last_Accessed_Time(string file_path)
		{
			return File.GetLastAccessTime(file_path);
		}

		public static long Get_File_Size(string file_path)
		{
			return new FileInfo(file_path).Length;
		}

		public static string Get_File_Version(string file_path)
		{
			var versionInfo = FileVersionInfo.GetVersionInfo(file_path);
			return versionInfo.FileVersion;
		}

		public static string Get_Product_Version(string file_path)
		{
			var versionInfo = FileVersionInfo.GetVersionInfo(file_path);
			return versionInfo.ProductVersion;
		}

		public static string Get_Extension(string file_path)
		{
			return Path.GetExtension(file_path).Substring(1);
		}

		public static string Get_File_Attributes(string file_path)
		{
			return File.GetAttributes(file_path).ToString();
		}
	}
}
