using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

    public class Host : NetworkRole
    {
        // this object is only used to lock the data, avoiding reading and writing values by several thread at the same time
        UdpClient udpSocket = new UdpClient(5000);
        // list of all udpclients : IP and port used
        List<IPEndPoint> udpClients = new List<IPEndPoint>();
        // list of tcpClients
        List<TcpClient> tcpSocket = new List<TcpClient>();
        // The listener listen for new incoming connections
        TcpListener tcpListener = null;

        // tells when to stop receiving data
        bool stopTask = false;

        public Host(Packet _packet, Packet.PacketInterpreter _onConnect, Packet.PacketInterpreter _onDisconnect, Packet.PacketInterpreter _onReceive)
        {
            packet = _packet;
            onConnect = _onConnect;
            onDisconnect = _onDisconnect;
            onReceive = _onReceive;
            // on connecte le TCP listener au port 5000 (classique) de l'ip local
            tcpListener = new TcpListener(IPAddress.Any, 5000);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(AcceptNewClient, null);
            udpSocket.BeginReceive(ReceiveUdpData, null);
        }

        override public void Disconnect()
        {
            lock (locker)
            {
                stopTask = true;
                tcpListener.Stop();
                udpSocket.Close();
                for (int x = 0; x < tcpSocket.Count; ++x)
                {
                    tcpSocket[x].GetStream().Close();
                    tcpSocket[x].Close();
                }
            }
        }

        override public void SendUdp(Packet _packet)
        {
            lock (locker)
            {
                for (int x = 0; x < udpClients.Count; ++x)
                {
                    udpSocket.Send(_packet.Bytes, _packet.Length, udpClients[x]);
                }
            }
        }

        override public void SendTcp(Packet _packet)
        {
            lock (locker)
            {
                for (int x = 0; x < tcpSocket.Count; ++x)
                {
                    tcpSocket[x].GetStream().Write(_packet.Bytes, 0, _packet.Length);
                }
            }
        }

        public void SendTcpTo(Packet _packet, IPEndPoint _endPoint)
        {
            lock (locker)
            {
                for (int x = 0; x < tcpSocket.Count; ++x)
                {
                    if (tcpSocket[x].Client.RemoteEndPoint.Equals(_endPoint))
                    {
                        tcpSocket[x].GetStream().Write(_packet.Bytes, 0, _packet.Length);
                    }
                }
            }
        }

        public void SendTcpExcept(Packet _packet, IPEndPoint _endPoint)
        {
            lock (locker)
            {
                for (int x = 0; x < tcpSocket.Count; ++x)
                {
                    if (!tcpSocket[x].Client.RemoteEndPoint.Equals(_endPoint))
                    {
                        tcpSocket[x].GetStream().Write(_packet.Bytes, 0, _packet.Length);
                    }
                }
            }
        }

        public void AcceptNewClient(IAsyncResult _result)
        {
            // protect data
            lock (locker)
            {
                // if yes, we register a new tcp client 
                tcpSocket.Add(tcpListener.EndAcceptTcpClient(_result));
                udpClients.Add((IPEndPoint)tcpSocket[tcpSocket.Count - 1].Client.RemoteEndPoint);

                onConnect(packet, (IPEndPoint)tcpSocket[tcpSocket.Count - 1].Client.RemoteEndPoint, this);

                // start listening incoming data from the new client
                tcpSocket[tcpSocket.Count - 1].GetStream().BeginRead(packet.Bytes, 0, packet.Length, ReceiveTcpData, tcpSocket.Count - 1);

                // start listening for new incoming connections
                if (!stopTask)
                {
                    tcpListener.BeginAcceptTcpClient(AcceptNewClient, null);
                }
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
                    packet.Length = tcpSocket[(int)_result.AsyncState].GetStream().EndRead(_result);

                    // quand une information est reçut elle est envoyée à tous les autres clients
                    for (int x = 0; x < tcpSocket.Count; ++x)
                    {
                        if (x != (int)_result.AsyncState)
                        {
                            tcpSocket[x].GetStream().Write(packet.Bytes, 0, packet.Length);
                        }
                    }

                    onReceive(packet, (IPEndPoint)tcpSocket[(int)_result.AsyncState].Client.RemoteEndPoint, this);
                  
                    // start listening for new data again
                    if (!stopTask)
                    {
                        tcpSocket[(int)_result.AsyncState].GetStream().BeginRead(packet.Bytes, 0, 250, ReceiveTcpData, _result.AsyncState);
                    }
                }
                catch(System.IO.IOException)
                {
                    onDisconnect(packet, (IPEndPoint)tcpSocket[(int)_result.AsyncState].Client.RemoteEndPoint, this);
                    tcpSocket.RemoveAt((int)_result.AsyncState);
                    udpClients.RemoveAt((int)_result.AsyncState);
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

                for (int x = 0; x < udpClients.Count; ++x)
                {
                    if (!udpClients[x].Equals(senderData))
                    {
                        udpSocket.Send(packet.Bytes, packet.Length, udpClients[x]);
                    }
                }

                onReceive(packet, senderData, this);

                // start listening for new data again
                if (!stopTask)
                {
                    udpSocket.BeginReceive(ReceiveUdpData, null);
                }
            }  
        }
    }