using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using Tragate.Common.Library;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Interfaces.Email;
using Tragate.Domain.Models;

namespace Tragate.Domain.EventHandlers
{
    /// <summary>
    /// Burada Email hem handler hem de generic olarak kullanılmaktadır.Çünkü tüm emaillerin
    /// kaydedilmesi gerekirken ayrıca gold memberlara ozel generic email templatelerinin tanımlamaları yapılmaktadır.
    /// Handler ve Email Save işlemi için ayrıca bir tasarıma gerek duyulmadıgından ve template ve kayıt işlemleri de bir event
    /// oldugundan daha alt izolasyona gidilmesine gerek duyulmamıstır.
    /// </summary>
    public class EmailHandler : INotificationHandler<EmailSentEvent>
    {
        private readonly Dictionary<MembershipPackageType, IEMailHandler> _handlerList =
            new Dictionary<MembershipPackageType, IEMailHandler>();

        private readonly IEmailRepository _emailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public EmailHandler(IEmailService emailService, IOptions<ConfigSettings> settings,
            IEmailRepository emailRepository, IMapper mapper, IUserRepository userRepository){
            _emailRepository = emailRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _handlerList.Add(MembershipPackageType.Gold, new GoldMemberHandler(emailService, settings.Value));
        }

        /// <summary>
        /// Generic Email Template Selector
        /// </summary> 
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void Execute(MembershipPackageType type, object data){
            _handlerList[type].Execute(data);
        }

        /// <summary>
        /// All Email Saving
        /// </summary>
        /// <param name="message"></param>
        public void Handle(EmailSentEvent message){
            foreach (var to in message.To){
                var entity = _mapper.Map<Email>(message);
                var user = _userRepository.GetByEmail(to);
                if (user == null){
                    Log.Error("User not found when email is saving");
                    return;
                }

                entity.To = to;
                entity.UserId = user.Id;
                _emailRepository.Add(entity);
            }

            _emailRepository.SaveChanges();
        }
    }
}