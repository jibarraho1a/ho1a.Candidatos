using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.services.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace ho1a.reclutamiento.services.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification([NotNull] Expression<Func<T, bool>> criteria)
        {
            Contract.Requires(criteria != null);

            this.Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude([NotNull] Expression<Func<T, object>> includeExpression)
        {
            Contract.Requires(includeExpression != null);

            this.Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            this.IncludeStrings.Add(includeString);
        }
    }
}
