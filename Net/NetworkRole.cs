using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class NetworkRole
{
    protected readonly object locker = new object();
    protected Packet packet = null;
    protected Packet.PacketInterpreter onConnect = null;
    protected Packet.PacketInterpreter onDisconnect = null;
    protected Packet.PacketInterpreter onReceive = null;

    abstract public void Disconnect();
    abstract public void SendUdp(Packet _packet);
    abstract public void SendTcp(Packet _packet);
    abstract protected void ReceiveTcpData(IAsyncResult _result);
    abstract protected void ReceiveUdpData(IAsyncResult _result);
    public void SetOnConnect(Packet.PacketInterpreter _interpreter)
    {
        lock(locker)
        {
            onConnect = _interpreter;
        }
    }
    public void SetOnReceive(Packet.PacketInterpreter _interpreter)
    {
        lock(locker)
        {
            onReceive = _interpreter;
        }
    }
    public void SetOnDisconnect(Packet.PacketInterpreter _interpreter)
    {
        lock(locker)
        {
            onDisconnect = _interpreter;
        }
    }
}
