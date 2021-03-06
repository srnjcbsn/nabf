﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmasEngineController;

namespace NabfProject.AI
{
	public class ServerApplication : XmasController
	{
		private ServerCommunication communication;

		public ServerApplication(ServerCommunication communication)
		{
			this.communication = communication;
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Start()
		{
			base.Start();
		}
	}
}
