using Dal;
using DbGateway.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbGateway
{
    public static class Gateway
    {
        public static long AddProductReturnId(string Imei)
        {
            Product p = new Product() { Imei = Imei, State = 1 };
            using (RedisStressContext ctx = new RedisStressContext())
            {
                ctx.Products.Add(p);
                ctx.SaveChanges();
            }
            return p.Id;
        }
        public static ProductDto AddProductReturnDto(string Imei)
        {
            Product p = new Product() { Imei = Imei, State = 1 };
            using (RedisStressContext ctx = new RedisStressContext())
            {
                ctx.Products.Add(p);
                ctx.SaveChanges();
            }
            return AutoMapper.Mapper.Map<ProductDto>(p);
        }
    }
}
