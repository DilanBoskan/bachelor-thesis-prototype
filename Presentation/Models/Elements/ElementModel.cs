using Domain.Entities.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models.Elements;
public abstract class ElementModel(ElementId id, DateTime creationDate) : ObservableObjectWithResources {
    public ElementId Id { get; } = id;
    public DateTime CreationDate { get; } = creationDate;
}
