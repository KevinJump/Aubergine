using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aubergine.UserContent.Models;
using Aubergine.UserContent.Persistance.Models;
using AutoMapper;

namespace Aubergine.UserContent.Persistance.Mappers
{
    internal class InitializeMappers
    {
        public InitializeMappers() { }

        public void CreateMappings()
        {
            Mapper.CreateMap<UserContentDTO, UserContentItem>()
                .ForMember(x => x.Properties,
                    opt => opt.ResolveUsing(new UserContentPropertiesResolver()));

            Mapper.CreateMap<UserContentItem, UserContentDTO>()
                .ForMember(x => x.PropertyData,
                    opt => opt.ResolveUsing(new UserContentToDTOPropertiesResolver()));
        }
    }
}
