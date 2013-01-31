﻿namespace Barista.WebSocket.Protocol
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Text;
  using Barista.SocketBase.Protocol;

  /// <summary>
  /// http://tools.ietf.org/html/rfc6455#section-4.4
  /// </summary>
  class MultipleProtocolSwitchProcessor : IProtocolProcessor
  {
    private readonly byte[] m_switchResponse;

    public MultipleProtocolSwitchProcessor(IEnumerable<int> availableVersions)
    {
      var responseBuilder = new StringBuilder();

      responseBuilder.AppendWithCrCf("HTTP/1.1 400 Bad Request");
      responseBuilder.AppendWithCrCf("Upgrade: WebSocket");
      responseBuilder.AppendWithCrCf("Connection: Upgrade");
      responseBuilder.AppendWithCrCf("Sec-WebSocket-Version: " + string.Join(", ", availableVersions.Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray()));
      responseBuilder.AppendWithCrCf();

      m_switchResponse = Encoding.UTF8.GetBytes(responseBuilder.ToString());
    }

    public bool CanSendBinaryData { get { return false; } }

    public ICloseStatusCode CloseStatusClode { get; set; }

    public IProtocolProcessor NextProcessor { get; set; }

    public bool Handshake(IWebSocketSession session, WebSocketReceiveFilterBase previousReader, out IReceiveFilter<IWebSocketFragment> dataFrameReader)
    {
      dataFrameReader = null;
      session.SendRawData(m_switchResponse, 0, m_switchResponse.Length);
      return true;
    }

    public void SendMessage(IWebSocketSession session, string message)
    {
      throw new NotImplementedException();
    }

    public void SendData(IWebSocketSession session, byte[] data, int offset, int length)
    {
      throw new NotImplementedException();
    }

    public void SendCloseHandshake(IWebSocketSession session, int statusCode, string closeReason)
    {
      throw new NotImplementedException();
    }

    public void SendPong(IWebSocketSession session, byte[] pong)
    {
      throw new NotImplementedException();
    }

    public void SendPing(IWebSocketSession session, byte[] ping)
    {
      throw new NotImplementedException();
    }

    public int Version
    {
      get { return 0; }
    }

    public bool IsValidCloseCode(int code)
    {
      throw new NotImplementedException();
    }
  }
}