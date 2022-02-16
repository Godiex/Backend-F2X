using System.ComponentModel.DataAnnotations;
using Application.Collection.Queries.SimpleQuery;
using MediatR;

namespace Application.Collection.Queries.Report;

    public record CollectionReportQuery(
        [Required] DateTime Date
    ) : IRequest<List<List<CollectionReportDto>>>;