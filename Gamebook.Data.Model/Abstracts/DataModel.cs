using Gamebook.Data.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamebook.Data.Model.Abstracts
{
    public abstract class DataModel : IDeletable, IAuditable
    {
        public Guid Id { get; set; }

        public DataModel()
        {
            this.Id = Guid.NewGuid();
        }

        [Index]
        public bool isDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
