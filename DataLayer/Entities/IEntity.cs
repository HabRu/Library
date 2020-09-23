using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
