using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Resource;
using Microsoft.Bot.Connector;

namespace BJUTxiaomei
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
            if (activity.GetActivityType() == ActivityTypes.Message)
            {             
                await Conversation.SendAsync(activity, () => new Dialogs.Xiaomei());    //调用XiaoMei这个方法
            }
            else
            {
                await this.HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            string messageType = message.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                var reply = message.CreateReply("Hello,我是小美\n" +
                    "现在，我可以完成以下任务.\n" +
                    "问路\n" +
                    "问时间\n" +
                    "选课介绍\n" +
                    "新生须知\n" +
                    "攻略系统\n" +
                    "一键式访问教务管理系统\n" +
                    "如果问题解决不了，再去问辅导员\n" +
                    "爱护导员，从我做起");
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                await connector.Conversations.ReplyToActivityAsync(reply);

            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (messageType == ActivityTypes.Ping)
            {
            }

            return null;
        }        
    }
}