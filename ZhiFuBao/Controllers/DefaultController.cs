using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhiFuBao.Models;

namespace ZhiFuBao.Controllers
{

    // 参考网站https://blog.csdn.net/qq_42532852/article/details/107646339?spm=1001.2014.3001.5501
    public class DefaultController : Controller
    {
        //沙箱环境，请求支付链接的地址           
        const string URL = "https://openapi.alipaydev.com/gateway.do";
        //支付宝正式环境 
        //https://openapi.alipaydev.com/gateway.do   
        //APPID即创建应用后生成,沙箱环境中的AppId
        const string APPID = "2021000116696629";
        //这里是我们之前用秘钥工具生成的商户私钥
        const string APP_PRIVATE_KEY = "MIIEowIBAAKCAQEAi3tVUjyKsMzHgbnWeikW/zDMS1TbdACZB33TvKybOcZj3+SJZ+nIvHpMsVhzCWxwSVul8VyBo6/E/J56L7T56i0pkwg35lTrQFIVi19zUGIPDcz7ehAJPvg15otViD+mV5HQaZDgBFzrIutWoVlbAoGB909/03Q+THfxvRhSk0gJmBi8HWBZIU3+8PvSTrHd2Z4mT9DArevxvkAkJi4DZcLcecf3CvZllyrN/IwbgcLkktri9/FIKF2fgS6nsaAgvTwnWwtImuoiAsukWO0Qv9li/27Z0b5gFIILdBayjBboGXg5ZHTo6JXRLd1P8wYy6Eo7SMd40IlpXTZnevMtRQIDAQABAoIBADdD69+Eq3M4AEivSILoqvr768ZhXf6NihTEz1NvlKWErOf2Vlc881NypdaS9CmktKzsSkji3V7s8wEhDcq/S1J1CIcAd52kwf2fSHgLntYXtF1EfdO0bOVtVXX+eSpeBdumo8xgIdK4ulYApzeQ/xV0GPnTnxM6eMC1uaux6m3+Q95BSg7RrW9BdMPlz6B7Fxwgr8uWAk5jyuDaNFYoM4c9r4cm9k4UVfQq5kmhhgf/RTee/OL3gyFJN3zFSTKWoF/IRwuQ9FR1U+JhbskmuMisGQxkdE+zDoenUytRwKYTQdBErxJSu4UzhXyZbw3FtfL5SLItoTItU0bGtZ/eYQECgYEAy43xAjXPHvzNx5sb+Y8F1xdccqNqXg+m9A3GcNETydApENg9rAlp92sWgoK+CIZL8DivM5tul7585bCGtKyHlVfXhOm/2Wx0BFRFkwMYKoOKDvim2IbNXhTw5AJTBlFN+RmpWHrcT/gZd3uAL5NDBkgF9EdNXEtU5Wv/E3qeyoUCgYEAr2tH/ZLJDeM/VggnIShiN/YV7hzosrnQSpIcIUrdJMxGWpzkVmmAezhYS6DY32qUJ1yuvbnUJnujxZ2y5xAFYS/kv+bb+SZDKbjXtKh2REN/fZX2rcxVMv0nL11J5hoJC6xFTrBMqFvnxgNBQmn89/RMuDpeP0OhSBbbTaqUM8ECgYBM8/RRKZaL2HbTJ0iEKBXFIwfILw9fT/uF+E81B1W7zf44aAeoqkZtSwbPgC3nex3qSwOxNumwZEtDamECnNcFFphbnyLwR5f0qwdCdMEGEYR39HjfEtdA4hnPmTFaChK4QYNL9+aiNOEr0ny7f6ivDztxBiOw6KnHsfRUoN63IQKBgHqtBAOJi6FysvBvcy31F/sa1etXvESBPfFojNObj1wUn37vCaCE151UoisBqalU15dPccStGHhxLu8cgeilg52b6NVrlZp/76hV5EQrK8pBif9ByOlyZILsF2NAUlXhD0Jij3WgRXvYChaoYBMaHBcrehu8dfy4RyorEeMWz94BAoGBALPguyLGpy6upgE6sXAdPb1hBYn5/peomLHnE8vwUcxWC/YTfJgwHzbO+qg2DbYDk3MpQkVcYfWITW7/2ACX38wpz1q/LP8ldGOkEusEMY6PPSUye1xmzITb1yN6cZZTx8UqjEbluXpO4R4WadA8aU+CpNF6S3SBKcID3ODILvHP";
        //参数返回格式，只支持json
        const string FORMAT = "json";
        //支持GBK和UTF-8
        const string CHARSET = "UTF-8";
        //支付宝公钥
        const string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAioKfS/s7dGagN2Ywc2HtIUWgRcoDeoXyuZWjHafC3V2JGdgOYVh1mhiaabfSaFbxW7GL35+l3Zhdp+2qjXZIKvJV4FTQ8iT6e+BaXjJIv/tEndiPMPclnDtC2Jptmab0H22TKzwSp2w2snKBT1wZ/Sust598dEeUuiQUo/+MY0Sre18glqZ72cnB/eV7BJNBzDC1jaru9QTXdUMlkNx1y6d9KnVOs79ZNsPAj/E7nfJ0rmsmTEom0xFJgReDhlo+dIveAfph6+yS7CAmdvf1KrCopDgLJ2Bv3uTqrVaNR+MgNAYg2d/zBzsN4SH/0mWEYYXlX4qtH+YY9H++V1RVwwIDAQAB";


        [HttpPost]
        public ActionResult Show(Order order)
        {
            IAopClient client = new DefaultAopClient(URL, APPID, APP_PRIVATE_KEY, FORMAT, "2.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：
            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();//创建API对应的request类,请求返回二维码
            AlipayTradePagePayRequest requestPagePay = new AlipayTradePagePayRequest();//请求返回支付宝支付网页
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            //主要注意的是这个地方的值
            model.Body = order.GoodsMs;
            model.Subject = order.OrderName;
            model.TotalAmount = order.Count;
            model.OutTradeNo = DateTime.Now.ToString("yyyyMMddHHmmss"); ;//订单号我们是直接用日期产生的
            model.StoreId = "William001";
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";
            requestPagePay.SetBizModel(model);
            //这是要注意的，支付后就要通过他完成回调，这里填写你要跳转页面的地址
            requestPagePay.SetReturnUrl("http://47.106.255.80:8080/Snail/#/OrderDetailss");
            var response = client.SdkExecute(requestPagePay);//Execute(request);
            if (!response.IsError)
            {
                var res = new
                {
                    success = true,
                    out_trade_no = response.OutTradeNo,
                    // qr_code = response.QrCode,    //二维码字符串
                    pay_url = URL + "?" + response.Body
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    success = false,
                };
                return Json(res);
            }

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
