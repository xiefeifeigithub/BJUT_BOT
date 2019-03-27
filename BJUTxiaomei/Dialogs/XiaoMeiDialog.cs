using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using System.Linq;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using BJUTxiaomei.Models;
using System.Data;
using System.Data.Entity;
using System.Text;
using CarouselCardsBot;

namespace BJUTxiaomei.Dialogs
{
    //   [LuisModel("65e92e60-f04f-4e25-a744-b67900533d4c", "a4db9d3ff6eb4b0d8153356010df8fe5")]

    //   [LuisModel("82ca2af8-eb84-4a09-8107-7718125bec21", "3871a80a5cd046a6b463d24c42b2e605")]

    [LuisModel("cc79a7cc-a669-4b7d-884b-79b4e4391077", "1d84f0f1581d49b8b8506a526ba1ddde")]
    [Serializable]
    public class Xiaomei : LuisDialog<object>
    {       
        //private BJUT_test001Entities db = new BJUT_test001Entities();       
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"我是小美，是智能机器人，你可以问我有关工大的所有问题";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        //[LuisIntent("QueryTime")]
        //public Task QueryTime(IDialogContext context, LuisResult result)
        //{
        //    var building = result.Entities.FirstOrDefault(x => x.Type == "building");

        //    if (building != null)
        //    {
        //        switch (building.Entity)
        //        {
        //            //教学楼
        //            case "一教": context.PostAsync("一教开放时间：周一~周五 9:00---17:00"); break;
        //            case "二教": context.PostAsync("二教开放时间：周一~周五 9:00---17:00"); break;
        //            case "三教": context.PostAsync("三教开放时间：周一~周五 9:00---17:00"); break;
        //            case "四教": context.PostAsync("四教开放时间：周一~周五 9:00---17:00"); break;

        //            //澡堂
        //            case "北区浴室": context.PostAsync("北区浴室开放时间：周一~周五 9:00---17:00\t周六不开"); break;
        //            case "南区浴室": context.PostAsync("南区浴室开放时间：周一~周五 9:00---17:00"); break;

        //            //校医院
        //            case "校医院": context.PostAsync("校医院开放时间：周一~周五 9:00---17:00"); break;
        //        }
        //        //if (building.Entity == "四教")
        //        //{
        //        //    context.PostAsync("四教开放时间：周一~周五 9:00---17:00");
        //        //}
        //    }

        //    return Task.CompletedTask;
        //}

        //[LuisIntent("QueryTime")]
        //public Task QueryTime
        //    (IDialogContext context, LuisResult result)
        //{
        //    //private BJUT_test001Entities db = new BJUT_test001Entities();
        //    var db = new BJUT_test001Entities();  

        //    var building = result.Entities.FirstOrDefault(x => x.Type == "building");

        //    var buildingsInfor = db.Buildings.Where(x => x.Name == building.Entity);

        //    var buildingOpenHours = buildingsInfor.First().OpenHours;

        //    context.PostAsync(buildingOpenHours);

        //    return Task.CompletedTask;
        //}

        [LuisIntent("QueryTime")]
        public Task QueryTime
            (IDialogContext context, LuisResult result)
        {
            //private BJUT_test001Entities db = new BJUT_test001Entities();
            var db = new BJUT_test001Entities();

            var building = result.Entities.FirstOrDefault(x => x.Type == "building");
            
            if(building != null)
            {
                if ( building.Entity == "澡堂" || building.Entity == "洗澡" || building.Entity=="浴室")  //澡堂时间查询
                {
                    //化为数据库中标准名称
                    if (building.Entity == "澡堂") building.Entity = "浴室";
                    if (building.Entity == "洗澡") building.Entity = "浴室";

                    //数据库查询
                    var buildingsInfor = db.Buildings.Where(x => x.BuildingsName == building.Entity);
                    var buildingName = buildingsInfor.First().BuildingsName; //建筑物Name
                    var buildingOpenHours = buildingsInfor.First().OpenHours; //建筑物OpenHours

                    context.Call(new QueryTimeBathhouseDialog(buildingName, buildingOpenHours), async (ctx, r) =>
                    {
                        var imessageActivity = ctx.Activity as Activity;

                        var returnMessage = imessageActivity.CreateReply();
                        var attachments = await r;
                        returnMessage.Attachments = attachments;

                        await context.PostAsync(returnMessage);
                        context.Wait(MessageReceived);

                    });
                }
                else  //其他建筑物时间查询
                {

                    if (building.Entity == "第一教学楼") building.Entity = "一教";
                    if (building.Entity == "第二教学楼") building.Entity = "二教";
                    if (building.Entity == "第三教学楼") building.Entity = "三教";
                    if (building.Entity == "第四教学楼") building.Entity = "四教";

                    //数据库查询
                    var buildingsInfor = db.Buildings.Where(x => x.BuildingsName == building.Entity);
                    var buildingName = buildingsInfor.First().BuildingsName; //建筑物Name
                    var buildingOpenHours = buildingsInfor.First().OpenHours; //建筑物OpenHours



                    context.Call(new QueryTimeDialog(buildingName, buildingOpenHours), async (ctx, r) =>
                    {
                        var activity = ctx.Activity as Activity;

                        var returnMessage = activity.CreateReply();
                        var attachmentResult = await r;
                        returnMessage.Attachments = new List<Attachment>() { attachmentResult };

                        await context.PostAsync(returnMessage);
                        context.Wait(MessageReceived);

                    });
                }
                
            }
            else
            {
                context.PostAsync("404NotFound");
            }

           

            return Task.CompletedTask;
        }

        [LuisIntent("AskWay")]
        public Task AskWay
            (IDialogContext context, LuisResult result)
        {
            var db = new BJUT_test001Entities();

            var building = result.Entities.FirstOrDefault(x => x.Type == "building");

            if (building != null)
            {
               
                if (building.Entity == "第一教学楼") building.Entity = "一教";
                if (building.Entity == "第二教学楼") building.Entity = "二教";
                if (building.Entity == "第三教学楼") building.Entity = "三教";
                if (building.Entity == "第四教学楼") building.Entity = "四教";

                //数据库查询
                var buildingsInfor = db.Buildings.Where(x => x.BuildingsName == building.Entity);
                var buildingName = buildingsInfor.First().BuildingsName;  //地址Name
                var buildingAddressDescription = buildingsInfor.First().AddressDescription;  //地址简要描述
                //var buildingAddressImage = buildingsInfor.First().AddressImage;  //地址图片
                var buildingPhone = buildingsInfor.First().Phone;  //地址电话
                //var buildingAddress3D = buildingsInfor.First().Address3D;  //目的地三维建模
               
                if (buildingPhone != null)
                {
                    context.PostAsync(buildingName + "电话:" + buildingPhone);
                }

                context.Call(new AskWayDialog(buildingName, buildingAddressDescription,buildingPhone), async (ctx, r) =>
                {
                    var imessageActivity = ctx.Activity as Activity;

                    var returnMessage = imessageActivity.CreateReply();
                    var attachments = await r;
                    returnMessage.Attachments = attachments;

                    await context.PostAsync(returnMessage);
                    context.Wait(MessageReceived);

                });

            }
          
            return Task.CompletedTask;
        }

        //[LuisIntent("AskWay")]
        //public Task AskWay(IDialogContext context, LuisResult result)
        //{
        //    var building = result.Entities.FirstOrDefault(x => x.Type == "building");

        //    if (building != null)
        //    {
        //        switch(building.Entity)
        //        {
        //            //教学楼
        //            case "一教": context.PostAsync("一教在西门东侧"); break;                 
        //            case "二教": context.PostAsync("二教在一教南侧"); break;
        //            case "三教": context.PostAsync("三教在南操西侧"); break;
        //            case "四教": context.PostAsync("四教在南操东侧"); break;
        //            //澡堂
        //            case "北区浴室": context.PostAsync("北区浴室在北门南侧"); break;
        //            case "南区浴室": context.PostAsync("南区浴室在南操西侧"); break;
        //            //校医院
        //            case "校医院": context.PostAsync("校医院在北门外面"); break;
        //            //学校各大部门
        //            case "就业大厅": context.PostAsync("学综楼四楼420室"); break;
        //        }         
        //    }

        //    return Task.CompletedTask;
        //}
     
        [LuisIntent("Telephone")]
        public Task Telephone(IDialogContext context, LuisResult result)
        {
            //学校各个部门电话
            var department = result.Entities.FirstOrDefault(x => x.Type == "department");
            if( department != null )
            {
                switch(department.Entity)
                {
                    case "就业大厅": context.PostAsync("就业大厅：111111111"); break;
                }
            }

            return Task.CompletedTask;
        }

        //模糊问题
        [LuisIntent("Things")]
        public Task Things(IDialogContext context, LuisResult result)
        {
            var things = result.Entities.FirstOrDefault(x => x.Type == "things");

            if (things != null)
            {
                switch (things.Entity)
                {
                    case "一卡通":
                        {
                            context.PostAsync("地点：知新园西门一层右转");
                            context.PostAsync("时间：工作日的早8点~11点、下午2点~5点");
                            context.PostAsync("哦，对了，记着带上学生卡");
                            context.PostAsync("还有，拿30块钱人名币");
                        }
                        break;
                }
            }

            return Task.CompletedTask;
        }

        [LuisIntent("天气.天气预测")]
        public Task WeatherForest(IDialogContext context, LuisResult result)
        {
            cn.com.webxml.www.WeatherWebService w = new cn.com.webxml.www.WeatherWebService();
            //把webservice当做一个类来操作  
            string[] s = new string[23];//声明string数组存放返回结果  
            string city = "北京";//获得文本框录入的查询城市  
            s = w.getWeatherbyCityName(city);
            //for (int i = 0; i < s.Length; i++)
            //{
            //  MessageBox.Show(s[i]);
            //}

            //以文本框内容为变量实现方法getWeatherbyCityName  
            if (s[8] == "")
            {
                context.PostAsync("暂不支持该城市");
            }
            else
            {
                context.PostAsync(s[1] + "\n" + s[5] + "\n" + s[6]);
            }
            

                return Task.CompletedTask;
        }

        [LuisIntent("SearchWebsite")]
        public Task SearchWebsite(IDialogContext context,LuisResult result)
        {
            context.Call(new SearchWebsiteDialog(), async (ctx, r) =>
            {
                var activity = ctx.Activity as Activity;

                var returnMessage = activity.CreateReply();
                var attachmentResult = await r;
                returnMessage.Attachments = new List<Attachment>() { attachmentResult };

                await context.PostAsync(returnMessage);
                context.Wait(MessageReceived);

            });


            return Task.CompletedTask;
        }

        [LuisIntent("选课介绍")]
        public Task IntroductionCourseSelection(IDialogContext context, LuisResult result)
        {
            context.Call(new IntroductionCourseSelectionDialog(), async (ctx, r) =>
            {
                var activity = ctx.Activity as Activity;

                var returnMessage = activity.CreateReply();
                var attachmentResult = await r;
                returnMessage.Attachments = new List<Attachment>() { attachmentResult };

                await context.PostAsync(returnMessage);
                context.Wait(MessageReceived);

            });


            return Task.CompletedTask;
        }

        [LuisIntent("开学须知")]
        public Task NewStudent(IDialogContext context, LuisResult result)
        {
            context.Call(new NewStudentDialog(), async (ctx, r) =>
            {
                var activity = ctx.Activity as Activity;

                var returnMessage = activity.CreateReply();
                var attachmentResult = await r;
                returnMessage.Attachments = new List<Attachment>() { attachmentResult };

                await context.PostAsync(returnMessage);
                context.Wait(MessageReceived);

            });


            return Task.CompletedTask;
        }

        [LuisIntent("攻略系统")]
        public async Task StrategySystemAsync(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "校医院攻略",
                    "Offload the heavy lifting of data center management",
                    "Store and help protect your data. Get durable, highly available data storage across the globe and pay only for what you use.",
                    new CardImage(url: "https://docs.microsoft.com/en-us/aspnet/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/data-storage-options/_static/image5.png"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/storage/")),
                GetThumbnailCard(
                    "外网访问教务攻略",
                    "Blazing fast, planet-scale NoSQL",
                    "NoSQL service for highly available, globally distributed apps—take full advantage of SQL and JavaScript over document and key-value data without the hassles of on-premises or virtual machine-based cloud database options.",
                    new CardImage(url: "https://docs.microsoft.com/en-us/azure/documentdb/media/documentdb-introduction/json-database-resources1.png"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/documentdb/")),
                GetHeroCard(
                    "保研攻略",
                    "Process events with a serverless code architecture",
                    "An event-based serverless compute experience to accelerate your development. It can scale based on demand and you pay only for the resources you consume.",
                    new CardImage(url: "https://msdnshared.blob.core.windows.net/media/2016/09/fsharp-functions2.png"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/functions/")),
                GetThumbnailCard(
                    "考研攻略",
                    "Build powerful intelligence into your applications to enable natural and contextual interactions",
                    "Enable natural and contextual interaction with tools that augment users' experiences using the power of machine-based intelligence. Tap into an ever-growing collection of powerful artificial intelligence algorithms for vision, speech, language, and knowledge.",
                    new CardImage(url: "https://msdnshared.blob.core.windows.net/media/2017/03/Azure-Cognitive-Services-e1489079006258.png"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/cognitive-services/")),
            };
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}
