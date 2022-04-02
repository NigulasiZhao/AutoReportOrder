using AutoReportOrder.Common;
using AutoReportOrder.UserInfo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace AutoReportOrder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserConfig UserConfigModel = GetUserConfig();
            ResponseResult<UserModel> UserInfo = GetUserModel(UserConfigModel);
            Random rd = new Random();

            if (UserInfo.data != null)
            {
                Console.WriteLine("登陆成功!登陆账号为:" + UserInfo.data.mobile);
                if (UserConfigModel.Cycles != 0)
                {
                    for (int i = 0; i < UserConfigModel.Cycles; i++)
                    {
                        SaveRpair(UserInfo.data, UserConfigModel);
                        Thread.Sleep(rd.Next((UserConfigModel.Interval - 5) * 1000 > 0 ? (UserConfigModel.Interval - 5) * 1000 : 0, (UserConfigModel.Interval + 5) * 1000));
                    }
                }
                else
                {
                    while (true)
                    {
                        SaveRpair(UserInfo.data, UserConfigModel);
                        Thread.Sleep(rd.Next((UserConfigModel.Interval - 5) * 1000 > 0 ? (UserConfigModel.Interval - 5) * 1000 : 0, (UserConfigModel.Interval + 5) * 1000));
                    }
                }
            }
            else
            {
                Console.WriteLine("登陆失败!" + UserInfo.message);
            }
            Console.ReadKey();
        }
        public static HttpClient client = new HttpClient();
        /// <summary>
        /// 根据JSON获取配置文件
        /// </summary>
        /// <returns></returns>
        public static UserConfig GetUserConfig()
        {
            StreamReader sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserConfig.json"), Encoding.GetEncoding("GB2312"));
            UserConfig UserConfigModel = JsonConvert.DeserializeObject<UserConfig>(sr.ReadToEnd());
            return UserConfigModel;
        }
        /// <summary>
        /// 登陆获取用户信息
        /// </summary>
        /// <param name="UserConfigModel"></param>
        /// <returns></returns>
        public static ResponseResult<UserModel> GetUserModel(UserConfig UserConfigModel)
        {
            string data = JsonConvert.SerializeObject(new
            {
                sign_method = "md5",
                app_session = "f81eb851a320a29b7f792d99f85f4abe",
                method = "post_user_code",
                app_key = "6c5ea2ca96c165e86cd11f83875f0135-8",
                charset = "UTF-8",
                format = "json",
                @params = new
                {
                    phone = UserConfigModel.UserName,
                    registrationId = "18171adc03cf63ee4d6",
                    password = UserConfigModel.Password
                },
                platform = "iOS",
                version = "3.5.3",
                sign = "0f417960a5a38d1b6e1b106753392fd1",
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            var buffer = Encoding.UTF8.GetBytes(data);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("https://mitemp.rwjservice.com/mifan/loginUC2PW", byteContent).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            ResponseResult<UserModel> Result = JsonConvert.DeserializeObject<ResponseResult<UserModel>>(result);
            return Result;
        }
        /// <summary>
        /// 提交投诉工单
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="UserConfigModel"></param>
        public static void SaveRpair(UserModel userModel, UserConfig UserConfigModel)
        {
            string data = JsonConvert.SerializeObject(new
            {
                houseId = userModel.house.HouseId,
                mobile = userModel.mobile,
                accountId = userModel.id,
                taskTypeId = 19,
                remark = UserConfigModel.Content,
                images = new string[] { },
            });
            var buffer = Encoding.UTF8.GetBytes(data);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("https://mitemp.rwjservice.com/mifan/repairs/saveRpair", byteContent).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }
    }
}
