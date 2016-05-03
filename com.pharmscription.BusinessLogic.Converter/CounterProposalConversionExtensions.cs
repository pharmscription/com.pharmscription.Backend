
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

        public static ICollection<CounterProposal> ConvertToEntities(this IReadOnlyCollection<CounterProposalDto> list)
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
        /// <param name="counterproposal"></param>
        /// <returns>null when it get null as parameter</returns>
        public static CounterProposalDto ConvertToDto(this CounterProposal counterproposal)
        {
            if (counterproposal == null) return null;
            var counterproposalDto = new CounterProposalDto
            {
                Message = counterproposal.Message,
                Date = counterproposal.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
                Id = counterproposal.Id.ToString()
            };
            return counterproposalDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="counterproposalDto"></param>
        /// <returns>null when it get null as parameter</returns>
        public static CounterProposal ConvertToEntity(this CounterProposalDto counterproposalDto)
        {
            if (counterproposalDto == null) return null;
            var counterproposal = new CounterProposal
            {
                Message = counterproposalDto.Message,
                Date = DateTime.Parse(counterproposalDto.Date, CultureInfo.CurrentCulture)
            };
            if (!string.IsNullOrWhiteSpace(counterproposalDto.Id))
            {
                counterproposal.Id = new Guid(counterproposalDto.Id);
            }
            return counterproposal;
        }

        public static bool DtoEqualsEntity(this CounterProposalDto counterproposalDto, CounterProposal counterproposal)
        {
            if (counterproposalDto == null || counterproposal == null)
            {
                return false;
            }
            return counterproposalDto.Date.Equals(counterproposal.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture)) &&
                   counterproposalDto.Message == counterproposal.Message &&
                   counterproposalDto.Id == counterproposal.Id.ToString();
        }
        public static bool EntityEqualsDto(this CounterProposal counterproposal, CounterProposalDto counterproposalDto)
        {
            return DtoEqualsEntity(counterproposalDto, counterproposal);
        }

        public static bool DtoListEqualsEntityList(this IReadOnlyCollection<CounterProposalDto> counterproposalDtos, IReadOnlyCollection<CounterProposal> counterproposals)
        {
            return !counterproposalDtos.Where((t, i) => !counterproposalDtos.ElementAt(i).DtoEqualsEntity(counterproposals.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this IReadOnlyCollection<CounterProposal> counterproposals, IReadOnlyCollection<CounterProposalDto> counterproposalDtos)
        {
            return DtoListEqualsEntityList(counterproposalDtos, counterproposals);
        }
    }
}
