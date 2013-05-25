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
			catch (ThreadStateException)
			{
				Console.Write("Something went wrong while connecting");
				return false;
			}
			Console.Write("Connection success\n");

			String text = "circle butt radius=1000 b1=0.1 b2=0.01 g1=0.1 g2=0.01 cx=500";
			byte[] encoded = Encoding.ASCII.GetBytes(text);
			sock.Send(encoded);

			return true;
		}
	}
}
