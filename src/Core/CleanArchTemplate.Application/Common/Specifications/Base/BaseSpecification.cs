﻿using CleanArchTemplate.Application.Common.Extensions;
using CleanArchTemplate.Domain;

using System.Linq.Expressions;

namespace CleanArchTemplate.Application.Common.Specifications.Base
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class, IEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = default!;
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query)
        {
            return Criteria = Criteria == null ? query : Criteria.And(query);
        }

        public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query)
        {
            return Criteria = Criteria == null ? query : Criteria.Or(query);
        }
    }
}