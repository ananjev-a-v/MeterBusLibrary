using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Bindings.Udp
{
    public sealed class UdpEndpointBinding : EndpointBinding, IEndPointBinding, IDisposable
    {
        private const int MAX_BUFFER_SIZE = 2048;
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private readonly EndPoint _endpoint;

        /// <summary>
        /// Constructor for UdpEndpointBinding.
        /// </summary>
        /// <param name="endpoint">
        /// Client: new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1690)
        /// Server: new IPEndPoint(IPAddress.Any, 1690)</param>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
        public UdpEndpointBinding(EndPoint endpoint, IPacketSerializer serializer, ITelemetryLogger logger) : base(serializer, logger)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        public bool Send(INetworkPacket package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            if (_socket == null)
                throw new NullReferenceException(nameof(_socket));

            if (_serializer == null)
                throw new NullReferenceException(nameof(_serializer));

            var data = _serializer.Serialize(package, out var length);

            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = _endpoint;
            args.SetBuffer(data, 0, length);
            args.AcceptSocket = _socket;
            args.UserToken = package;
            args.Completed += OnClientOperation;

            return _socket.SendToAsync(args);
        }

        public bool StartReceiving()
        {
            var receiveBuffer = new byte[MAX_BUFFER_SIZE];

            var args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = _endpoint;
            args.AcceptSocket = _socket;
            args.SetBuffer(new byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);
            args.Completed += OnServerOperation;

            _socket.Bind(_endpoint);

            return _socket.ReceiveFromAsync(args);
        }

        private void OnServerOperation(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                _logger.Info($"Operation: '{e.LastOperation}' Status: '{e.SocketError}'", null);

                switch (e.SocketError)
                {
                    case SocketError.Success:
                        {
                            switch (e.LastOperation)
                            {
                                case SocketAsyncOperation.SendTo:
                                    {

                                    }
                                    break;
                                case SocketAsyncOperation.ReceiveFrom:
                                    {
                                        _packageStream.OnNext(new PacketStreamEvent(_serializer.Deserialize(e.Buffer), new UdpEndpointBinding(e.RemoteEndPoint, _serializer, _logger)));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _packageStream.OnError(ex);
            }
            finally
            {
                var receiveBuffer = new byte[MAX_BUFFER_SIZE];
                e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                var result = e.AcceptSocket.ReceiveFromAsync(e);
            }
        }

        private void OnClientOperation(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                _logger.Info($"Operation: '{e.LastOperation}' Status: '{e.SocketError}'", null);

                switch (e.SocketError)
                {
                    case SocketError.Success:
                        {
                            switch (e.LastOperation)
                            {
                                case SocketAsyncOperation.SendTo:
                                    {

                                    }
                                    break;
                                case SocketAsyncOperation.Receive:
                                    {
                                        _packageStream.OnNext(new PacketStreamEvent(_serializer.Deserialize(e.Buffer), new UdpEndpointBinding(e.RemoteEndPoint, _serializer, _logger)));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _packageStream.OnError(ex);
            }
            finally
            {
                var receiveBuffer = new byte[MAX_BUFFER_SIZE];
                e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                var result = e.AcceptSocket.ReceiveAsync(e);
            }
        }      

        public void Dispose()
        {
            _socket?.Dispose();
        }

        public override string ToString()
        {
            switch (_endpoint)
            {
                case IPEndPoint ipEndpoint: return $"Udp {ipEndpoint.Address.ToString()}:{ipEndpoint.Port}";
                default: return base.ToString();
            }
        }
    }
}
