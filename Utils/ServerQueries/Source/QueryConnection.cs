using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ServerQueries.Common;
using ServerQueries.Common.ByteHelper;
using ServerQueries.Responses;

namespace ServerQueries.Source
{
    public class QueryConnection : IQueryConnection
    {
        private UdpClient m_udpClient;
        private IPEndPoint m_endPoint;

        /// <inheritdoc />
        public string Host
        {
            get;
            set;
        } = "127.0.0.1";

        /// <inheritdoc />
        public int Port
        {
            get;
            set;
        } = 27015;

        public QueryConnection()
        {

        }

        /// <inheritdoc />
        public void Connect(int timeoutMiliSec)
        {
            if (string.IsNullOrEmpty(Host))
                throw new ArgumentNullException(nameof(Host), "A Host must be specified");
            if (Port < 0x0 || Port > 0xFFFF)
                throw new ArgumentOutOfRangeException(nameof(Port), "A Valid Port has to be specified (between 0x0000 and 0xFFFF)");

            if (m_udpClient != null || (m_udpClient != null && m_udpClient.Client.Connected))
                return;

            m_udpClient = new UdpClient();
            m_endPoint = new IPEndPoint(IPAddress.Parse(Host), Port);
            m_udpClient.Connect(m_endPoint);
            m_udpClient.Client.SendTimeout = timeoutMiliSec;
            m_udpClient.Client.ReceiveTimeout = timeoutMiliSec;
        }

        /// <inheritdoc />
        public void Connect()
        {
            Connect(5000);
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            if (m_udpClient != null)
            {
                if (m_udpClient.Client != null && m_udpClient.Client.Connected)
                {
                    m_udpClient.Client.Disconnect(false);
                }
                m_udpClient.Dispose();
            }
        }

        private void request(byte[] requestMessage)
        {
            m_udpClient.Send(requestMessage, requestMessage.Length);
        }

        private byte[] fetchResponse()
        {
            byte[] response = m_udpClient.Receive(ref m_endPoint);
            IByteReader byteReader = Common.Helper.GetByteReader(response);
            if (byteReader.GetLong().Equals(Common.Constants.SimpleResponseHeader))
            {
                return byteReader.GetRemaining();
            }
            else
            {
                throw new NotImplementedException("Mulitpacket Responses are not yet supported.");
            }
        }

        /// <summary>
        /// Gets the servers general informations
        /// </summary>
        /// <returns>InfoResponse containing all Infos</returns>
        /// <exception cref="SourceQueryException"></exception>
        public InfoResponse GetInfo()
        {
            try
            {
                request(Common.Constants.A2S_INFO_REQUEST);
                var response = fetchResponse();

                var byteReader = Common.Helper.GetByteReader(response);
                byte header = byteReader.GetByte();
                if (header != 0x49)
                    throw new ArgumentException("The fetched Response is no A2S_INFO Response.");

                InfoResponse res = new InfoResponse();

                res.Header = header;
                res.Protocol = byteReader.GetByte();
                res.Name = byteReader.GetString();
                res.Map = byteReader.GetString();
                res.Folder = byteReader.GetString();
                res.Game = byteReader.GetString();
                res.ID = byteReader.GetShort();
                res.Players = byteReader.GetByte();
                res.MaxPlayers = byteReader.GetByte();
                res.Bots = byteReader.GetByte();
                res.ServerType = Common.Helper.ToServerType(byteReader.GetByte());
                res.Environment = Common.Helper.ToEnvironment(byteReader.GetByte());
                res.Visibility = Common.Helper.ToVisibility(byteReader.GetByte());
                res.VAC = byteReader.GetByte() == 0x01;
                //Check for TheShip
                if (res.ID == 2400)
                {
                    res.Mode = Common.Helper.ToTheShipMode(byteReader.GetByte());
                    res.Witnesses = byteReader.GetByte();
                    res.Duration = TimeSpan.FromSeconds(byteReader.GetByte());
                }
                res.Version = byteReader.GetString();

                //IF Has EDF Flag 
                if (byteReader.Remaining > 0)
                {
                    res.EDF = byteReader.GetByte();

                    if ((res.EDF & 0x80) == 1)
                    {
                        res.Port = byteReader.GetShort();
                    }
                    if ((res.EDF & 0x10) == 1)
                    {
                        res.SteamID = byteReader.GetLong();
                    }
                    if ((res.EDF & 0x40) == 1)
                    {
                        res.SourceTvPort = byteReader.GetShort();
                        res.SourceTvName = byteReader.GetString();
                    }
                    if ((res.EDF & 0x20) == 1)
                    {
                        res.KeyWords = byteReader.GetString();
                    }
                    if ((res.EDF & 0x01) == 1)
                    {
                        res.GameID = byteReader.GetLong();
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                throw new SourceQueryException("Could not gather Info", ex);
            }
        }
    }
}