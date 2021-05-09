using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application {
    public class ContentService : IContentService {
        private readonly IContentRepository _contentRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public ContentService (IContentRepository contentRepository,
            IMapper mapper,
            IMediatorHandler bus) {
            _contentRepository = contentRepository;
            _bus = bus;
            _mapper = mapper;
        }

        public IQueryable<ContentDto> GetAll () {
            return _contentRepository.GetAll ().ProjectTo<ContentDto> ();
        }

        public ContentDto GetBySlug (string slug, int statusId) {
            return _mapper.Map<ContentDto> (_contentRepository.GetBySlug (slug, statusId));
        }

        public ContentDto GetById (int id) {
            return _mapper.Map<ContentDto> (_contentRepository.GetById (id));
        }

        public void AddContent (ContentViewModel model) {
            var registerCommand = _mapper.Map<AddNewContentCommand> (model);
            _bus.SendCommand (registerCommand);
        }

        public void UpdateContent (ContentViewModel model) {
            var updateCommand = _mapper.Map<UpdateContentCommand> (model);
            _bus.SendCommand (updateCommand);
        }

        public void Dispose () {
            GC.SuppressFinalize (this);
        }
    }
}