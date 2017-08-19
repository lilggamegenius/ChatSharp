using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatSharp.Handlers
{
    internal static class CapabilityHandlers
    {
        public static void HandleCapability(IrcClient client, IrcMessage message)
        {
            var serverCaps = new List<string>();
            var supportedCaps = client.Capabilities.ToArray();
            var requestedCaps = new List<string>();

            switch (message.Parameters[1])
            {
                case "LS":
                    // Parse server capabilities
                    var serverCapsString = (message.Parameters[2] == "*" ? message.Parameters[3] : message.Parameters[2]);
                    serverCaps.AddRange(serverCapsString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                    // CAP 3.2 multiline support. Send CAP requests on the last CAP LS line.
                    // The last CAP LS line doesn't have * set as Parameters[2]
                    if (message.Parameters[2] != "*")
                    {
                        // Check which capabilities we support that the server supports
                        requestedCaps.AddRange(supportedCaps.Select(cap => cap.Name).Intersect(serverCaps));

                        // Check if we have to request any capability to be enabled.
                        // If not, end the capability negotiation.
                        if (requestedCaps.Count > 0)
                            client.SendRawMessage("CAP REQ :{0}", string.Join(" ", requestedCaps));
                        else
                            client.SendRawMessage("CAP END");
                    }
                    break;
                case "ACK":
                    // Get the accepted capabilities
                    var acceptedCaps = message.Parameters[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string acceptedCap in acceptedCaps)
                        client.Capabilities.Enable(acceptedCap);

                    // Check if the enabled capabilities count is the same as the ones
                    // acknowledged by the server.
                    if (client.Capabilities.Enabled.Count() == acceptedCaps.Count())
                        client.SendRawMessage("CAP END");

                    break;
                case "NAK":
                    // Get the rejected capabilities
                    var rejectedCaps = message.Parameters[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string acceptedCap in rejectedCaps)
                        client.Capabilities.Disable(acceptedCap);

                    // Check if the disabled capabilities count is the same as the ones
                    // rejected by the server.
                    if (client.Capabilities.Disabled.Count() == rejectedCaps.Count())
                        client.SendRawMessage("CAP END");

                    break;
                case "LIST":
                    // Not implemented yet
                    break;
                case "NEW":
                    // Not implemented yet
                    break;
                case "DEL":
                    // Not implemented yet
                    break;
                default:
                    break;
            }
        }
    }
}
