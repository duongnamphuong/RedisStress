using AutoMapper;
using Dal;
using DbGateway.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbGateway
{
    public static class Map
    {
        public static void CreateMapAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ProductDto, Product>());
        }
    }
}
