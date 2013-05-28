using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RampageXL.Mugic
{
	class MugicConnection
	{
		private static Socket sock;
		protected static Queue<MugicPacket> outgoing;

		public static bool Connect(String ip)
		{
			// Set up our connection data
			sock = new Socket(SocketType.Dgram, ProtocolType.Udp);
			IPAddress server_ip = IPAddress.Parse(Config.CalVRIP);
			IPEndPoint server = new IPEndPoint(server_ip, Config.CalVRPort);
			
			Console.Write("Connecting to network\n");

			try
			{
				sock.Connect(server);
			}
			catch (Exception)
			{
				Console.Write("Something went wrong while connecting");
				return false;
			}
			Console.Write("Connection success\n");

			outgoing = new Queue<MugicPacket>();

			return true;
		}

		public static void EnueuePacket(MugicPacket p)
		{
			outgoing.Enqueue(p);
		}


		public static void SendUpdate()
		{
			if (outgoing.Count > 0)
			{
				Thread uThread = new Thread(new ThreadStart(SendThreadedUpdate));
				uThread.Start();
			}
		}

		private static void SendThreadedUpdate()
		{
			List<string> packets = new List<string>();
			int size = 0;
			string currentPacket = "";
			while (outgoing.Count > 0)
			{
				MugicPacket p = outgoing.Dequeue();
				string packet = p.ToString();

				int tempSize = size;
				tempSize += packet.Length;

				if (tempSize < Config.Max_Packet_Size)
				{
					currentPacket += packet;
					size += packet.Length;
				}
				else
				{
					packets.Add(currentPacket);
					currentPacket = "";
					size = 0;
					currentPacket += packet;
				}
			}
			// Add any remainder
			packets.Add(currentPacket);

			foreach (String packet in packets)
			{
				byte[] encoded = Encoding.ASCII.GetBytes(packet);
				Console.Write(packet+"\n");
				sock.Send(encoded);
			}
		}
	}
}
