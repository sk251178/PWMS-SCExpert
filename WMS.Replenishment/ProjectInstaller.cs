using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using Made4Net.Shared;

namespace WMS.Replenishment
{
	/// <summary>
	/// Summary description for ProjectInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ProjectInstaller : System.Configuration.Install.Installer
	{
		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProjectInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public override void Install (System.Collections.IDictionary stateSaver)
		{
			try 
			{
				this.Uninstall(null);
				System.Threading.Thread.Sleep(3000);
			} 
			catch{}
			base.Install(stateSaver);
			if (this.Context.Parameters["SQLSERVER"]!=null) 
			{
				updateConfigFile();
			}
		}
		private void updateConfigFile() 
		{
			string server= this.Context.Parameters["SQLSERVER"];
			string user  = this.Context.Parameters["SQLUSER"];
			string pwd  = this.Context.Parameters["SQLPWD"];
			string sysdb  = this.Context.Parameters["SQLSYSDB"];
			string datadb  = this.Context.Parameters["SQLDATADB"];


			System.Reflection.Assembly Asm  = System.Reflection.Assembly.GetExecutingAssembly();
			string strConfigLoc ;
			strConfigLoc = Asm.Location;

			string strTemp ;
			string strName ;
			try
			{
        
				strTemp = strConfigLoc;
				strName = strTemp.Substring(strTemp.LastIndexOf("\\") + 1);
				strTemp = strTemp.Remove(strTemp.LastIndexOf("\\"), strTemp.Length - strTemp.LastIndexOf("\\"));
				System.IO.FileInfo FileInfo  = new System.IO.FileInfo(strTemp + "\\" + strName + ".config");
				if (!FileInfo.Exists) {
					throw new Exception(strName + " : Missing config file");
					 }

				System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
				XmlDocument.Load(FileInfo.FullName);

        // Finds the right node and change it to the new value.
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_DSN']").Attributes.GetNamedItem("value").InnerText = sysdb;
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_MappedName']").Attributes.GetNamedItem("value").InnerText = user;
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_MappedKey']").Attributes.GetNamedItem("value").InnerText = pwd;
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Made4NetSchema_DBType']").Attributes.GetNamedItem("value").InnerText = "SQL";
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_DSN']").Attributes.GetNamedItem("value").InnerText = datadb;
				XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_MappedName']").Attributes.GetNamedItem("value").InnerText = user;
		        XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_MappedKey']").Attributes.GetNamedItem("value").InnerText = pwd;
			    XmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Default_DBType']").Attributes.GetNamedItem("value").InnerText = "SQL";

				XmlDocument.Save(FileInfo.FullName);
				}
        catch (Exception ex) {
			throw new Exception(strConfigLoc + " Install Error : " + ex.Message);
					  }
		}
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstaller1
			// 
			this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstaller1.Password = null;
			this.serviceProcessInstaller1.Username = null;
			// 
			// serviceInstaller1
			// 
            //this.serviceInstaller1.DisplayName = "Expert Replenishment Service";
            //this.serviceInstaller1.ServiceName = "Expert Replenishment Service";
            this.serviceInstaller1.DisplayName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.ReplanishmnetServiceSection);
            this.serviceInstaller1.ServiceName = Util.GetServiceName(ConfigurationSettingsConsts.ServiceInstancename, ConfigurationSettingsConsts.ReplanishmnetServiceSection);
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.DelayedAutoStart = true;
			this.serviceInstaller1.ServicesDependedOn = new string[] {"MSMQ"};
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
																					  this.serviceProcessInstaller1,
																					  this.serviceInstaller1});

		}
		#endregion
	}
	
}
