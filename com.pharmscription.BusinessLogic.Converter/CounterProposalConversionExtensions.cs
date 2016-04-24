
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class CounterProposalConversionExtensions
    {
        public static List<CounterProposalDto> ConvertToDtos(this List<CounterProposal> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<CounterProposalDto>(list.Count);
            newList.AddRange(list.Select(counterProposal => counterProposal.ConvertToDto()));
            return newList;
        }

        public static List<CounterProposal> ConvertToEntites(this List<CounterProposalDto> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<CounterProposal>(list.Count);
            newList.AddRange(list.Select(counterProposalDto => counterProposalDto.ConvertToEntity()));
            return newList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counterProposal"></param>
        /// <returns>null when it get null as parameter</returns>
        public static CounterProposalDto ConvertToDto(this CounterProposal counterProposal)
        {
            if (counterProposal == null) return null;
            var counterProposalDto = new CounterProposalDto
            {
                Message = counterProposal.Message,
                Date = counterProposal.Date.ToString(CultureInfo.InvariantCulture),
                Id = counterProposal.Id.ToString()
            };
            return counterProposalDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counterProposalDto"></param>
        /// <returns>null when it get null as parameter</returns>
        public static CounterProposal ConvertToEntity(this CounterProposalDto counterProposalDto)
        {
            if (counterProposalDto == null) return null;
            var counterProposalGuid = string.IsNullOrWhiteSpace(counterProposalDto.Id) ? new Guid() : new Guid(counterProposalDto.Id);
            var counterProposal = new CounterProposal
            {
                Message = counterProposalDto.Message,
                Date = DateTime.Parse(counterProposalDto.Date),
                Id = counterProposalGuid
            };
            return counterProposal;
        }

        public static bool DtoEqualsEntity(this CounterProposalDto counterProposalDto, CounterProposal counterProposal)
        {
            return counterProposalDto.Date.Equals(counterProposal.Date.ToString(CultureInfo.InvariantCulture)) &&
                   counterProposalDto.Message == counterProposal.Message &&
                   counterProposalDto.Id == counterProposal.Id.ToString();
        }
        public static bool EntityEqualsDto(this CounterProposal counterProposal, CounterProposalDto counterProposalDto)
        {
            return DtoEqualsEntity(counterProposalDto, counterProposal);
        }

        public static bool DtoListEqualsEntityList(this List<CounterProposalDto> counterProposalDtos, List<CounterProposal> counterProposals)
        {
            return !counterProposalDtos.Where((t, i) => !counterProposalDtos.ElementAt(i).DtoEqualsEntity(counterProposals.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this List<CounterProposal> counterProposals, List<CounterProposalDto> counterProposalDtos)
        {
            return DtoListEqualsEntityList(counterProposalDtos, counterProposals);
        }
    }
}
