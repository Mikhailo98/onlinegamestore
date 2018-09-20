using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Extension;

namespace BusinessLogicLayer.Configuration
{
    public class Extension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IUnitOfWork, UnitOfWork>();

            Container.RegisterType<IGameService, GameService>();

            Container.RegisterType<IPublisherService, PublisherService>();
        }
    }
}
