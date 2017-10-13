using Gamebook.Web.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gamebook.Web.Models.Book
{
    using AutoMapper;
    using Gamebook.Data.Model;

    public class BookViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string AuthorEmail { get; set; }

        public int CatalogueNumber { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool isDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Book, BookViewModel>()
                .ForMember(bookViewModel => bookViewModel.AuthorEmail, cfg => cfg.MapFrom(book => book.Author.Email))
                .ForMember(bookViewModel => bookViewModel.CreatedOn, cfg => cfg.MapFrom(book => book.CreatedOn));
        }
    }
}