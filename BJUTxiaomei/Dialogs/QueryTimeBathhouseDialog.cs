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
    public class QueryTimeBathhouseDialog : IDialog<List<Attachment>>
    {
        private static string buildingName;
        private static string buildingOpenHours;

        public QueryTimeBathhouseDialog(string inbuildingName, string inbuildingOpenHours)
        {
            buildingName = inbuildingName;
            buildingOpenHours = inbuildingOpenHours;
        }

        public Task StartAsync(IDialogContext context)
        {
            var NorthBathhouseName = "北区浴室";
            var SouthBathhouseName = "南区浴室";
            string NorthBathhouseOpenHours = "";
            string SouthBathHouseOpenHours = "";

            for( int i=0; i<buildingOpenHours.Length;i++)
            {               
                 if( i<buildingOpenHours.Length/2 ) NorthBathhouseOpenHours += buildingOpenHours[i];
                 if( i>buildingOpenHours.Length/2 ) SouthBathHouseOpenHours += buildingOpenHours[i];
            }

            HeroCard heroCard = GetHeroCard(NorthBathhouseName, NorthBathhouseOpenHours);

            AdaptiveCard adCard = GetAdaptiveCard(SouthBathhouseName, SouthBathHouseOpenHours);

            context.Done(new List<Attachment>() { heroCard.ToAttachment(),
                    new Attachment()
                    {
                         Content = adCard,
                         ContentType = AdaptiveCard.ContentType
                    }
            });

            return Task.CompletedTask;
        }

        private static HeroCard GetHeroCard(string NorthBathhouseName, string NorthBathhouseOpenHours)
        {
            return new HeroCard()
            {
                Title = NorthBathhouseName,
                Text = "开放时间：" + NorthBathhouseOpenHours,
                //Images = new List<CardImage>()
                //        {
                //               new CardImage(buildingAddress3D)
                //        }
                //Buttons = new List<CardAction>()
                //        {
                //            new CardAction("openUrl", "官網", value: "http://www.google.com")
                //        }
            };
        }

        private AdaptiveCard GetAdaptiveCard(string SouthBathhouseName,string SouthBathHouseOpenHours)
        {
            var card = new AdaptiveCard();
            var columSet = new ColumnSet();
            columSet.Columns.Add(new Column()
            {
                Size = "1",
                Items = new List<CardElement>()
                         {
                             new TextBlock()
                             {
                                  Text = SouthBathhouseName,
                                  Weight = TextWeight.Bolder,
                                  Size = TextSize.ExtraLarge
                             },
                             new TextBlock()
                             {
                                 Text = "开放时间：" + SouthBathHouseOpenHours,
                                 IsSubtle = true,
                                   Wrap = false
                             }
                             // new TextBlock()
                             //{
                             //    Text = buildingPhone,
                             //    IsSubtle = true,
                             //      Wrap = false
                             //}
                         }
            });

            //columSet.Columns.Add(new Column()
            //{
            //    Size = "1",
            //    Items = new List<CardElement>()
            //            {
            //                new Image()
            //                {
            //                    Url = buildingAddressImage,
            //                    Size = ImageSize.Auto
            //                }
            //            }
            //});

            card.Body.Add(columSet);

            //card.Actions.Add(new TextBlock()
            //{
            //     = "电话：" + buildingPhone,
            //    //Url = "http://wwww.google.com"
            //});

            //columSet.Columns.Add(new Column()
            //{
            //    new TextBlock()
            //                 {
            //                      Text = buildingName,
            //                      Weight = TextWeight.Bolder,
            //                      Size = TextSize.ExtraLarge
            //                 }
            //});

            return card;
        }
    }
}