﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LACountyLibraryBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.Invoke)
            {
                
            }
            else if (message.Type == ActivityTypes.Message)
            {
                
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                IConversationUpdateActivity conversationUpdated = message as IConversationUpdateActivity;
                if(conversationUpdated != null) 
                {
                    ConnectorClient client = new ConnectorClient(new Uri(message.ServiceUrl));

                    foreach(var member in conversationUpdated.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                    {
                        if(member.Id == conversationUpdated.Recipient.Id)
                        {
                            Activity reply = message.CreateReply($"Hi, I'm a digital librarian. How may I assist you?");
                            reply.Attachments.Add(new Attachment() { ContentUrl = "https://colalibrarybot.blob.core.windows.net/images/howdoiguyhead.png", ContentType="image/png", Name="Bot Avatar" });
                            client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}