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
    public class AskWayDialog :/* IDialog<Attachment>*/ IDialog<List<Attachment>>
    {
        private static string buildingName;
        private static string buildingAddressDescription;
        //private static string buildingAddressImage;
        //private static string buildingAddress3D;
        private static string buildingPhone;

        public AskWayDialog(string inbuildingName, string inbuildingAddressDescription  /*string inbuildingAddressImage,string inbuildingAddress3D*/,string inbuildingPhone)
        {
            buildingName = inbuildingName;
            buildingAddressDescription = inbuildingAddressDescription;
            //buildingAddressImage = inbuildingAddressImage;
            //buildingAddress3D = inbuildingAddress3D;
            buildingPhone = inbuildingPhone;
        }

        //public Task StartAsync(IDialogContext context)
        //{
        //    HeroCard heroCard = GetHeroCard();

        //    context.Done(heroCard.ToAttachment());

        //    return Task.CompletedTask;
        //}

        ////private static HeroCard GetHeroCard()
        ////{
        ////    return new HeroCard()
        ////    {
        ////        Title = "一教",
        ////        Text = "西门右手边50米",
        ////        Images = new List<CardImage>()
        ////                {
        ////                       //new CardImage("https://cdn.pixabay.com/photo/2016/02/10/13/32/hotel-1191709_1280.jpg")
        ////                       new CardImage("C:\\Project\\Bot\\BJUTxiaomei\\BJUTxiaomei\\Images\\14号宿舍楼.webp")
        ////                },
        ////        Buttons = new List<CardAction>()
        ////                {
        ////                    new CardAction("openUrl", "按钮", value: "http://www.google.com")
        ////                }
        ////    };
        ////}

        //private static HeroCard GetHeroCard()
        //{
        //    return new HeroCard()
        //    {
        //        Title = buildingName,
        //        Text = buildingAddressDescription,
        //        Images = new List<CardImage>()
        //                {
        //                       //new CardImage("https://cdn.pixabay.com/photo/2016/02/10/13/32/hotel-1191709_1280.jpg")
        //                       new CardImage(buildingAddressImage)
        //                },
        //        Buttons = new List<CardAction>()
        //                {
        //                    new CardAction("openUrl", "按钮", value: "http://www.google.com")
        //                }


        //    };
        //}


        public Task StartAsync(IDialogContext context)
        {
            HeroCard heroCard = GetHeroCard();

            AdaptiveCard adCard = GetAdaptiveCard();

            context.Done(new List<Attachment>() { heroCard.ToAttachment(),
                    new Attachment()
                    {
                         Content = adCard,
                         ContentType = AdaptiveCard.ContentType
                    }
            });

            return Task.CompletedTask;
        }
         
        private static HeroCard GetHeroCard()
        {
            return new HeroCard()
            {
                Title = buildingName/* + "三位立体图"*/,
                Text = "西门刚进来的左手边"
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

        private AdaptiveCard GetAdaptiveCard()
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
                                  Text = buildingName,
                                  Weight = TextWeight.Bolder,
                                  Size = TextSize.ExtraLarge
                             },
                             new TextBlock()
                             {
                                 Text = buildingAddressDescription,
                                 IsSubtle = true,
                                   Wrap = false
                             },
                              new TextBlock()
                             {
                                 Text = buildingPhone,
                                 IsSubtle = true,
                                   Wrap = false
                             }
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