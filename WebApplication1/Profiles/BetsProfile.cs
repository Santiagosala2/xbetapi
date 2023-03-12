using AutoMapper;
using Bets.Dtos;
using Bets.Models;

namespace Bets.Profiles
{
    public class BetsProfile : Profile
    {
        public BetsProfile()
        {
            //Source -> Target
            CreateMap<CreateBetDto, Bet>();
            CreateMap<Bet, CreateBetDto>();
            CreateMap<Bet, ReadBetDto>();
            CreateMap<ReadBetDto, Bet>();
        }
    }
}