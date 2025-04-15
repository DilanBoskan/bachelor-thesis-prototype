﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Messages;
public interface IMessagePublisher {
    void Publish(IMessage message);

    void Flush();
}
