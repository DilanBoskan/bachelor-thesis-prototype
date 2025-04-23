using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Command;
public interface ICommand<TResult> : ICommand;

public interface ICommand;
