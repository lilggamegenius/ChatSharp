using System.Linq;

namespace ChatSharp
{
    /// <summary>
    /// Represents an IRC message sent from user-to-user or user-to-channel.
    /// </summary>
    public class PrivateMessage
    {
        internal PrivateMessage(IrcClient client, IrcMessage message, ServerInfo serverInfo)
        {
            Source = message.Parameters[0];
            Message = message.Parameters[1];

            User = client.Users.GetOrAdd(message.Prefix);
			char[] channelTypes = serverInfo.ChannelTypes;
			if(channelTypes == null){
				channelTypes = new[]{'#'}; // Assume this is twitch
			}
			if(channelTypes.Any(c=>Source.StartsWith(c.ToString()))){
				IsChannelMessage = true;
				if(client.Channels.Contains(Source)){
					Channel = client.Channels[Source];
				}
			} else{
				Source = User.Nick;
			}
		}

        /// <summary>
        /// The user that sent this message.
        /// </summary>
        public IrcUser User { get; set; }
        /// <summary>
        /// The message text.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The source of the message (a nick or a channel name).
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// True if this message was posted to a channel.
        /// </summary>
        public bool IsChannelMessage { get; set; }
		/// <summary>
		/// The channel this message came from, if it is a channel message
		/// </summary>
		public IrcChannel Channel { get; set; }
    }
}
