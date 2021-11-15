using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using UnityEngine;


public class Packet
{
    public delegate void PacketInterpreter(Packet _packet, IPEndPoint _endPoint, NetworkRole _net);
    //public delegate void PacketInterpreter(Packet _packet);

    int index = 0;
    int size = 0;
    int maxSize = 255;
    byte[] bytes = null;

    public int Length { get => size; set => size = value; }
    public byte[] Bytes { get => bytes; set => bytes = value; }
    public int MaxSize { get => maxSize; set => maxSize = value; }

    public Packet()
    {
        bytes = new byte[maxSize];
    }

    public void Clear()
    {
        index = 0;
        size = 0;
    }

    public void Append(Int32 _int)
    {
        byte[] i32Byte = BitConverter.GetBytes(_int);
        for (int x = 0; x < 4; ++x)
        {
            bytes[size++] = i32Byte[x];
        }
    }

    public void Read(ref Int32 _int)
    {
        _int = BitConverter.ToInt32(bytes, index);
        index += 4;
    }

    public void Append(Int16 _int)
    {
        byte[] bint = BitConverter.GetBytes(_int);

        for (int x = 0; x < 2; ++x)
        {
            bytes[size++] = bint[x];
        }

    }

    public void Read(ref Int16 _int)
    {
        _int = BitConverter.ToInt16(bytes, index);
        index += 2;
    }

    public void Append(float _float)
    {
        byte[] fByte = BitConverter.GetBytes(_float);

        for (int x = 0; x < 4; ++x)
        {
            bytes[size++] = fByte[x];
        }
    }

    public void Read(ref float _float)
    {
        _float = BitConverter.ToSingle(bytes, index);
        index += 4;
    }

    public void Append(double _double)
    {
        byte[] bdouble = BitConverter.GetBytes(_double);

        for (int x = 0; x < 8; ++x)
        {
            bytes[size++] = bdouble[x];
        }
    }

    public void Read(ref double _double)
    {
        _double = BitConverter.ToDouble(bytes, index);
        index += 8;
    }
    public void Append(char _char)
    {
        byte[] bchar = BitConverter.GetBytes(_char);
        bytes[size++] = bchar[0];
    }

    public void Read(ref char _char)
    {
        _char = BitConverter.ToChar(bytes, index);
        ++index;
    }

    public void Append(string _string)
    {
        byte[] sByte = Encoding.UTF8.GetBytes(_string);
        byte[] slByte = BitConverter.GetBytes(sByte.Length);

        for (int x = 0; x < 4; ++x)
        {
            bytes[size++] = slByte[x];
        }

        for (int x = 0; x < sByte.Length; ++x)
        {
            bytes[size++] = sByte[x];
        }
    }

    public void Read(ref string _string)
    {
        int size = BitConverter.ToInt32(bytes, index);
        index += 4;
        _string = Encoding.UTF8.GetString(Bytes, index, size);
        index += size;
    }

    public void Append(Vector3 _vec)
    {
        byte[] f1bytes = BitConverter.GetBytes(_vec.x);
        byte[] f2bytes = BitConverter.GetBytes(_vec.y);
        byte[] f3bytes = BitConverter.GetBytes(_vec.z);

        for (int x = 0; x < 4; ++x)
        {
            bytes[size] = f1bytes[x];
            bytes[size + 4] = f2bytes[x];
            bytes[size++ + 8] = f3bytes[x];
        }
    }

    public void Read(ref Vector3 _vec)
    {
        _vec.x = BitConverter.ToSingle(bytes, index);
        index += 4;
        _vec.y = BitConverter.ToSingle(bytes, index);
        index += 4;
        _vec.z = BitConverter.ToSingle(bytes, index);
        index += 4;
    }

    public void SetByte(byte[] _byte)
    {
        index = 0;
        size = _byte.Length;
        for (int x = 0; x < _byte.Length; ++x)
        {
            bytes[x] = _byte[x];
        }
    }
}