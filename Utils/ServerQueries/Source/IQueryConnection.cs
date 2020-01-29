using System;
using System.Net.Sockets;
using ServerQueries.Responses;

namespace ServerQueries.Source
{
    public interface IQueryConnection
    {
        string Host { get; set; }

        /// <summary>
        /// Valid Port (between 0x0000 and 0xFFFF)
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Connects to the given Address with given Host and Port
        /// </summary>
        /// <param name="timeoutMiliSec">Timeout in miliseconds if no response is received</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        void Connect(int timeoutMiliSec);

        /// <summary>
        /// Connects to the given Address with Host and Port
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        void Connect();

        /// <summary>
        /// Disconnects
        /// </summary>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        void Disconnect();

        InfoResponse GetInfo();
    }
}