using System;
using System.Net;
using System.Net.Sockets;

public class NTPClient
{
    private const string NTPServer = "192.168.0.97"; // Cambia esto a la dirección de tu servidor NTP
    private const int NTPPort = 123;

    public long GetNetworkTime()
    {
        const byte ntpDataLength = 48;
        var ntpData = new byte[ntpDataLength];
        ntpData[0] = 0x1B; // NTP version 3, client request

        var addresses = Dns.GetHostEntry(NTPServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], NTPPort);
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
        }

        // Convertir la respuesta a tiempo NTP
        ulong intPart = BitConverter.ToUInt32(ntpData, 40);
        ulong fractPart = BitConverter.ToUInt32(ntpData, 44);
        ulong milliseconds = (intPart * 1000 + (fractPart * 1000) / 0x100000000UL);
        return (long)milliseconds - 2208988800000; // Ajustar a Unix time
    }
}
