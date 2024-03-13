using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClasses 
{ 
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<ShelfItem, Item>()
                .ForMember(x => x.ItemID, y => y.MapFrom(z => z.ItemID))
                .ForMember(x => x.ItemName, y => y.MapFrom(z => z.ItemName))
                .ForMember(x => x.ItemCPrice, y => y.MapFrom(z => z.ItemCPrice))
                .ForMember(x => x.ItemVat, y => y.MapFrom(z => z.ItemVat))
                .ForMember(x => x.ItemPrice, y => y.MapFrom(z => z.ItemPrice))
                .ForMember(x => x.ItemDiscount, y => y.MapFrom(z => z.ItemDiscount))
                .ForMember(x => x.ItemAmount, y => y.MapFrom(z => z.ItemAmount))
                .ForMember(x => x.CategoryID, y => y.MapFrom(z => z.CategoryID));          
        }
    }
}

