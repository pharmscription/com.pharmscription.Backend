
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class CounterProposalConversionExtensions
    {
        public static List<CounterProposalDto> ConvertToDtos(this ICollection<CounterProposal> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<CounterProposalDto>(list.Count);
            newList.AddRange(list.Select(counterProposal => counterProposal.ConvertToDto()));
            return newList;
        }

        public static ICollection<CounterProposal> ConvertToEntites(this IReadOnlyCollection<CounterProposalDto> list)
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
                Date = counterProposal.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
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
            var counterProposal = new CounterProposal
            {
                Message = counterProposalDto.Message,
                Date = DateTime.Parse(counterProposalDto.Date, CultureInfo.CurrentCulture)
            };
            if (!string.IsNullOrWhiteSpace(counterProposalDto.Id))
            {
                counterProposal.Id = new Guid(counterProposalDto.Id);
            }
            return counterProposal;
        }

        public static bool DtoEqualsEntity(this CounterProposalDto counterProposalDto, CounterProposal counterProposal)
        {
            if (counterProposalDto == null || counterProposal == null)
            {
                return false;
            }
            return counterProposalDto.Date.Equals(counterProposal.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture)) &&
                   counterProposalDto.Message == counterProposal.Message &&
                   counterProposalDto.Id == counterProposal.Id.ToString();
        }
        public static bool EntityEqualsDto(this CounterProposal counterProposal, CounterProposalDto counterProposalDto)
        {
            return DtoEqualsEntity(counterProposalDto, counterProposal);
        }

        public static bool DtoListEqualsEntityList(this IReadOnlyCollection<CounterProposalDto> counterProposalDtos, IReadOnlyCollection<CounterProposal> counterProposals)
        {
            return !counterProposalDtos.Where((t, i) => !counterProposalDtos.ElementAt(i).DtoEqualsEntity(counterProposals.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this IReadOnlyCollection<CounterProposal> counterProposals, IReadOnlyCollection<CounterProposalDto> counterProposalDtos)
        {
            return DtoListEqualsEntityList(counterProposalDtos, counterProposals);
        }
    }
}
