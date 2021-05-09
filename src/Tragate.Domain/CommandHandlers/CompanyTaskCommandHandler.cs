using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class CompanyTaskCommandHandler : CommandHandler,
        INotificationHandler<AddNewCompanyTaskCommand>,
        INotificationHandler<UpdateStatusCompanyTaskCommand>,
        INotificationHandler<DeleteCompanyTaskCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyTaskRepository _companyTaskRepository;

        public CompanyTaskCommandHandler(IUnitOfWork uow, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            ICompanyTaskRepository companyTaskRepository, IMapper mapper) : base(uow,
            bus, notifications){
            _companyTaskRepository = companyTaskRepository;
            _mapper = mapper;
        }

        public void Handle(AddNewCompanyTaskCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var companyTask = _mapper.Map<CompanyTask>(message);
                _companyTaskRepository.Add(companyTask);
                base.Commit();
            }
        }

        public void Handle(UpdateStatusCompanyTaskCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var companyTask = _companyTaskRepository.GetById(message.Id);
                if (companyTask != null){
                    if (!(companyTask.CreatedUserId == message.UpdatedUserId ||
                          companyTask.ResponsibleUserId == message.UpdatedUserId)){
                        base.RaiseError(message, "The responsible or created of task can update status");
                        return;
                    }

                    companyTask.StatusId = (byte) message.StatusId;
                    companyTask.UpdatedUserId = message.UpdatedUserId;
                    _companyTaskRepository.Update(companyTask);
                    base.Commit();
                }
            }
        }

        public void Handle(DeleteCompanyTaskCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var companyTask = _companyTaskRepository.GetById(message.Id);
                if (companyTask != null){
                    companyTask.StatusId = (byte) StatusType.Deleted;
                    _companyTaskRepository.Update(companyTask);
                    base.Commit();
                }
                else{
                    base.RaiseError(message, "Company task not found");
                }
            }
        }
    }
}