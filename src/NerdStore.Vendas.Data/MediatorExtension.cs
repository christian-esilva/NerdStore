﻿using NerdStore.Core.DomainObjects;
using NerdStore.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, VendasContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entidade>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
