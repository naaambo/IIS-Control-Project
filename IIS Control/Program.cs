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
			ServerManager serverManager = new ServerManager();
			

			//Settings the Name, Protocol, Bindings (in this case, Port80 and a HostName, does not include Binding Information) and Default Path
			Site site = serverManager.Sites.Add("TestSite", "http", "*:80:www.onboardtest.local", "D:\\TestSite");
			site.Name = "OnBoardTest";

			//Setting ApplicationPoolName
			site.ApplicationDefaults.ApplicationPoolName = "OnBoardDev";

			//Starting Server and applying bindings.
			site.ServerAutoStart = true;
			serverManager.CommitChanges();

			Console.ReadLine();
			site.Stop();
			Console.ReadLine();

		}
	}
}
