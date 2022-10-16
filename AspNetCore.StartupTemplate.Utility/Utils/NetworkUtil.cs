using System.Net;
using System.Net.Sockets;

namespace AspNetCore.StartUpTemplate.Utility.Utils;

public static class NetworkUtil
{
    public static string GetAddressIpByDns()
    {
        string? localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint?.Address.ToString();
        }
        return localIP==null?"":localIP;
    }
}
