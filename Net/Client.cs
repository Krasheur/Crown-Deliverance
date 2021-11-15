using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

public class Client : NetworkRole
{
    // this object is only used to lock the data, avoiding reading and writing values by several thread at the same time
    UdpClient udpSocket = null;
    TcpClient tcpSocket = null;
    // store the host address and a port to use to send data
    IPEndPoint hostEndPoint = null;
    public delegate void function();
    function onConnectionFailed = null;

    public void SetOnConnectionFailed(function _interpreter)
    {
        lock(locker)
        {
            onConnectionFailed = _interpreter;
        }
    }

    // tells when to stop receiving data
    bool stopTask = false;

    public Client(string host, Packet _packet, Packet.PacketInterpreter _onConnect, function _onConnectionFailed, Packet.PacketInterpreter _onDisconnect, Packet.PacketInterpreter _packetInterpreter)
    {
        // convert the string into host address
        hostEndPoint = new IPEndPoint(IPAddress.Parse(host), 5000);
        // creates the sockets which will be used
        tcpSocket = new TcpClient();
        onConnect = _onConnect;
        onConnectionFailed = _onConnectionFailed;
        onDisconnect = _onDisconnect;
        onReceive = _packetInterpreter;
        packet = _packet;

        /*tcpSocket.Connect(hostEndPoint);

        if (tcpSocket.Connected)
        {
            tcpSocket.GetStream().BeginRead(packet.Bytes, 0, 250, ReceiveTcpData, null);
            //tcpSocket.Client.
            // launchs a new thread to listen incomming data
            udpSocket = new UdpClient(((IPEndPoint)tcpSocket.Client.LocalEndPoint).Port);
            //udpSocket.Connect(new IPEndPoint(IPAddress.Any, ((IPEndPoint)tcpSocket.Client.LocalEndPoint).Port));
            udpSocket.BeginReceive(ReceiveUdpData, null);

            onConnect(packet, (IPEndPoint)tcpSocket.Client.RemoteEndPoint, this);
        }
        else
        {
            onConnectionFailed();
        }*/

        tcpSocket.BeginConnect(hostEndPoint.Address, 5000, ConnectTCP, null);
    }

    public void ConnectTCP(IAsyncResult _result)
    {
        if (tcpSocket.Connected)
        {
            tcpSocket.EndConnect(_result);
            tcpSocket.GetStream().BeginRead(packet.Bytes, 0, 250, ReceiveTcpData, null);
            //tcpSocket.Client.
            // launchs a new thread to listen incomming data
            udpSocket = new UdpClient(((IPEndPoint)tcpSocket.Client.LocalEndPoint).Port);
            //udpSocket.Connect(new IPEndPoint(IPAddress.Any, ((IPEndPoint)tcpSocket.Client.LocalEndPoint).Port));
            udpSocket.BeginReceive(ReceiveUdpData, null);

            onConnect(packet, (IPEndPoint)tcpSocket.Client.RemoteEndPoint, this);
        }
        else
        {
            onConnectionFailed();
        }
    }

    override public void Disconnect()
    {
        lock (locker)
        {
            stopTask = true;
            udpSocket.Close();
            tcpSocket.GetStream().Close();
            tcpSocket.Close();
        }
    }

    override public void SendUdp(Packet _packet)
    {
        lock (locker)
        {
            udpSocket.Send(_packet.Bytes, _packet.Length, hostEndPoint);
        }
    }

    override public void SendTcp(Packet _packet)
    {
        lock (locker)
        {
            tcpSocket.GetStream().Write(_packet.Bytes, 0, _packet.Length);
        }
    }

    override protected void ReceiveTcpData(IAsyncResult _result)
    {

        // protect data
        // they now can only be used on this thread
        lock (locker)
        {
            try
            {
                packet.Clear();
                packet.Length = tcpSocket.GetStream().EndRead(_result);
                onReceive(packet, (IPEndPoint)tcpSocket.Client.RemoteEndPoint, this);

                // start listening for new data again
                if (!stopTask)
                {
                    tcpSocket.GetStream().BeginRead(packet.Bytes, 0, packet.MaxSize, ReceiveTcpData, null);
                }
            }
            catch (System.IO.IOException)
            {
                onDisconnect(packet, (IPEndPoint)tcpSocket.Client.RemoteEndPoint, this);
            }
        }
    }

    override protected void ReceiveUdpData(IAsyncResult _result)
    {
        // protect data
        // they now can only be used on this thread
        lock (locker)
        {
            // creates an IPEndPoint to store the sender's data (IP and port used)
            IPEndPoint senderData = new IPEndPoint(IPAddress.None, 0);
            packet.SetByte(udpSocket.EndReceive(_result, ref senderData));
            onReceive(packet, senderData, this);

            // start listening for new data again
            if (!stopTask)
            {
                udpSocket.BeginReceive(ReceiveUdpData, null);
            }
        }
    }
}
