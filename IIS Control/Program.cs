using System;
using System.Collections.Generic;
using System.Linq;
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
			//Define Version/Application Source/Settings Folder
			//Automatically add the settings file.
			//Edit Hosts File

			ServerManager serverManager = new ServerManager();
			Application connect;
			Application boardpadweb;

			//Settings the Name, Protocol, Bindings (in this case, Port80 and a HostName, does not include Binding Information) and Default Path
			Site site = serverManager.Sites.Add("TestSite", "http", "*:80:www.onboardtest.local", @"D:\Default");
			site.Name = "OnBoardTest";

			connect = site.Applications.Add("/connect", @"D:\Source\Connect\v1.9\OnBoardNextGeneration\OnBoard");
			connect.VirtualDirectories.Add("/settings", @"D:\Default\1.9a\connect");

			boardpadweb = site.Applications.Add("/boardpadweb", @"D:\Source\Connect\v1.9\OnBoardNextGeneration\OnBoard");
			boardpadweb.VirtualDirectories.Add("/settings", @"D:\Default\1.9a\boardpadweb");


			//Setting ApplicationPoolName
			site.ApplicationDefaults.ApplicationPoolName = "OnBoardDev";
			
			//Starting Server and applying bindings.
			site.ServerAutoStart = true;

//			site.Applications.Add("/boardpadweb", @"D\Source\Connect\v1.9\OnBoardNextGeneration\OnBoard").VirtualDirectories.Add(
//																										"/settings",
//																										@"D:\Default\1.9a\boardpadweb");

			serverManager.CommitChanges();

			Console.ReadLine();

		}
	}
}
