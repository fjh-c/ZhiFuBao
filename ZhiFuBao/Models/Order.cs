using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZhiFuBao.Models
{
    public class Order
    {
        //商品描述
        public string GoodsMs { get; set; }
        //商品名称
        public string OrderName { get; set; }
        //商品价格
        public string Count { get; set; }
        //订单的编号
        public string OrderNo { get; set; }
    }
}
