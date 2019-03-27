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
    public class QueryTimeDialog : IDialog<Attachment>
    {
        private static string buildingName;
        private static string buildingOpenHours;

        public QueryTimeDialog(string inbuildingName, string inbuildingOpenHours)
        {
            buildingName = inbuildingName;
            buildingOpenHours = inbuildingOpenHours;
        }

        public Task StartAsync(IDialogContext context)
        {
            var webSiteCard = new HeroCard()
            {
                Title = buildingName,
                Text = buildingOpenHours,
                //Images = new List<CardImage>()
                //{
                //    new CardImage("C:\\Project\\Bot\\BJUTxiaomei\\BJUTxiaomei\\Images\\教务管理系统网站.jpg")
                //},
                //Buttons = new List<CardAction>()
                //{
                //    new CardAction("openUrl","点击这里",value:"http://188.131.128.233")
                //}
            };

            context.Done(webSiteCard.ToAttachment());

            return Task.CompletedTask;
        }
    }
}