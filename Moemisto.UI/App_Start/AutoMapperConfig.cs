using System;
using System.Linq;
using AutoMapper;
using Moemisto.Data.Entities;
using Moemisto.UI.Areas.Admin.Models;
using Moemisto.UI.Models;

namespace Moemisto.UI
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.CreateMap<EventSchedule, EventScheduleVm>()
                .AfterMap((src, dest) =>
                {
                    string res = src.StartEventList.Aggregate(String.Empty, (current, item) => current + item.StartEvent.ToString("hh:mm dd MMMM") + ", ");
                    dest.StartEventListStr = res.Substring(0, res.Length < 2 ? 0 : res.Length - 2);
                    //var days = src.StartEventList.GroupBy(g => new { day = g.StartEvent.Day, month = g.StartEvent.Month }).Select(s => new { s.Key.day, s.Key.month });
                    //dest.StartEventList = new Dictionary<string, string>();
                    //foreach (var dayItem in days)
                    //{
                    //    dest.StartEventList.Add(string.Format("{0} {1}",dayItem.day, dayItem.month), );
                    //}
                });

            Mapper.CreateMap<Event, EventBaseVm>()
                .ForMember(d => d.PosterUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileName)))
                .ForMember(d => d.PosterSmallUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileNameSmall)))
                .ForMember(d => d.CategoryName,
                    opt => opt.MapFrom(src => src.Type.TitleOne))
                .ForMember(d => d.CategoryUrlName,
                    opt => opt.MapFrom(src => src.Type.Title))
                .ForMember(d => d.CategoryUrl,
                    opt => opt.MapFrom(src => src.Type.TranslitUrl));

            Mapper.CreateMap<Place, PlaceBaseVm>()
                .ForMember(d => d.PosterUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileName)))
                .ForMember(d => d.PosterSmallUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileNameSmall)));

            Mapper.CreateMap<Article, ArticleBaseVm>()
                .ForMember(d => d.TitleShort, opt => opt.MapFrom(scr => String.IsNullOrEmpty(scr.TitleShort) ? scr.Title : scr.TitleShort))
                .AfterMap((src, dest) =>
                {
                    var picture = src.Pictures.FirstOrDefault(w => w.Top);
                    if (picture != null)
                    {
                        dest.PictureUrlTop = String.Format("{0}{1}", picture.Path, picture.FileName);
                        dest.PictureUrlHomeTop = String.Format("{0}{1}", picture.Path, picture.FileNameHomeTop);
                        dest.PictureUrlSmall = String.Format("{0}{1}", picture.Path, picture.FileNameSmall);
                    }
                });

            Mapper.CreateMap<Article, NewsDetailsVm>()
                .AfterMap((src, dest) =>
                {
                    var picture = src.Pictures.FirstOrDefault(w => w.Top);
                    if (picture != null)
                    {
                        dest.PictureUrlTop = String.Format("{0}{1}", picture.Path, picture.FileName);
                        dest.PictureUrlSmall = String.Format("{0}{1}", picture.Path, picture.FileNameSmall);
                    }
                });

            Mapper.CreateMap<PlaceTypeGroup, PlaceTypeBaseVm>()
                .ForMember(d => d.Id, opt => opt.MapFrom(scr => scr.PlaceTypeGroupId))
                .ForMember(d => d.ActionName, opt => opt.UseValue("Types"));
            Mapper.CreateMap<PlaceType, PlaceTypeBaseVm>()
                .ForMember(d => d.Id, opt => opt.MapFrom(scr => scr.PlaceTypeId))
                .ForMember(d => d.ActionName, opt => opt.UseValue("List"));

            Mapper.CreateMap<Place, PlaceDetailsVm>()
                .ForMember(d => d.TypeTitle, opt => opt.MapFrom(scr => scr.PlaceType.Title))
                .ForMember(d => d.TypeTitleOne, opt => opt.MapFrom(scr => scr.PlaceType.TitleOne))
                .ForMember(d => d.PlaceTypeGroupId, opt => opt.MapFrom(scr => scr.PlaceType.PlaceTypeGroupId))
                .ForMember(d => d.PosterUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileName)))
                .ForMember(d => d.PosterSmallUrl,
                    opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileNameSmall)));

            Mapper.CreateMap<Article, SearchItemVm>()
                .ForMember(d => d.ItemId, opt => opt.MapFrom(scr => scr.ArticleId))
                .ForMember(d => d.PictureUrl,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.Pictures.Where(w => w.Top)
                                    .Select(s => String.Format("{0}{1}", s.Path, s.FileNameSmall))
                                    .FirstOrDefault()))
                .ForMember(d => d.ActionName, opt => opt.UseValue("Details"))
                .ForMember(d => d.ControllerName, opt => opt.UseValue("News"));
            Mapper.CreateMap<Event, SearchItemVm>()
                .ForMember(d => d.ItemId, opt => opt.MapFrom(scr => scr.EventId))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath,src.PictureFileNameSmall)))
                .ForMember(d => d.ActionName, opt => opt.UseValue("Details"))
                .ForMember(d => d.ControllerName, opt => opt.UseValue("Event"));
            Mapper.CreateMap<Place, SearchItemVm>()
                .ForMember(d => d.ItemId, opt => opt.MapFrom(scr => scr.PlaceId))
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom(src => String.Format("{0}{1}", src.PicturePath, src.PictureFileNameSmall)))
                .ForMember(d => d.ActionName, opt => opt.UseValue("Details"))
                .ForMember(d => d.ControllerName, opt => opt.UseValue("Place"));

            #region Admin

            Mapper.CreateMap<Article, AdminNewsEditVm>();
               // .ForMember(d => d.TimePublish, opt => opt.MapFrom(scr => scr.DatePublish));

            Mapper.CreateMap<ArticleBaseVm, Article>();
                //.AfterMap((src, dest) =>
                //{
                //    DateTime publishDateTime = src.DatePublish.Date;
                //    publishDateTime = publishDateTime.AddHours(src.TimePublish.Hour).AddMinutes(src.TimePublish.Minute);
                //    dest.DatePublish = publishDateTime;
                //});
            Mapper.CreateMap<Picture, AdminPictureItemVm>()
                .ForMember(d => d.Url, opt => opt.MapFrom(scr => String.Format("{0}{1}", scr.Path, scr.FileNameSmall ?? scr.FileName)));

            Mapper.CreateMap<EventBaseVm, Event>()
                .ForMember(d => d.Type, opt => opt.Ignore());

            Mapper.CreateMap<EventSchedule, AdminEventScheduleBaseVm>()
                .ForMember(d => d.StartEventList, opt => opt.MapFrom(scr => scr.StartEventList.Where(w => w.StartEvent >= today).Select(s => s.StartEvent).ToList()));

            Mapper.CreateMap<AdminEventScheduleBaseVm, EventSchedule>()
                .ForMember(d => d.StartEventList, opt => opt.MapFrom(scr => scr.StartEventList.Select(s => new EventScheduleDateTime { StartEvent = s, EventScheduleId = scr.EventScheduleId }).ToList()));

            #endregion
        }
    }
}