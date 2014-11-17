using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace IIS_Control
{
	class Program
	{
		static void Main(string[] args)
		{
			//Need to split everything up into Chunks.
			CreateSite();
		}

		public static void CreateSite()
		{
			Console.WriteLine("Enter a host name");
			string HostName = String.Format("www.{0}.local",Console.ReadLine());
			string formattedName = String.Format("*:80:{0}", HostName);


			//Define Version/Application Source/Settings Folder
			//Automatically add the settings file.

			ServerManager serverManager = new ServerManager();
			Application connect;
			Application boardpadweb;

			//Settings the Name, Protocol, Bindings (in this case, Port80 and a HostName, does not include Binding Information) and Default Path
			Site site = serverManager.Sites.Add(HostName, "http", formattedName, @"D:\Default");
			site.Name = HostName;

			//Setting ApplicationPoolName
			site.ApplicationDefaults.ApplicationPoolName = "OnBoardDev";

			//Creating the Connect application, and adding a virtual directory for the settings file
			connect = site.Applications.Add("/connect", @"D:\Source\Connect\v1.9\OnBoardNextGeneration\OnBoard");
			connect.VirtualDirectories.Add("/settings", @"D:\Default\1.9a\connect");

			//Creating the boardpadweb application, and adding a virtual directory for the settings file
			boardpadweb = site.Applications.Add("/boardpadweb", @"D:\Source\Connect\v1.9\OnBoardNextGeneration\OnBoard");
			boardpadweb.VirtualDirectories.Add("/settings", @"D:\Default\1.9a\boardpadweb");

			ModifyHosts(HostName);

			//Starting Server then applying bindings.
			site.ServerAutoStart = true;
			serverManager.CommitChanges();

			Console.WriteLine("\nPress Enter To Exit");
			Console.ReadLine();
		}

		static async void ModifyHosts(string hostName)
		{
			string host = hostName;

			FileIOPermission f = new FileIOPermission(PermissionState.None);
			f.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, @"C:\Windows\System32\drivers\etc\hosts");
			try
			{
				f.Demand();
				Console.WriteLine("Permission Successful");
			}
			catch (SecurityException s)
			{
				Console.WriteLine(s.Message);
			}


			using (StreamWriter writer = new StreamWriter(@"C:\Windows\System32\drivers\etc\hosts", true))
			{
				await writer.WriteLineAsync(String.Format("127.0.0.1 {0}", host));
				writer.Dispose();
				Console.WriteLine("Hosts File Updated");
			}

			
		}
	}
}
