﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using JSLibrary.IiLang;
using JSLibrary.IiLang.DataContainers;
using JSLibrary.IiLang.Parameters;
using NabfAgentLogic.AgentInterfaces;
using System.Collections.Concurrent;
using Microsoft.FSharp.Collections;
using NabfAgentLogic;
using JSLibrary.Data.GenericEvents;

namespace NabfProject.AI.Client
{
	public class ClientApplication
	{
		private IAgentLogic logic;
		private XmlPacketTransmitter<IilPerceptCollection,IilAction> transmitter;
		private HashSet<Thread> activeThreads = new HashSet<Thread>();
		private ConcurrentQueue<IilAction> packets = new ConcurrentQueue<IilAction>();
		private AutoResetEvent packetadded = new AutoResetEvent(false);
		private ServerCommunication servCom;

		public ClientApplication(XmlPacketTransmitter<IilPerceptCollection, IilAction> transmitter, ServerCommunication servCom, IAgentLogic logic)
		{
			this.servCom = servCom;
			this.logic = logic;
			this.transmitter = transmitter;

            logic.EvaluationCompleted += logic_needMessageSent;
            //logic.SendAgentServerAction += logic_SendAgentServerAction;
            logic.SendMarsServerAction += logic_SendMarsServerAction;
            logic.EvaluationStarted += logic_needMessageSent;
			//logic.PerceptsLoaded += logic_PerceptsLoaded;
			//logic.JobLoaded += logic_JobLoaded;
			
		}

        void logic_SendMarsServerAction(object sender, UnaryValueEvent<IilAction> evt)
        {
            
        }

        void logic_SendAgentServerAction(object sender, UnaryValueEvent<IilAction> evt)
        {
            throw new NotImplementedException();
        }

       
		void logic_needMessageSent(object sender, JSLibrary.Data.GenericEvents.UnaryValueEvent<IilAction> evt)
		{
			this.AddPacket(evt.Value);
		}

		public void UpdateSender()
		{
			this.packetadded.WaitOne();
			bool hasPacket = false;
			do
			{
				IilAction packet;
				hasPacket = this.packets.TryDequeue(out packet);
				if (hasPacket)
				{
					this.transmitter.SeralizePacket(packet);
				}
			} while (hasPacket);

		}

		public void UpdateReceiver()
		{
            var data = transmitter.DeserializeMessage();
			if(data.Percepts.Count != 0)
			{
				logic.HandlePercepts(data);
			}
		}

		public void StartThread(Action action)
		{
			var thread = new Thread(new ThreadStart(action));
			lock (activeThreads)
			{
				this.activeThreads.Add(thread);
			}
			thread.Start();
		}


		private void AddPacket(IilAction packet)
		{
			this.packets.Enqueue(packet);
			this.packetadded.Set();
		}
		
	}
}
