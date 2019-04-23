﻿using BenefactAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Replicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BenefactAPI.Controllers
{
    [ReplicateType]
    [ReplicateRoute(Route = "")]
    public class BoardsInterface
    {
        IServiceProvider Services;
        public BoardsInterface(IServiceProvider services)
        {
            Services = services;
        }

        Expression<Func<CardData, bool>> termCardExpression(CardQueryTerm term)
        {
            var andTerms = new List<Expression<Func<CardData, bool>>>();
            if (term.Tags != null)
            {
                andTerms.AddRange(term.Tags.SelectExp<int, CardData, bool>(
                    tagId => card => card.Tags.Any(cardTag => cardTag.TagId == tagId)));
            }
            if (term.Title != null)
                andTerms.Add(card => card.Title.ToLower().Contains(term.Title.ToLower()));
            if (term.ColumnId.HasValue)
                andTerms.Add(card => card.Column.Id == term.ColumnId);
            if (!andTerms.Any())
                andTerms.Add(c => true);
            return andTerms.BinaryCombinator(Expression.And);
        }

        IQueryable<CardData> FilterCards(IQueryable<CardData> baseQuery, List<CardQueryTerm> terms)
        {
            var query = baseQuery;
            if (terms != null)
            {
                var exp = terms.SelectExp(termCardExpression).BinaryCombinator(Expression.Or);
                query = query.Where(exp);
            }
            return query;
        }

        [AuthRequired(RequirePrivilege = Privilege.Read)]
        [ReplicateRoute(Route = "/")]
        public Task<CardsResponse> Get(CardQuery query)
        {
            query = query ?? new CardQuery();
            query.Groups = query.Groups ?? new Dictionary<string, List<CardQueryTerm>>() { { "All", null } };
            var boardId = BoardExtensions.Board.Id;
            return Services.DoWithDB(async db =>
            {
                IQueryable<CardData> baseQuery = db.Cards
                    .BoardFilter()
                    .Include(card => card.Tags)
                    .Include(card => card.Comments)
                    .Include(card => card.Votes)
                    .Include(card => card.Attachments)
                    .OrderBy(card => card.Index);
                var cardGroups = new Dictionary<string, List<CardData>>();
                // TODO: This is derpy and serial, but the EF Core Include seems to have a bug in it when the queries run simultanesouly
                // which duplicates Tags in CardData
                foreach (var group in query.Groups)
                {
                    cardGroups[group.Key] = await FilterCards(baseQuery, group.Value).ToListAsync();
                }
                return new CardsResponse()
                {
                    Cards = cardGroups,
                    Columns = await db.Columns.BoardFilter().OrderBy(col => col.Index).ToListAsync(),
                    Tags = await db.Tags.BoardFilter().OrderBy(tag => tag.Id).ToListAsync(),
                    Users = await db.Users
                    .Where(u => u.Roles.Any(p => p.BoardId == boardId) || u.Votes.Any(v => v.BoardId == boardId) || u.Comments.Any(c => c.BoardId == boardId))
                    .ToListAsync(),
                };
            });
        }
    }
}