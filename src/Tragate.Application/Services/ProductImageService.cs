using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IMediatorHandler bus, IProductImageRepository productImageRepository,
            IMapper mapper){
            _bus = bus;
            _productImageRepository = productImageRepository;
            _mapper = mapper;
        }

        public ProductImageDto GetById(int id){
            return _mapper.Map<ProductImageDto>(_productImageRepository.GetById(id));
        }

        public void UploadImages(Guid uuid, int userId, IFormFileCollection files){
            var uploadImageCommand = new AddNewProductImageCommand()
            {
                UuId = uuid,
                Files = files,
                UpdatedUserId = userId
            };
            _bus.SendCommand(uploadImageCommand);
        }

        public void DeleteImage(int id){
            _bus.SendCommand(new DeleteProductImageCommand() {Id = id});
        }
    }
}