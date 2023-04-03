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
            CreateMap<Bet, ReadBetDto>()
                .ForMember(r =>  r.FriendName , opt => opt.MapFrom(b => b.Status.Contains("*") || b.Status == "Awaiting" ? b.User.FirstName: b.Friend.FirstName))
                .ForMember(r => r.JudgeName, opt => opt.MapFrom(b => b.Judge != null ? b.Judge.FirstName : null));
            CreateMap<ReadBetDto, Bet>();
        }
    }
}