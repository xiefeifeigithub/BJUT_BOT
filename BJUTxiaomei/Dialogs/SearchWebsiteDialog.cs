using BJUTxiaomei.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdaptiveCards;

namespace BJUTxiaomei.Dialogs
{
    [Serializable]
    public class SearchWebsiteDialog : IDialog<Attachment>
    {
        public Task StartAsync(IDialogContext context)
        {
            var webSiteCard = new HeroCard()
            {
                Title = "北京工业大学教务管理系统网站",
                Text = "一键式外网访问",
                Images = new List<CardImage>()
                {
                    new CardImage("https://bjutxiaomei20190216052438.azurewebsites.net/Images/jw.png")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction("openUrl","点击这里",value:"http://188.131.128.233")
                }
            };

            context.Done(webSiteCard.ToAttachment());

            return Task.CompletedTask;
        }
    }
}